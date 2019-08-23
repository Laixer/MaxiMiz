using Dapper;
using Maximiz.Model;
using Maximiz.Model.Entity;
using Maximiz.Model.Protocol;
using Maximiz.Repositories.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

        public async Task CreateGroup(CampaignGroup entity)
        {
            // TODO: Put this away somewhere else. This is temporary.
            var message = new CreateOrUpdateObjectsMessage(entity, CrudAction.Create);

            var qClient = GetQueueClient;

            var bf = new BinaryFormatter();
            using (var stream = new MemoryStream()) {
                bf.Serialize(stream, message);
                // Send message to SB
                await qClient.SendAsync(new Message(stream.ToArray()));
            }
            //!

            // TODO: Inline SQL is also temporary
            var sql = @" 
            INSERT INTO PUBLIC.campaign_group
                (NAME,
                 branding_text,
                 location_include,
                 location_exclude,
                 language,
                 device,
                 os,
                 initial_cpc,
                 budget,
                 budget_daily,
                 budget_model,
                 delivery,
                 bid_strategy,
                 start_date,
                 end_date,
                 status,
                 create_date,
                 update_date,
                 delete_date,
                 note,
                 connection)
                VALUES  (@Name,
                         @Branding_Text,
                         @Location_Include,
                         @Location_Exclude,
                         @Language,
                         @Device,
                         @OS,
                         @Initial_CPC,
                         @Budget,
                         @Budget_Daily,
                         @Budget_Model,
                         @Delivery,
                         @Bid_Strategy,
                         @Start_Date,
                         @End_Date,
                         @Status,
                         @Create_Date,
                         @Update_Date,
                         @Delete_Date,
                         @Note,
                         @Connection
                );";

            var parameters = new
            {
                Name = entity.Name,
                Branding_Text = entity.BrandingText,
                Location_Incldue = entity.LocationInclude
                // TODO ETC...
            };

            using (IDbConnection connection = GetConnection) {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

        async Task IEntityRepository<Campaign, Guid>.Create(Campaign entity)
        {
            throw new NotImplementedException();
        }

        async Task IEntityRepository<Campaign, Guid>.Delete(Campaign entity)
        {
            throw new NotImplementedException();
        }

        async Task<Campaign> IEntityRepository<Campaign, Guid>.Get(Guid id)
        {
            using (IDbConnection connection = GetConnection) {
                var result = await connection.QueryFirstOrDefaultAsync<Campaign>(@"
                    SELECT * FROM campaign WHERE id = @Id", new { Id = id });
                return result;
            }
        }

        async Task<IEnumerable<Campaign>> ICampaignRepository.GetAll()
        {
            // TODO: Limited to 100 for now
            using (IDbConnection connection = GetConnection) {
                return await connection.QueryAsync<Campaign>($"SELECT * FROM campaign LIMIT {100}");
            }
        }

        async Task<IEnumerable<Campaign>> ICampaignRepository.Search(string q)
        {
            using (IDbConnection connection = GetConnection) {
                // TODO: Fix/optimize query
                IEnumerable<Campaign> result =
                    connection.Query<Campaign>(
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

            using (IDbConnection connection = GetConnection) {
                //connection.Update(campaign);
            }
            throw new NotImplementedException();
        }
    }
}
