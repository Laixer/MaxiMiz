        /// <summary>
        /// Performs syncback over a single campaign.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="campaign">The campaign</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task SyncbackCampaignAsync(AccountEntity account,
            CampaignEntity campaign, CancellationToken token)
        {
            // First syncback the campaign
            var campaignApi = await GetCampaignAsync(account, campaign.SecondaryId, token);
            var converted = _mapperCampaign.Convert(campaignApi);
            var list = new List<CampaignEntity>();
            list.Add(converted);
            await CommitCampaigns(list, token);

            // Then syncback all ad items
            var result = await GetCampaignAllItemsAsync(account, campaign, token);
            var items = _mapperAdItem.ConvertAll(result.Items);
            await CommitAdItems(items, token);
        }
