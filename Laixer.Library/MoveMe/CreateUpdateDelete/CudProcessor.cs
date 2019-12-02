using Maximiz.Model;
using Maximiz.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Maximiz.Transactions.CreateUpdateDelete
{

    /// <summary>
    /// Implementation used to select the correct <see cref="ICud{TEntity}"/>
    /// interface for generic entity operations.
    /// </summary>
    internal class CudProcessor : ICudProcessor
    {

        private readonly ICud<Campaign> cudCampaign;
        private readonly ICud<CampaignGroup> cudCampaignGroup;
        private readonly ICud<AdItem> cudAdItem;
        private readonly ICud<AdGroup> cudAdGroup;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="campaign">Cud object for campaigns</param>
        /// <param name="campaignGroup">Cud object for campaign groups</param>
        /// <param name="adItem">Cud object for ad items</param>
        /// <param name="adGroup">Cud object for ad groups</param>
        public CudProcessor(ICud<Campaign> campaign, ICud<CampaignGroup> campaignGroup,
            ICud<AdItem> adItem, ICud<AdGroup> adGroup)
        {
            cudCampaign = campaign;
            cudCampaignGroup = campaignGroup;
            cudAdItem = adItem;
            cudAdGroup = adGroup;
        }

        /// <summary>
        /// Processes a create, update or delete request for a given entity.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        /// <param name="action"><see cref="CrudAction"/></param>
        /// <returns>The processed entity as returned from the database</returns>
        public async Task<Entity> ProcessOperationAsync(Entity entity, CrudAction action)
        {
            switch (action)
            {
                case CrudAction.Create:
                    return await Create(entity);
                case CrudAction.Update:
                    return await Update(entity);
                case CrudAction.Delete:
                    return await Delete(entity);
            }

            throw new ArgumentException($"The action to process should be create," +
                $"update or delete. Could not handle action: {action}");
        }

        /// <summary>
        /// Creates an entity using the correct CUD object.
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity as returned from the database</returns>
        private async Task<Entity> Create(Entity entity)
        {
            switch (entity)
            {
                case Campaign x:
                    return await cudCampaign.Create(x);
                case CampaignGroup x:
                    return await cudCampaignGroup.Create(x);
                case AdItem x:
                    return await cudAdItem.Create(x);
                case AdGroup x:
                    return await cudAdGroup.Create(x);
            }

            throw new ArgumentException($"Could not perform " +
                $"create operation on entity {entity.GetType()}");
        }

        /// <summary>
        /// Updates an entity using the correct CUD object.
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity as returned from the database</returns>
        private async Task<Entity> Update(Entity entity)
        {
            switch (entity)
            {
                case Campaign x:
                    return await cudCampaign.Update(x);
                case CampaignGroup x:
                    return await cudCampaignGroup.Update(x);
                case AdItem x:
                    return await cudAdItem.Update(x);
                case AdGroup x:
                    return await cudAdGroup.Update(x);
            }

            throw new ArgumentException($"Could not perform " +
                $"update operation on entity {entity.GetType()}");
        }

        /// <summary>
        /// Deletes an entity using the correct CUD object.
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity as returned from the database</returns>
        private async Task<Entity> Delete(Entity entity)
        {
            switch (entity)
            {
                case Campaign x:
                    return await cudCampaign.Delete(x);
                case CampaignGroup x:
                    return await cudCampaignGroup.Delete(x);
                case AdItem x:
                    return await cudAdItem.Delete(x);
                case AdGroup x:
                    return await cudAdGroup.Delete(x);
            }

            throw new ArgumentException($"Could not perform " +
                $"delete operation on entity {entity.GetType()}");

        }
    }
}
