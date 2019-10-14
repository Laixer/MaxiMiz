using Newtonsoft.Json.Linq;
using Poller.Helper;
using Poller.Taboola;
using System;
using System.Collections.Generic;
using System.IO;

namespace Poller.Test.Taboola.Utility
{

    /// <summary>
    /// Handles our configuration import for us.
    /// </summary>
    public class TestConfigurationProvider
    {

        /// <summary>
        /// Imported configuration file.
        /// </summary>
        private JObject _config;

        /// <summary>
        /// Constructor that initializes the configuration file.
        /// </summary>
        /// <remarks>This throws if we can't find the file.</remarks>
        /// <param name="fileName">The filename</param>
        public TestConfigurationProvider(string fileName)
        {
            try
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    _config = Json.Deserialize<JObject>(json);
                }
            }
            catch (Exception e)
            {
                throw new FileNotFoundException($"Error while accessing test configuration " +
                    $"file at {fileName}. Discovered exception message: {e.Message}.");
            }
        }

        /// <summary>
        /// Gets a connection string for us.
        /// </summary>
        /// <remarks>Throws if not found</remarks>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetConnectionString(string name)
        {
            var connectionStrings = _config.GetValue("ConnectionStrings") as JObject;
            var result = connectionStrings.GetValue(name).ToString();

            if (result.Equals(null)) { throw new KeyNotFoundException($"Cound not find connectionstring for key = {name}"); }
            return result;
        }

        /// <summary>
        /// Extracts a properly formatted TaboolaPollerOptions object from our
        /// test configuration json file.
        /// </summary>
        /// <returns>The options object</returns>
        public TaboolaPollerOptions GenerateTaboolaPollerOptions()
        {
            var obj = _config.GetValue("Taboola") as JObject;
            var oauth = obj.GetValue("OAuth2") as JObject;

            return new TaboolaPollerOptions
            {
                BaseUrl = GetString(obj, "BaseUrl"),
                OAuth2 = new Model.OAuth2
                {
                    ClientId = GetString(oauth, "ClientId"),
                    ClientSecret = GetString(oauth, "ClientSecret"),
                    Username = GetString(oauth, "Username"),
                    Password = GetString(oauth, "Password"),
                    GrantType = "password"
                },
                CreateOrUpdateObjectsEventBus = GetConnectionString("MaximizServiceBus")
            };
        }

        /// <summary>
        /// Gets a string value based on a key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The string value</returns>
        private string GetString(JObject obj, string key)
        {
            try
            {
                return obj.GetValue(key).ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
