using Dapper;
using Dapper.Contrib.Extensions;
using Maximiz.InputModels.Campaigns;
using Maximiz.Model;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Model.Protocol;
using Maximiz.Repositories.Interfaces;
using Maximiz.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Maximiz.Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly IConfiguration _configuration;

        public CampaignRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Returns a new instance of <see cref="NpgsqlConnection"/> for the MaxiMiz database
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection => new NpgsqlConnection(_configuration.GetConnectionString("MaxiMizDatabase"));

        /// <summary>
        /// Returns a new instance of <see cref="QueueClient"/> for the MaxiMiz service bus
        /// </summary>
        /// <returns></returns>
        public IQueueClient GetQueueClient => new QueueClient(_configuration.GetConnectionString("MaxiMizServiceBus"), "testqueue");

        /// <summary>
        /// Inserts a new campaign group and campaigns into the database and sends commands to the service bus.
        /// </summary>
        /// <param name="campaignGroup">Model to insert</param>
        /// <returns></returns>
        public async Task CreateGroup(CampaignGroup campaignGroup)
        {
            // SQL query to insert campaign group and select inserted row ID
            var sql_group_insert = @" 
            INSERT INTO PUBLIC.campaign_group
                (NAME, branding_text, location_include, location_exclude, language, device, os, initial_cpc, budget, budget_daily, budget_model, delivery, bid_strategy, start_date, end_date, connection)
                VALUES  (
                    @Name,
                    @BrandingText,
                    @LocationInclude, @LocationExclude,
                    '{NL}', @Device, @OS,
                    @InitialCPC,
                    @Budget, @DailyBudget, @BudgetModelText::budget_model,
                    @DeliveryText::delivery,
                    @BidStrategyText::bid_strategy,
                    COALESCE(@StartDate, CURRENT_TIMESTAMP),
                    @EndDate,
                    @Connection
                )
                RETURNING id";

            // SQL query to insert a new campaign
            string sql_campaign_insert = @"INSERT INTO
	                public.campaign(secondary_id, campaign_group, name, branding_text, location_include, location_exclude, language, language_as_text, device, os, initial_cpc, budget, budget_daily, budget_model, delivery, start_date, end_date, utm, connection)
                VALUES
                    (
                        @SecondaryId,
                        @CampaignGroup,
                        @Name,
                        @BrandingText,
                        '{0}','{0}',
                        '{NL}',
                        @Language, @Device, @OS,
                        @InitialCpc,
                        @Budget, @DailyBudget, @BudgetModelText::budget_model,
                        @DeliveryText::delivery,
                        COALESCE(@StartDate, CURRENT_TIMESTAMP),
                        @EndDate,
                        @Utm,
                        @Connection
                    );";

            // For temporary storage of the campaigns to create
            List<Campaign> campaignsToCreate = new List<Campaign>();

            Parallel.ForEach(campaignGroup.Device, device =>
            {
                Parallel.ForEach(campaignGroup.Os, os =>
                {
                    Parallel.ForEach(campaignGroup.LocationInclude, location =>
                    {
                        // Generate the campaign
                        Campaign campaign = Campaign.FromGroup(campaignGroup);
                        campaign.Device = new Device[] { device };
                        campaign.Os = new OS[] { os };
                        campaign.LocationInclude = new int[1] { location };

                        campaign.Name = CampaignNameGenerator.Generate(campaignGroup.Name, campaign.Language, location.ToString(), os, device);

                        campaignsToCreate.Add(campaign);
                    });
                });
            });

            using (IDbConnection connection = GetConnection)
            {
                connection.Open();

                IDbTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Create a new campaign group record.
                    int newCampaignGroupId = await connection.ExecuteScalarAsync<int>
                        (sql_group_insert, campaignGroup, transaction: transaction);

                    campaignGroup.Id = newCampaignGroupId;

                    // Insert the campaigns.
                    foreach (Campaign campaign in campaignsToCreate)
                    {
                        campaign.CampaignGroup = newCampaignGroupId;
                        campaign.SecondaryId = $"TEST{Guid.NewGuid().ToString().Substring(0, 8)}";

                        await connection.ExecuteAsync(sql_campaign_insert, campaign, transaction: transaction);
                    }

                    // Send create messages to the service bus.
                    await ServiceBusQueue.SendObjectMessage(campaignGroup, CrudAction.Create);
                    await ServiceBusQueue.SendObjectMessages(campaignsToCreate, CrudAction.Create);

                    transaction.Commit();
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        public Task<Guid> Create(Campaign entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Campaign entity)
        {
            using (IDbConnection connection = GetConnection)
            {
                await connection.DeleteAsync(entity);
            }
        }

        public async Task<Campaign> Get(Guid id)
        {
            using (IDbConnection connection = GetConnection)
            {
                var result = await connection.QueryFirstOrDefaultAsync<Campaign>(@"
                    SELECT * FROM campaign WHERE id = @Id", new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<Campaign>> GetAll()
        {
            // TODO: Limited to 100 and sorted by latest date desc for now
            using (IDbConnection connection = GetConnection)
            {
                var result = await connection.QueryAsync<Campaign>(@"SELECT * FROM campaign ORDER BY create_date DESC LIMIT 100");
                return result;
            }
        }

        public async Task<IEnumerable<Campaign>> Search(string q)
        {
            using (IDbConnection connection = GetConnection)
            {
                // TODO: Fix/optimize query
                IEnumerable<Campaign> result =
                    await connection.QueryAsync<Campaign>(
                    @"SELECT * FROM campaign WHERE name ~* @SearchQuery OR branding_text ~* @SearchQuery;",
                    new { SearchQuery = q }
                    );

                return result;
            }
        }

        // TODO: Fix mapping for Dapper Contrib
        public async Task<Campaign> Update(Campaign entity)
        {
            using (IDbConnection connection = GetConnection)
            {
                bool success = await connection.UpdateAsync(entity);

                if (success)
                {
                    return entity;
                }

                //TODO: Update failed
                return null;
            }
        }

    }
}
