using Newtonsoft.Json.Linq;
using Poller.Helper;
using Poller.Taboola.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Poller.Taboola.Traffic
{

    /// <summary>
    /// Used to create content for http request messages. This is tuned to work
    /// with the Taboola API.
    /// </summary>
    internal class ContentBuilder
    {

        /// <summary>
        /// Does a proper encoding of any object. This returns a JSON string
        /// content object with UTF-8 encoding and application/json added to it. 
        /// This also sets all read-only parameters to null to prevent errors   
        /// in the Taboola API.
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <returns>Stringcontent object</returns>
        public StringContent BuildStringContent(object obj, bool sanetizeForUpdate = false)
        {
            // Nullify all read only parameters
            var serialized = Json.Serialize(obj);
            var parsed = JObject.Parse(serialized);
            // TODO This might clash with similarly named parameters
            NullifyReadOnlyCampaign(parsed);
            NullifyReadOnlyAdItem(parsed);

            // If we are doing an update call
            if (sanetizeForUpdate)
            {
                // Target sanetizing
                // TODO Not very elegant
                if (obj as Campaign != null)
                {
                    SanetizeCampaignTarget(parsed, "country_targeting");
                    SanetizeCampaignTarget(parsed, "contextual_targeting");
                    SanetizeCampaignTarget(parsed, "platform_targeting");

                    // TODO Beun fix
                    RemoveIfPresent(parsed, "sub_country_targeting"); // This should be country dependent, as this can only exist when we target a specific country
                }

            }

            serialized = Json.Serialize(parsed);
            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Removes all read-only keys from the object. If the Taboola API
        /// receives a read-only field, it returns a 400 BAD REQUEST response.
        /// This also works for non-campaign objects.
        /// 
        /// TODO Safety?
        /// </summary>
        /// <param name="obj">The campaign object to sanetize</param>
        private void NullifyReadOnlyCampaign(JObject obj)
        {
            RemoveIfPresent(obj, "id");
            RemoveIfPresent(obj, "advertiser_id");
            RemoveIfPresent(obj, "spent");
            RemoveIfPresent(obj, "status");
            RemoveIfPresent(obj, "approval_state");
            RemoveIfPresent(obj, "postal_code_targeting");
        }

        private void NullifyReadOnlyAdItem(JObject obj)
        {
            throw new NotImplementedException("Implement ad item sanetization!");
        }

        /// <summary>
        /// Sanetize a campaign target object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        private void SanetizeCampaignTarget(JObject obj, string key)
        {
            if (obj.ContainsKey(key))
            {
                obj[key] = SanetizeTarget(obj[key] as JObject);
            }
        }

        /// <summary>
        /// If the value array is empty, we set the array to null.
        /// TODO Also during INCLUDE or EXCLUDE?
        /// TODO This seems like a Taboola bug.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private JObject SanetizeTarget(JObject obj)
        {
            if (obj.ContainsKey("type") && obj.ContainsKey("value") &&
                obj.GetValue("type").ToString().Equals("ALL"))
            {
                obj["value"] = null;
            }
            return obj;
        }

        /// <summary>
        /// Removes a key-value combination if the key is present.
        /// </summary>
        /// <param name="obj">The JSON object</param>
        /// <param name="key">The key</param>
        private void RemoveIfPresent(JObject obj, string key)
        {
            if (obj.ContainsKey(key)) { obj.Remove(key); }
        }

    }
}
