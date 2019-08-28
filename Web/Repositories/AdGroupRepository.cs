using Dapper;
using Maximiz.InputModels;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Repositories.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Repositories
{
    public class AdGroupRepository : IAdGroupRepository
    {
        private readonly IConfiguration _configuration;

        public AdGroupRepository(IConfiguration configuration)
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

        public Task<int> Create(AdGroup entity)
        {
            throw new NotImplementedException();
        }

        public async Task CreateGroup(AdGroupInputModel adGroupInput)
        {
            var sql_group_insert = @"
                
            ";

            var sql_group_params =
                new
            {

            };

            using (IDbConnection connection = GetConnection)
            {
                connection.Open();

                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    // Create a new AdGroup record.
                    int newAdGroupId = await connection.ExecuteScalarAsync<int>(sql_group_insert, sql_group_params, transaction: transaction);

                    // Insert the generated campaigns.
                    foreach (AdItemInputModel adItemInput in adGroupInput.AdItems)
                    {
                        // Insert
                        //await connection.ExecuteAsync(sql_campaign_insert, @params, transaction: transaction);
                    }
                    transaction.Commit();
                }
            }
        }

        public Task Delete(AdGroup entity)
        {
            throw new NotImplementedException();
        }

        public Task<AdGroup> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AdGroup> Update(AdGroup entity)
        {
            throw new NotImplementedException();
        }
    }
}
