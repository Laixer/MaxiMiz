using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Poller.Taboola.Model;

using AccountEntity = Maximiz.Model.Entity.Account;
using CampaignEntity = Maximiz.Model.Entity.Campaign;
using AdItemEntity = Maximiz.Model.Entity.AdItem;
using Poller.Helper;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Poller.Taboola
{

    /// <summary>
    /// This is the part of our Taboola Poller that uses http. All CRUD
    /// operations are placed within this file.
    /// </summary>
    internal partial class TaboolaPoller
    {

   
        /// <summary>
        /// Creates a new campaign in the Taboola API, based on a campaign in
        /// our database. All known parameters will be sent to Taboola. After 
        /// creation the Taboola object is converted and returned, along with
        /// the original GUID.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="campaign">The core campaign</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>A new campaign object converted from the created campaign
        /// along with the correct GUID</returns>
        private async Task<CampaignEntity> CreateCampaignAsync(AccountEntity account,
            CampaignEntity campaign, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/";
            var converted = _mapperCampaign.Convert(campaign);
            var content = BuildStringContent(converted);

            var createdCampaign = await RemoteExecuteAndLogAsync<Campaign>(HttpMethod.Post, endpoint, content, token);
            var convertedCampaign = _mapperCampaign.Convert(createdCampaign);
            convertedCampaign.Id = campaign.Id;
            return convertedCampaign;
        }

        /// <summary>
        /// This first creates the ad item, then waits for Taboola to set
        /// the ad item status to anything but crawling. This method can 
        /// take a long time because we have to validate whether or not 
        /// the ad item was created. 
        /// 
        /// The created ad item is converted and inserted into our own 
        /// database after its creation.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="adItem">The ad item</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The created ad item entity</returns>
        private async Task<AdItemEntity> CreateAdItemAsync(AccountEntity account, AdItemEntity adItem,
            CancellationToken token)
        {
            // Get campaign this belongs to
            var campaignExternalId = GetLocalCampaignFromGuidAsync(adItem.CampaignGuid, token).Result.SecondaryId;

            // First create the ad item
            var endpoint = $"api/1.0 /{account.Name}/campaigns/{campaignExternalId}/items/";
            var content = BuildStringContent(adItem.TargetUrl);
            var createdAdItem = RemoteExecuteAndLogAsync<AdItem>(HttpMethod.Post,
                endpoint, content, token).Result;

            // Validate if the ad item has been created --> not crawling.
            var createdWithFields = await ValidateAdItemAsync(account, campaignExternalId, createdAdItem, token);
            await CommitAdItem(createdWithFields, token);
            return createdWithFields;
        }

        /// <summary>
        /// Keeps on polling the API to validate that our created ad item has
        /// been validated and left the crawling state. This operation can
        /// take a long time.
        /// </summary>
        /// <remarks>This throws after 30 seconds without result</remarks>
        /// <param name="account">The account</param>
        /// <param name="campaignId">The numeric id as string of the campaign
        /// this ad item belolngs to</param>
        /// <param name="createdAdItem">The created Taboola ad item</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The validated ad item</returns>
        private async Task<AdItemEntity> ValidateAdItemAsync(AccountEntity account,
            string campaignId, AdItem createdAdItem, CancellationToken token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.IsRunning)
            {
                var result = GetCampaignAllItemsAsync(account, campaignId, token).Result.Items;
                var selected = result.Where(x => x.Id == createdAdItem.Id).FirstOrDefault();

                // Not crawling means we are done
                if (selected != null && selected.CampaignItemStatus != CampaignItemStatus.Crawling)
                {
                    return _mapperAdItem.Convert(selected);
                }

                // Else we wait
                else { await Task.Delay(2500); }

                // Set stopwatch boundary
                if (stopwatch.ElapsedMilliseconds > 30000)
                {
                    stopwatch.Stop();
                    break;
                }
            }

            throw new TimeoutException($"Ad item with id = {createdAdItem} " +
                $"was not created after 30 seconds, aborting.");
        }

        /// <summary>
        /// Updates a given campaign from our core database to the taboola API.
        /// The campaign entity MUST have a GUID attached to it.
        /// </summary>
        /// <param name="account">Account to which the campaign belongs</param>
        /// <param name="campaign">Campaign with parameters</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        private async Task<Campaign> UpdateCampaignAsync(AccountEntity account,
            CampaignEntity campaign, CancellationToken token)
        {
            // Throw if we don't have a valid GUID
            ValidateGuid(campaign);

            var converted = _mapperCampaign.Convert(campaign);
            var content = BuildStringContent(converted, true);
            var contentString = content.ReadAsStringAsync();
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaign.SecondaryId}";

            // TODO intermediate result
            var result = await RemoteExecuteAndLogAsync<Campaign>(
                HttpMethod.Post, endpoint, content, token);
            return result;
        }

        /// <summary>
        /// Updates an ad item in the Taboola API.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="adItem">The ad item</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task UpdateAdItemAsync(AccountEntity account, AdItemEntity adItem,
            CancellationToken token)
        {
            var content = BuildStringContent(adItem);
            var campaignId = await GetLocalCampaignFromGuidAsync(adItem.CampaignGuid, token);
            var endpoint = $"api/1.0/{account}/campaigns/{campaignId}/{adItem.SecondaryId}/";
            await RemoteExecuteAndLogAsync(HttpMethod.Post, endpoint, content, token);
        }

        /// <summary>
        /// Deletes a campaign from the Taboola API.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="campaign">The campaign</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task<Campaign> DeleteCampaignAsync(AccountEntity account,
            CampaignEntity campaign, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaign.SecondaryId}/";
            return await RemoteExecuteAndLogAsync<Campaign>(HttpMethod.Delete, endpoint, null, token);
        }

        /// <summary>
        /// Deletes an ad item from the Taboola API.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="adItem">The ad item to remove</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task DeleteAdItemAsync(AccountEntity account, AdItemEntity adItem,
            CancellationToken token)
        {
            var campaign = await GetLocalCampaignFromGuidAsync(adItem.CampaignGuid, token);
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaign.SecondaryId}/items/{adItem.SecondaryId}";
            await RemoteExecuteAndLogAsync(HttpMethod.Delete, endpoint, null, token);
        }

      

        /// <summary>
        /// Does a proper encoding of any object. This returns a JSON string
        /// content object with UTF-8 encoding and application/json added to it. 
        /// This also sets all read-only parameters to null to prevent errors   
        /// in the Taboola API.
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <returns>Stringcontent object</returns>
        private StringContent BuildStringContent(object obj, bool sanetizeForUpdate = false)
        {
            // Nullify all read only parameters
            var serialized = Json.Serialize(obj);
            var parsed = JObject.Parse(serialized);
            NullifyReadOnlyCampaign(parsed);

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
