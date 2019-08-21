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
        public IDbConnection CreateConnection => new NpgsqlConnection(_configuration.GetConnectionString("MaxiMizDatabase"));
        
        /// <summary>
        /// Returns a new instance of <see cref="QueueClient"/> for the MaxiMiz service bus
        /// </summary>
        public IQueueClient CreateQueueClient => new QueueClient(_configuration.GetConnectionString("MaxiMizServiceBus"), "testqueue");

        public void Create(Campaign entity)
        {
            var message = new CreateOrUpdateObjectsMessage(entity, CrudAction.Create);

            var qClient = CreateQueueClient;

            var bf = new BinaryFormatter();
            using (var stream = new MemoryStream()) {
                bf.Serialize(stream, message);
                // Send message to SB
                qClient.SendAsync(new Message(stream.ToArray()));
            }

            throw new NotImplementedException();

            string sql = @"INSERT INTO public.campaign(
	id, secondary_id, name, branding_text, location_include, location_exclude, language, device, os, initial_cpc, budget, budget_daily, budget_model, delivery, bid_strategy, start_date, end_date, utm, status, create_date, update_date, delete_date, note, campaign_group, connection)
	VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);";

            using (IDbConnection connection = CreateConnection) {
                int rowsAffected = connection.Execute(sql, new { name=entity.Name, branding_text=entity.BrandingText });
            }
        }

        public void Delete(Campaign entity)
        {
            throw new NotImplementedException();
        }

        public Campaign Get(Guid id)
        {
            using (IDbConnection connection = CreateConnection) {
                IEnumerable<Campaign> result = connection.Query<Campaign>(@"
                    SELECT * FROM campaign WHERE id = @Id", new { Id = id });
                return result.FirstOrDefault();
            }
        }

        public IEnumerable<Campaign> GetAll()
        {
            // TODO: Limited to 100 for now
            using (IDbConnection connection = CreateConnection) {
                IEnumerable<Campaign> result = connection.Query<Campaign>($"SELECT * FROM campaign LIMIT {100}");
                return result.ToList();
            }
        }

        public IEnumerable<Campaign> Search(string q)
        {
            using (IDbConnection connection = CreateConnection) {
                // TODO: Fix/optimize query
                // TODO: Which columns should be tested? Name and Branding_text only?
                IEnumerable<Campaign> result =
                    connection.Query<Campaign>(
                    @"SELECT * FROM campaign WHERE name ~* @SearchQuery OR branding_text ~* @SearchQuery;",
                    new { SearchQuery = q }
                    );

                return result;
            }
        }

        public void Update(Campaign entity)
        {
            object @params = new
            {
                entity.Name,
                entity.BrandingText,
                entity.InitialCpc
            };

            using (IDbConnection connection = CreateConnection) {
                //connection.Update(campaign);
            }
            throw new NotImplementedException();
        }
    }
}
