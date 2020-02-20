using Microsoft.AspNetCore.Mvc;
using Maximiz.Model.Entity;
using Maximiz.ViewModels.CampaignGroupWizard;
using System.Threading.Tasks;
using System;
using Maximiz.ViewModels.Columns;
using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Mapper;
using Maximiz.ViewModels.EntityModels;
using Maximiz.QueryTranslation;
using Maximiz.Operations;
using Microsoft.Extensions.Logging;
using Maximiz.Core.StateMachine.Abstraction;
using System.Threading;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller to manage our <see cref="CampaignGroup"/> creation.
    /// </summary>
    public sealed class CampaignGroupWizardController : Controller
    {

        private readonly IAdGroupWithStatsRepository _adGroupRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper<AdGroupWithStats, AdGroupModel> _mapperAdGroup;
        private readonly IMapper<Account, AccountModel> _mapperAccount;

        private readonly IQueryTranslator _queryTranslator;
        private readonly ILogger logger;

        private readonly IStateMachineManager _stateMachineManager;
        private readonly FormOperationExtractor _formOperationExtractor;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignGroupWizardController(IAdGroupWithStatsRepository adGroupRepository,
            IAccountRepository accountRepository,
            IMapper<AdGroupWithStats, AdGroupModel> mapperAdGroup,
            IMapper<Account, AccountModel> mapperAccount,
            IQueryTranslator queryTranslator,
            ILoggerFactory loggerFactory,
            IStateMachineManager stateMachineManager,
            FormOperationExtractor formOperationExtractor)
        {
            _adGroupRepository = adGroupRepository ?? throw new ArgumentNullException(nameof(adGroupRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _mapperAdGroup = mapperAdGroup ?? throw new ArgumentNullException(nameof(mapperAdGroup));
            _mapperAccount = mapperAccount ?? throw new ArgumentNullException(nameof(mapperAccount));
            _queryTranslator = queryTranslator ?? throw new ArgumentNullException(nameof(queryTranslator));
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(CampaignGroupWizardController));
            _stateMachineManager = stateMachineManager ?? throw new ArgumentNullException(nameof(stateMachineManager));
            _formOperationExtractor = formOperationExtractor ?? throw new ArgumentNullException(nameof(formOperationExtractor));
        }

        /// <summary>
        /// Shows the campaign wizard.
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        [HttpGet]
        public async Task<IActionResult> ShowWizard()
        {
            return View("Wrapper", new WizardCollectionsViewModel
            {
                AccountsTaboolaPublisher = _mapperAccount.ConvertAll(await _accountRepository.GetAllAsync())
            });
        }

        /// <summary>
        /// Submits the creation form, attempts to do the transactions and returns.
        /// </summary>
        /// <param name="model"><see cref="CampaignGroupFormAllViewModel"/></param>
        /// <returns><see cref="OkResult"/> or <see cref="BadRequestResult"/></returns>
        [HttpPost]
        public async Task<IActionResult> PostForm([FromBody] CampaignGroupFormAllViewModel model)
        {
            if (model == null) { throw new ArgumentNullException(nameof(model)); }
            if (!ModelState.IsValid) { return BadRequest("Model state is not valid"); }

            try
            {
                using (var source = new CancellationTokenSource())
                {
                    var operation = await _formOperationExtractor.ExtractAsync(model);
                    await _stateMachineManager.AttemptStartStateMachineAsync(operation, source.Token);
                    return Ok();
                }
            }
            catch (Exception e)
            {
                // TODO Maybe more clear error message display for the user?
                logger.LogError("Couldn't start state machine for campaign group creation wizard", e);
                return BadRequest();
            }
        }

        /// <summary>
        /// Get a partial view containing all ad groups based on some query.
        /// </summary>
        /// <param name="column"><see cref="ColumnCampaignGroupWizardAdGroup"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <param name="searchString">Search string</param>
        /// <param name="page">Page number</param>
        /// <returns><see cref="PartialViewResult"/></returns>
        public async Task<IActionResult> GetAdGroupsAsync(ColumnCampaignGroupWizardAdGroup column,
            Order order, string searchString = null, int page = 1)
        {
            if (page < 1) { throw new ArgumentOutOfRangeException(nameof(page)); }

            var query = _queryTranslator.Translate(column, order, searchString, page);
            return PartialView("_SectionAdGroupsTableRows", new CampaignGroupWizardAdGroupViewModel
            {
                AdGroupList = _mapperAdGroup.ConvertAll(await _adGroupRepository.GetAllAsync(query))
            });
        }

    }
}