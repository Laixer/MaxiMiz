using Dapper;
using Maximiz.InputModels;
using Maximiz.Model;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Interfaces;
using Maximiz.ServiceBus;
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

        public Task<int> Create(AdGroup entity)
        {
            throw new NotImplementedException();
        }

        public async Task CreateGroup(AdGroupInputModel adGroupInput)
        {
            // TODO Ensure these are the correct columns and properties to insert
            
            // TODO For later: Must link adgroup/ads to a campaign
            var sql_group_insert = @"
                INSERT INTO public.ad_group(
	                name, url)
	                VALUES (@Name, @Url)
                RETURNING id;
            ";

            var sql_ad_item_insert = @"
                INSERT INTO public.ad_item(
	                ad_group, title, url, content)
	                VALUES (@AdGroup, @Title, @Url, @Content);
            ";

            AdGroup adGroup = new AdGroup
            {
                Name = adGroupInput.Name,
                Description = adGroupInput.Description,
                Url = adGroupInput.Url
            };

            // Generate ad items to insert beforehand. 
            var adItemsToCreate = new List<AdItem>();
            foreach (AdItemInputModel adItemInput in adGroupInput.AdItems)
            {
                adItemsToCreate.Add(
                    new AdItem
                    {
                        Title = adItemInput.Title,
                        Url = adGroup.Url, // The URL is the same for all Ad Items.
                        Content = adItemInput.Content
                    }
                );
            }

            using (IDbConnection connection = GetConnection)
            {
                connection.Open();

                IDbTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Create a new AdGroup record.
                    int newAdGroupId = await connection.ExecuteScalarAsync<int>(sql_group_insert, adGroup, transaction: transaction);

                    // Insert the generated campaigns.
                    foreach (AdItem adItem in adItemsToCreate)
                    {
                        // Add the ID from the newly inserted Ad Group.
                        adItem.AdGroup = newAdGroupId;
                    
                        // Insert Ad Item.
                        await connection.ExecuteAsync(sql_ad_item_insert, adItem, transaction: transaction);
                    }

                    adGroup.Id = newAdGroupId;

                    //  Send create messages to the service bus.
                    await ServiceBusQueue.SendObjectMessage(adGroup, CrudAction.Create);
                    await ServiceBusQueue.SendObjectMessages(adItemsToCreate, CrudAction.Create);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
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
