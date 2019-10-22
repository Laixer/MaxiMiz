using Maximiz.Database;
using Maximiz.Database.Querying;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories
{

    /// <summary>
    /// Repository implementation for our <see cref="Campaign"/>s.
    /// TODO Implement.
    /// TODO Doc.
    /// TODO Maybe implement with a cache?
    /// TODO Implement usable cancellation tokens.
    /// TODO Implement pages
    /// TODO Implement memory management in the future (<see cref="lastUpdatedMap"/>).
    /// </summary>
    internal class CampaignRepository : RepositoryBase, ICampaignRepository
    {

        /// <summary>
        /// Contains the <see cref="DateTime"/> at which each respective list
        /// was last queried from the data store. Lists that have never been
        /// updated are inserted with a null.
        /// </summary>
        private IDictionary<IEnumerable<Campaign>, DateTime?> lastUpdatedMap;

        private IEnumerable<CampaignWithStats> campaignsAll;
        private IEnumerable<CampaignWithStats> campaignsActive;
        private IEnumerable<CampaignWithStats> campaignsInactive;
        private IEnumerable<CampaignWithStats> campaignsPending;
        private IEnumerable<CampaignWithStats> campaignsHidden;
        private IEnumerable<CampaignWithStats> campaignsConcept;
        private IEnumerable<CampaignWithStats> campaignsDeleted;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="crudInternalWebClient"></param>
        public CampaignRepository(ICrudInternalWebClient crudInternalWebClient)
            : base(crudInternalWebClient)
        {
            campaignsAll = new List<CampaignWithStats>();
            campaignsActive = new List<CampaignWithStats>();
            campaignsInactive = new List<CampaignWithStats>();
            campaignsPending = new List<CampaignWithStats>();
            campaignsHidden = new List<CampaignWithStats>();
            campaignsConcept = new List<CampaignWithStats>();
            campaignsDeleted = new List<CampaignWithStats>();

            // Setup last updated dictionary
            // TODO Implement later
            lastUpdatedMap = new Dictionary<IEnumerable<Campaign>, DateTime?>();
            //lastUpdatedMap.Add(campaignsAll, null);
            //lastUpdatedMap.Add(campaignsActive, null);
            //lastUpdatedMap.Add(campaignsInactive, null);
            //lastUpdatedMap.Add(campaignsPending, null);
            //lastUpdatedMap.Add(campaignsHidden, null);
            //lastUpdatedMap.Add(campaignsConcept, null);
            //lastUpdatedMap.Add(campaignsDeleted, null);
        }

        /// <summary>
        /// Gets a campaign item based on its internal database id.
        /// </summary>
        /// <param name="id">The database <see cref="Guid"/></param>
        /// <returns>The campaign item</returns>
        public async Task<CampaignWithStats> Get(Guid id)
        {
            return await crudInternal.GetCampaignWithStatsAsync(id, source.Token);
        }

        /// <summary>
        /// Retrieves "all" campaigns from the data store.
        /// </summary>
        /// <param name="page">The page to return</param>
        /// <returns>List of campaigns</returns>
        public async Task<IEnumerable<CampaignWithStats>> GetAll(int page = 0)
        {
            var query = new Query<ColumnCampaignWithStats>(ColumnCampaignWithStats.Name);
            campaignsAll = await crudInternal.GetCampaignsAsync(query, page, source.Token);
            return campaignsAll;
        }

        /// <summary>
        /// Retrieves "all" active campaigns from the data store.
        /// </summary>
        /// <param name="page">The page to return</param>
        /// <returns>List of active campaigns</returns>
        public async Task<IEnumerable<CampaignWithStats>> GetActive(int page = 0)
        {
            var query = new Query<ColumnCampaignWithStats>(
                ColumnCampaignWithStats.Status,
                Order.Ascending,
                new EnumTranslator().Translate(CampaignStatus.Running));

            campaignsActive = await crudInternal.GetCampaignsAsync(query, page, source.Token);
            return campaignsActive;
        }

        /// <summary>
        /// Retrieves "all" inactive campaigns from the data store.
        /// </summary>
        /// <param name="page">The page to return</param>
        /// <returns>List of inactive campaigns</returns>
        public async Task<IEnumerable<CampaignWithStats>> GetInactive(int page)
        {
            var query = new Query<ColumnCampaignWithStats>(
                ColumnCampaignWithStats.Status,
                Order.Ascending,
                new QueryComposer().ComposeCampaignStatusInactive());

            campaignsActive = await crudInternal.GetCampaignsAsync(query, page, source.Token);
            return campaignsActive;
        }

        public async Task<IEnumerable<CampaignWithStats>> GetPending(int page)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CampaignWithStats>> GetHidden(int page)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CampaignWithStats>> GetConcept(int page)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CampaignWithStats>> GetDeleted(int page)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This will query the database for all required list. In this way we
        /// will always have our data ready and won't have the user waiting.
        /// 
        /// TODO Implement later, first get everything working.
        /// </summary>
        /// <returns>Task</returns>
        public async Task QueryOnLoad()
        {
            //
        }

        /// <summary>
        /// Checks whether or not we should re-query our database for a given list.
        /// TODO Implement, get things working first.
        /// </summary>
        /// <remarks>
        /// At the moment this will always return true.
        /// </remarks>
        /// <param name="list">The list to check</param>
        /// <return>True if we should re-query our database</returns>
        private bool ShouldUpdate(IEnumerable<CampaignWithStats> list)
        {
            // If we have never added the list before
            if (!lastUpdatedMap.Keys.Contains(list))
            {
                lastUpdatedMap.Add(list, null);
                return true;
            }

            // 
            var lastUpdated = lastUpdatedMap[list];
            if (lastUpdated == null) { return true; }
            else if (lastUpdated.Value.Ticks < MaxIntervalWithoutUpdatingSeconds)
            {

            }

            return true;
        }
    }
}
