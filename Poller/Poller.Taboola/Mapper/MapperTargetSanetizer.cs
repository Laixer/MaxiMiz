using Newtonsoft.Json.Linq;
using Poller.Helper;
using Poller.Taboola.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using CampaignInternal = Maximiz.Model.Entity.Campaign;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Used to sanetize our target objects within campaigns.
    /// </summary>
    internal partial class MapperTarget
    {

        /// <summary>
        /// Converts the OsTarget target type to the correct subtype for all 
        /// in the specified list.
        /// </summary>
        /// <param name="input">Campaign list</param>
        /// <returns>Converted campaign list</returns>
        internal IEnumerable<Campaign> ConvertAll(IEnumerable<Campaign> input)
        {
            IList<Campaign> result = new List<Campaign>();
            foreach (var campaign in input.AsParallel())
            {
                result.Add(ConvertTarget(campaign));
            }
            return result;
        }

        /// <summary>
        /// Converts the OsTargeting target type to the correct subtype. This 
        /// can be either of two:
        /// <see cref="TargetDefault"/> or
        /// <see cref="TargetOsFamily"/>.
        /// TODO This should be cleaner.
        /// </summary>
        /// <param name="input">Campaign</param>
        /// <returns>Campaign with converted os target</returns>
        internal Campaign ConvertTarget(Campaign input)
        {
            // Do nothing if conversion already happened
            if (input.OsTargeting is TargetDefault ||
                input.OsTargeting is TargetOsFamily) return input;

            // Convert
            try
            {
                var targetBase = (TargetBase)input.OsTargeting;
                var array = (JArray)Json.Deserialize<object>
                    (targetBase.Value.ToString());
                TargetBase targetConverted = null;

                // If we are os family type
                if (array.Count > 0 && array[0] is JObject)
                {
                    JObject jObject = (JObject)array[0];
                    if (jObject.ContainsKey("os_family") &&
                        jObject.ContainsKey("sub_categories"))
                    {

                        string osFamily = (string)jObject.GetValue("os_family");
                        JArray strings = (JArray)jObject.GetValue("sub_categories");
                        string[] subCategories = strings.ToObject<string[]>();

                        targetConverted = new TargetOsFamily
                        {
                            Type = targetBase.Type,
                            Href = targetBase.Href,
                            OsFamily = osFamily,
                            SubCategories = subCategories
                        };
                    }
                    else throw new InvalidOperationException(
                        "Json object does not contain the required " +
                        "properties to convert to TargetOsFamily");
                }

                // If we are string array type
                // TODO Might want to failsafe this
                else
                {
                    string[] strings = array.ToObject<string[]>();
                    targetConverted = new TargetDefault
                    {
                        Type = targetBase.Type,
                        Href = targetBase.Href,
                        Value = strings
                    };
                }

                // Apply and return
                input.OsTargeting = targetConverted;
                return input;

            }
            catch (Exception)
            {
                throw new InvalidOperationException(
                    "Could not convert OsTarget type for given campaign");
            }
        }

        internal void ConvertAndApply(CampaignInternal campaignInternal, Campaign campaignExternal)
        {
            throw new NotImplementedException("Build converter for targeting");
        }

        internal void ConvertAndApply(Campaign campaignExternal, CampaignInternal campaignInternal)
        {
            throw new NotImplementedException("Build converter for targeting");
        }

        internal TResult[] ParseTarget<TResult>(TargetBase target) 
        {
            throw new NotImplementedException("Implement parsetarget");
        }

        private string RemoveBrackets(string input)
        {
            if (input[0] != '{' || input[input.Length - 1] != '}') return input;
            return input.Substring(1, input.Length - 2);
        }

    }
}
