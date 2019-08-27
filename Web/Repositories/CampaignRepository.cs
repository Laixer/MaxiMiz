using Dapper;
using Maximiz.Model;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Model.Protocol;
using Maximiz.Repositories.Interfaces;
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
        public IDbConnection GetConnection => new NpgsqlConnection(_configuration.GetConnectionString("MaxiMizDatabase"));

        /// <summary>
        /// Returns a new instance of <see cref="QueueClient"/> for the MaxiMiz service bus
        /// </summary>
        public IQueueClient GetQueueClient => new QueueClient(_configuration.GetConnectionString("MaxiMizServiceBus"), "testqueue");

        public async Task CreateGroup(CampaignGroup campaignGroupEntity)
        {
            var message = new CreateOrUpdateObjectsMessage(campaignGroupEntity, CrudAction.Create);

            var qClient = GetQueueClient;

            var bf = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                bf.Serialize(stream, message);
                // Send message to SB
                await qClient.SendAsync(new Message(stream.ToArray()));
            }
            //!

            // SQL query to insert campaign group and select inserted row ID
            var sql_group_insert = @" 
            INSERT INTO PUBLIC.campaign_group
                (NAME, branding_text, location_include, location_exclude, language, device, os, initial_cpc, budget, budget_daily, budget_model, delivery, bid_strategy, start_date, end_date, connection)
                VALUES  (
                    @Name,
                    @BrandingText,
                    @LocationInclude, @LocationExclude,
                    @Language,
                    @Device, 
                    @OS,
                    @InitialCPC,
                    @Budget, @BudgetDaily, @BudgetModel::budget_model,
                    @Delivery::delivery,
                    @BidStrategy::bid_strategy,
                    COALESCE(@StartDate, CURRENT_TIMESTAMP),
                    @EndDate,
                    @Connection
                )
                RETURNING id";

            var sql_group_params = new
            {
                campaignGroupEntity.Name,
                campaignGroupEntity.BrandingText,
                campaignGroupEntity.LocationInclude,
                campaignGroupEntity.LocationExclude,
                Language = new string[] { "N", "L" },
                campaignGroupEntity.Device,
                campaignGroupEntity.OS,
                campaignGroupEntity.InitialCpc,
                campaignGroupEntity.Budget,
                BudgetDaily = campaignGroupEntity.DailyBudget,
                BudgetModel = campaignGroupEntity.BudgetModel.GetEnumMemberName(),
                Delivery = campaignGroupEntity.Delivery.GetEnumMemberName(),
                BidStrategy = campaignGroupEntity.BidStrategy.GetEnumMemberName(),
                campaignGroupEntity.StartDate,
                campaignGroupEntity.EndDate,
                campaignGroupEntity.Connection
            };

            // SQL query to insert a new campaign
            var sql_campaign_insert = @"INSERT INTO
	                public.campaign(secondary_id, name, branding_text, location_include, location_exclude, language, device, os, initial_cpc, budget, budget_daily, budget_model, delivery, start_date, end_date, utm, connection, campaign_group)
                VALUES
                    (
                        @SecondaryId,
                        @Name,
                        @BrandingText,
                        '{0}',
                        '{0}',
                        @Language,
                        @Device,
                        @OS,
                        @InitialCpc,
                        @Budget,
                        @BudgetDaily,
                        @BudgetModel::budget_model,
                        @Delivery::delivery,
                        COALESCE(@StartDate, CURRENT_TIMESTAMP),
                        @EndDate,
                        @Utm,
                        @Connection,
                        @CampaignGroupId
                    );";

            // For temporary storage of the campaigns to create
            List<Campaign> campaignsToCreate = new List<Campaign>();

            Parallel.ForEach(campaignGroupEntity.Device, device =>
            {
                Parallel.ForEach(campaignGroupEntity.OS, os =>
                {
                    Parallel.ForEach(campaignGroupEntity.LocationInclude, location =>
                    {
                        // Generate the campaign
                        Campaign campaign = Campaign.FromGroup(campaignGroupEntity);
                        campaign.Device = new Device[] { device };
                        campaign.OS = new OS[] { os };
                        campaign.LocationInclude = new int[1] { location };

                        campaignsToCreate.Add(campaign);
                    });
                });
            });

            var gen = campaignsToCreate;

            using (IDbConnection connection = GetConnection)
            {
                connection.Open();

                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    // Create a new campaign group record.
                    int newCampaignGroupId = await connection.ExecuteScalarAsync<int>(sql_group_insert, sql_group_params, transaction: transaction);

                    // Insert the generated campaigns.
                    foreach (Campaign campaign in campaignsToCreate)
                    {
                        var @params = new
                        {
                            SecondaryId = Guid.NewGuid().ToString().Substring(0,16),
                            CampaignGroupId = newCampaignGroupId,
                            campaign.Name,
                            campaign.BrandingText,
                            campaign.LocationInclude,
                            campaign.LocationExclude,
                            Language = new string[] { "N", "L" },   // TODO: For some reason length must be 3
                            campaign.Device,
                            campaign.OS,               
                            campaign.InitialCpc,
                            campaign.Budget,
                            BudgetDaily = campaign.DailyBudget,
                            BudgetModel = campaign.BudgetModel.GetEnumMemberName(),
                            Delivery = campaign.Delivery.GetEnumMemberName(),
                            BidStrategy = campaign.BidStrategy.GetEnumMemberName(),
                            campaign.StartDate,
                            campaign.EndDate,
                            campaign.Utm,
                            campaign.Connection
                            // TODO finish
                        };

                        await connection.ExecuteAsync(sql_campaign_insert,@params,transaction: transaction);
                    }

                    transaction.Commit();
                }
            }
        }

        Task IEntityRepository<Campaign, Guid>.Create(Campaign entity)
        {
            throw new NotImplementedException();
        }

        Task IEntityRepository<Campaign, Guid>.Delete(Campaign entity)
        {
            throw new NotImplementedException();
        }

        async Task<Campaign> IEntityRepository<Campaign, Guid>.Get(Guid id)
        {
            using (IDbConnection connection = GetConnection)
            {
                var result = await connection.QueryFirstOrDefaultAsync<Campaign>(@"
                    SELECT * FROM campaign WHERE id = @Id", new { Id = id });
                return result;
            }
        }

        async Task<IEnumerable<Campaign>> ICampaignRepository.GetAll()
        {
            // TODO: Limited to 100 for now
            using (IDbConnection connection = GetConnection)
            {
                var result = await connection.QueryAsync<Campaign>(@"SELECT * FROM campaign ORDER BY create_date DESC LIMIT 100");
                return result;
            }
        }

        async Task<IEnumerable<Campaign>> ICampaignRepository.Search(string q)
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

        async Task IEntityRepository<Campaign, Guid>.Update(Campaign entity)
        {
            //TODO
            object @params = new
            {
                entity.Name,
                entity.BrandingText,
                entity.InitialCpc
            };

            using (IDbConnection connection = GetConnection)
            {
                //connection.Update(campaign);
            }
            throw new NotImplementedException();
        }
    }
}
