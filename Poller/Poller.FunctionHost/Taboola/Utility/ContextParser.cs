using Maximiz.Model;
using Maximiz.Model.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Poller.Helper;
using Poller.Poller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Poller.FunctionHost.Taboola.Utility
{

    /// <summary>
    /// Attempts to convert a json string to a desired object.
    /// </summary>
    internal class ContextParser
    {

        /// <summary>
        /// Parses a crud context object. This is complex because of the entity
        /// array, which uses polymorphism. 
        /// TODO There has to be a more elegant fix.
        /// </summary>
        /// <param name="queueItem"></param>
        /// <returns></returns>
        internal CreateOrUpdateObjectsContext Parse(string queueItem)
        {
            // Deserialize
            var json = Json.Deserialize<JObject>(queueItem);

            // Parse all entities separately
            var jsonEntities = json.GetValue("Entity") as JArray;
            var entities = new List<Entity>();
            foreach (var item in jsonEntities) { entities.Add(ParseEntity(item.ToString())); }

            // Construct result and return
            // TODO This loses the callback action.
            var asPollerContext = Json.Deserialize<PollerContext>(queueItem);
            var asCrudcontext = new CreateOrUpdateObjectsContext(
                runCount: asPollerContext.RunCount,
                lastRun: asPollerContext.LastRun)
            {
                Action = ExtractProperty<CrudAction>(json, "Action"),
                Entity = entities.ToArray()
            };
            return asCrudcontext;
        }

        /// <summary>
        /// Attempts to parse an entity to the corresponding entity implementation.
        /// TODO This might be unsafe.
        /// TODO Make more elegant.
        /// </summary>
        /// <remarks>
        /// This throws an <see cref="ArgumentException"/> upon conversion error.
        /// </remarks>
        /// <param name="input">The input json string for a single entity</param>
        /// <returns>The converted entity</returns>
        private Entity ParseEntity(string input)
        {
            var jObject = Json.Deserialize<JObject>(input);

            if (IsType<Account>(jObject)) { return Json.Deserialize<Account>(input); }
            if (IsType<AdGroup>(jObject)) { return Json.Deserialize<AdGroup>(input); }
            if (IsType<AdItem>(jObject)) { return Json.Deserialize<AdItem>(input); }
            if (IsType<Campaign>(jObject)) { return Json.Deserialize<Campaign>(input); }
            if (IsType<CampaignGroup>(jObject)) { return Json.Deserialize<CampaignGroup>(input); }

            throw new ArgumentException($"Could not parse entity: {input}");
        }

        /// <summary>
        /// This compares all type names between a JObject and a given object.
        /// TODO This only compares names! This might be unsafe.
        /// </summary>
        /// <typeparam name="TType">The type to compare against</typeparam>
        /// <param name="jObject">The jobject</param>
        /// <returns>True if matches, which means we can deserialize</returns>
        private bool IsType<TType>(JObject jObject)
        {
            var type = typeof(TType);
            var propertiesCompare = type.GetProperties();
            var propertiesCompareNames = propertiesCompare.Select(x => x.Name);
            var propertiesJson = jObject.Properties();
            foreach (var property in jObject.Properties())
            {
                if (!propertiesCompareNames.Contains(property.Name))
                {
                    return false;
                }
            }


            return true;
        }

        /// <summary>
        /// Attempts to extract and parse a property from a json object.
        /// </summary>
        /// <typeparam name="TResult">The type to extract</typeparam>
        /// <param name="json">The json object</param>
        /// <param name="key">The key of the property</param>
        /// <returns>The converted property</returns>
        private TResult ExtractProperty<TResult>(JObject json, string key)
        {
            var count = json.Count;
            var asJToken = GetValueRecursive(json, key);
            var asString = asJToken.ToString();
            var asBytes = Encoding.ASCII.GetBytes(asString);
            var asStream = new MemoryStream(asBytes);
            using (var streamReader = new StreamReader(asStream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var result = new JsonSerializer().Deserialize<TResult>(jsonTextReader);
                return result;
            }
        }

        /// <summary>
        /// Gets a value in a json object recursively.
        /// TODO Is this even required?
        /// </summary>
        /// <remarks>
        /// This returns null if we can't find the key recursively.
        /// </remarks>
        /// <param name="json">The json object</param>
        /// <param name="key">The key to look for</param>
        /// <returns>The value as jtoken</returns>
        private JToken GetValueRecursive(JObject json, string key)
        {
            if (json.Property(key) != null) { return json.GetValue(key); }
            foreach (var node in json.Children())
            {
                return GetValueRecursive(json, key);
            }
            return null;
        }

    }
}
