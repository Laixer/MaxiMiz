using Dapper;
using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Infrastructure.EventQueue;
using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Core.Utility;
using Maximiz.Infrastructure.Database;
using Maximiz.Model;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Model.Operations;
using Maximiz.Model.Protocol;
using Maximiz.Storage.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller used for debug purposes.
    /// </summary>
    public class DebugController : Controller
    {

        private readonly IDatabaseProvider _databaseProvider;
        private readonly IStorageManager _storageManager;
        private readonly ICampaignRepository _campaignRepository;
        private readonly ICampaignWithStatsRepository _campaignWithStatsRepository;
        private readonly IOperationItemCommitter _operationItemCommitter;
        private readonly IStateMachineManager _stateMachineManager;
        private readonly IEventQueueSender _eventQueueSender;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public DebugController(IStorageManager storageManager,
            IDatabaseProvider databaseProvider,
            ICampaignRepository campaignRepository,
            ICampaignWithStatsRepository campaignWithStatsRepository,
            IOperationItemCommitter operationItemCommitter,
            IStateMachineManager stateMachineManager,
            IEventQueueSender eventQueueSender)
        {
            _storageManager = storageManager ?? throw new ArgumentNullException(nameof(storageManager));
            _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
            _campaignRepository = campaignRepository ?? throw new ArgumentNullException(nameof(campaignRepository));
            _campaignWithStatsRepository = campaignWithStatsRepository ?? throw new ArgumentNullException(nameof(campaignWithStatsRepository));
            _operationItemCommitter = operationItemCommitter ?? throw new ArgumentNullException(nameof(operationItemCommitter));
            _stateMachineManager = stateMachineManager ?? throw new ArgumentNullException(nameof(stateMachineManager));
            _eventQueueSender = eventQueueSender ?? throw new ArgumentNullException(nameof(eventQueueSender));
        }

        /// <summary>
        /// Index function.
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> TestStateMachine()
        {
            using (var source = new CancellationTokenSource())
            {
                var entity = new CampaignGroup { Id = new Guid("23369f01-850c-4fbe-93f2-3e6431377957") };
                var message = new OperationMessage(entity, CrudAction.Create, new Guid("74708cb8-178d-4ed4-85f8-ceb22d8e2e4e"));
                await _eventQueueSender.SendMessageAsync(message, source.Token);
            }

            return View("Index");
        }

        private MyOperation CreateOperation()
        {
            var campaignGroup = new CampaignGroup
            {
                AccountId = new Guid("dec58735-af87-466b-b983-a7116f0406e9"),
                BidStrategy = BidStrategy.Fixed,
                BrandingText = "My debug branding text",
                Budget = 1000,
                BudgetDaily = 300,
                BudgetModel = BudgetModel.Campaign,
                Delivery = Delivery.Strict,
                Devices = new[] { Device.Desktop, Device.Laptop },
                EndDate = null,
                LocationExclude = new int[0],
                LocationInclude = MapperLocationIntegers.MapMultiple(new[] { Location.NL, Location.DE, Location.FR }),
                Language = "NL",
                InitialCpc = 0.54M,
                Name = "My testing campaign group",
                OperatingSystems = new[] { OS.Android, OS.Windows },
                Publisher = Publisher.Taboola,
                StartDate = new DateTime(2020, 2, 1),
                TargetUrl = "http://www.laixer.com",

                OperationItemStatus = OperationItemStatus.PendingCreate,
            };

            var linke1 = new AdGroupCampaignGroupLinkingEntry
            {
                LinkedId = campaignGroup.Id,
                AdGroupId = new Guid("8c20c3de-932c-4435-9460-dd34350b9a9e")
            };
            var linke2 = new AdGroupCampaignGroupLinkingEntry
            {
                LinkedId = campaignGroup.Id,
                AdGroupId = new Guid("cd9b046a-137e-4739-81e6-3077d099bbab")
            };
            return new MyOperation
            {
                CreateDate = DateTime.Now,
                TopEntity = campaignGroup,
                CrudAction = CrudAction.Create,
                Id = Guid.NewGuid(),
                AdGroupCampaignGroupLinksAdd = new List<AdGroupCampaignGroupLinkingEntry> { linke1, linke2 }
            };
        }

        private async Task CleanUpAsync()
        {
            using (var connection = _databaseProvider.GetConnectionScope())
            {
                var sql = @"
                    UPDATE public.campaign SET operation_item_status = 'up_to_date' WHERE operation_item_status IS NOT NULL;
                    UPDATE public.campaign_group SET operation_item_status = 'up_to_date' WHERE operation_item_status IS NOT NULL;
                    UPDATE public.ad_group SET operation_item_status = 'up_to_date' WHERE operation_item_status IS NOT NULL;
                    UPDATE public.ad_item SET operation_item_status = 'up_to_date' WHERE operation_item_status IS NOT NULL;";
                await connection.ExecuteAsync(sql);
            }
        }

    }
}