﻿using Dapper;
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
        
        public IDbConnection Connection {
            get {
                return new NpgsqlConnection(_configuration.GetConnectionString("MaxiMizDatabase"));
            }
        }

        public IQueueClient QueueClient {
            get {
                return new QueueClient(_configuration.GetConnectionString("MaxiMizServiceBus"), "testqueue");
            }
        }

        public async Task CreateCampaignTest(Campaign entity)
        {
            // TODO: Create a separate method for sending an entity and crud action to the SB
            var message = new CreateOrUpdateObjectsMessage(entity, CrudAction.Create);

            var bf = new BinaryFormatter();
            using (var stream = new MemoryStream()) {
                bf.Serialize(stream, message);
                // Send message to SB
                await QueueClient.SendAsync(new Message(stream.ToArray()));
            }
        }

        /// <summary>
        /// Retrieve a single campaign by id
        /// </summary>
        /// <param name="id">Unique identifier of the campaign record</param>
        public async Task<Campaign> GetCampaign(Guid id)
        {
            using (IDbConnection connection = Connection) {
                IEnumerable<Campaign> result = await connection.QueryAsync<Campaign>(@"
                    SELECT 
                        id, secondary_id, 
                        name, branding_text, 
                        location_include, location_exclude, 
                        language, device, os, 
                        initial_cpc, 
                        budget, budget_daily,  budget_model, 
                        delivery, 
                        bid_strategy, 
                        start_date, end_date, 
                        utm, 
                        status, 
                        create_date, update_date, delete_date, 
                        note, 
                        campaign_group, 
                        connection
	                FROM 
                        campaign
                    WHERE
                        id = @Id", new { Id = id });
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieves all campaigns
        /// </summary>
        /// <returns>A collection of campaigns</returns>
        public async Task<List<Campaign>> GetAllCampaigns()
        {
            // TODO: Limited to 100 for now
            // TODO: Retrieve associated ad data as well? (cpc, clicks, financial)
            using (IDbConnection connection = Connection) {
                connection.Open();
                IEnumerable<Campaign> result = await connection.QueryAsync<Campaign>($"SELECT * FROM campaign LIMIT {100}");
                return result.ToList();
            }
        }

    }
}