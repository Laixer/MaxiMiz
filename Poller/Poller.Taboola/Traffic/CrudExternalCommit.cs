using Microsoft.Extensions.Logging;
using Poller.Taboola.Model;
using System;
using System.Threading.Tasks;
using Poller.Taboola.Mapper;
using System.Net.Http;
using System.Threading;

using Account = Maximiz.Model.Entity.Account;
using CampaignInternal = Maximiz.Model.Entity.Campaign;
using AdItemInternal = Maximiz.Model.Entity.AdItem;
using EntityGuid = Maximiz.Model.Entity.Entity<System.Guid>;
using System.Diagnostics;

namespace Poller.Taboola.Traffic
{

    /// <summary>
    /// Responsible for all non-get operations in the Taboola API. This is a 
    /// partial class.
    /// </summary>
    internal partial class CrudExternal
    {

        /// <summary>
        /// Logging interface.
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Mapper for campaigns.
        /// </summary>
        private MapperCampaign _mapperCampaign;

        /// <summary>
        /// Mapper for ad items.
        /// </summary>
        private MapperAdItem _mapperAdItem;

        /// <summary>
        /// Transforms Taboola entities to properly formatted http content objects.
        /// </summary>
        private ContentBuilder _contentBuilder;

        /// <summary>
        /// Used to wrap our http operations.
        /// </summary>
        private HttpWrapper _httpWrapper;

        /// <summary>
        /// Communicates with our own database.
        /// </summary>
        private CrudInternal _crudInternal;

        /// <summary>
        /// Constructor for dependency injection.
        /// TODO Abstraction for mappers
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="mapperCampaign">Mapper for campaigns</param>
        /// <param name="mapperAdItem">Mapper for ad items</param>
        /// <param name="httpWrapper">Wrapper for html operations</param>
        /// <param name="crudInternal">Communicates with our own database</param>
        public CrudExternal(ILogger logger, MapperCampaign mapperCampaign,
            MapperAdItem mapperAdItem, HttpWrapper httpWrapper, CrudInternal crudInternal)
        {
            _logger = logger;
            _mapperCampaign = mapperCampaign;
            _mapperAdItem = mapperAdItem;
            _httpWrapper = httpWrapper;
            _crudInternal = crudInternal;

            _contentBuilder = new ContentBuilder();
        }


    }
}
