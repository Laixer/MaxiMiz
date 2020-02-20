using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Core.Utility;
using Maximiz.Model;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Model.Operations;
using Maximiz.ViewModels.CampaignDetails;
using Maximiz.ViewModels.CampaignGroupWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Maximiz.Mapper.MapperEnum;

namespace Maximiz.Operations
{

    /// <summary>
    /// Extracts <see cref="MyOperation"/> objects from form posts.
    /// </summary>
    public sealed class FormOperationExtractor
    {

        private readonly ICampaignRepository _campaignRepository;
        private readonly ICampaignGroupRepository _campaignGroupRepository;
        private readonly IAdGroupRepository _adGroupRepository;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public FormOperationExtractor(ICampaignRepository campaignRepository,
            ICampaignGroupRepository campaignGroupRepository,
            IAdGroupRepository adGroupRepository)
        {
            _campaignRepository = campaignRepository ?? throw new ArgumentNullException(nameof(campaignRepository));
            _campaignGroupRepository = campaignGroupRepository ?? throw new ArgumentNullException(nameof(campaignGroupRepository));
            _adGroupRepository = adGroupRepository ?? throw new ArgumentNullException(nameof(adGroupRepository));
        }

        /// <summary>
        /// Extracts a <see cref="MyOperation"/> object from a <see cref="CampaignGroupFormAllViewModel"/>
        /// form post.
        /// </summary>
        /// <param name="form"><see cref="CampaignGroupFormAllViewModel"/></param>
        /// <returns><see cref="MyOperation"/></returns>
        public Task<MyOperation> ExtractAsync(CampaignGroupFormAllViewModel form)
        {
            if (form == null) { throw new ArgumentNullException(nameof(form)); }
            // TODO Form checks

            // Generate operation
            var operation = new MyOperation
            {
                CrudAction = CrudAction.Create,
                StartDate = DateTime.Now,
                Id = Guid.NewGuid(), // TODO Should we?
            };

            // Get ad group links
            operation.AdGroupCampaignGroupLinksAdd = new List<AdGroupCampaignGroupLinkingEntry>();
            foreach (var adGroupId in form.SelectedAdGroupIds)
            {
                operation.AdGroupCampaignGroupLinksAdd.Add(new AdGroupCampaignGroupLinkingEntry { AdGroupId = adGroupId });
            }

            // Generate campaign group
            operation.TopEntity = new CampaignGroup
            {
                AccountId = form.AccountTaboolaId,
                BidStrategy = Map(form.BidStrategy),
                BrandingText = form.BrandingText,
                BudgetModel = Map(form.BudgetModel),
                Budget = form.Budget,
                BudgetDaily = (form.BudgetDailyInfinite || form.BudgetDaily == 0) ? null : form.BudgetDaily as decimal?,
                Delivery = Map(form.Delivery),
                Devices = form.Devices.Select(x => Map(x)).ToArray(),
                EndDate = form.IgnoreEndDate ? null : form.EndDate as DateTime?,
                InitialCpc = form.Cpc,
                LocationInclude = form.Locations.Select(x => MapperLocationIntegers.Map(Map(x))).ToArray(),
                Name = form.CampaignNameSuffix,
                OperatingSystems = form.OperatingSystems.Select(x => Map(x)).ToArray(),
                OperationId = operation.Id,
                OperationItemStatus = OperationItemStatus.PendingCreate,
                Publisher = form.Publishers.Select(x => Map(x)).FirstOrDefault(), // TODO This shouldnt go down like this mb
                StartDate = form.StartDate,
                TargetUrl = form.Url,
                Utm = form.UtmCustom
            };
            
            return Task.FromResult(operation);
        }

        /// <summary>
        /// Extracts a <see cref="MyOperation"/> object from a <see cref="FormCampaignDetailsViewModel"/>.
        /// </summary>
        /// <param name="form"><see cref="FormCampaignDetailsViewModel"/></param>
        /// <returns><see cref="MyOperation"/></returns>
        public async Task<MyOperation> ExtractAsync (FormCampaignDetailsViewModel form)
        {
            if (form == null) { throw new ArgumentNullException(nameof(form)); }
            if (form.CampaignId == null || form.CampaignId == Guid.Empty) { throw new ArgumentNullException("Campaign id can't be null or empty"); }
            if (!form.IgnoreEndDate && form.EndDate > DateTime.Now) { throw new InvalidOperationException("End date must be in the future"); }
            if (form.Cpc > form.Budget) { throw new InvalidOperationException("Cpc must be <= budget"); }
            // TODO Extra checks

            // Create operation itself
            var operation = new MyOperation
            {
                StartDate = DateTime.Now,
                CrudAction = CrudAction.Update,
                Id = Guid.NewGuid() // TODO Should we?                
            };

            // Get and modify campaign based on form
            var campaign = await _campaignRepository.GetAsync(form.CampaignId);
            campaign.BidStrategy = Map(form.BidStrategy);
            campaign.BrandingText = form.BrandingText;
            campaign.Budget = form.Budget;
            campaign.BudgetDaily = (form.BudgetDailyInfinite || form.BudgetDaily == 0) ? null : form.BudgetDaily as decimal?;
            campaign.BudgetModel = Map(form.BudgetModel);
            campaign.Delivery = Map(form.Delivery);
            campaign.Devices = form.Devices.Select(x => Map(x)).ToArray();
            campaign.EndDate = form.IgnoreEndDate ? null : form.EndDate as DateTime?;
            campaign.InitialCpc = form.Cpc;
            campaign.LocationInclude = form.Locations.Select(x => MapperLocationIntegers.Map(Map(x))).ToArray();
            campaign.Name = form.CampaignNameSuffix;
            campaign.OperatingSystems = form.OperatingSystems.Select(x => Map(x)).ToArray();
            campaign.OperationId = operation.Id;
            campaign.OperationItemStatus = OperationItemStatus.PendingUpdate;
            //campaign.StartDate = form.StartDate; // TODO How to handle this?
            campaign.TargetUrl = form.Url;
            campaign.Utm = form.UtmCustom; // TODO How to handle this?
            operation.TopEntity = campaign;

            // Get currently linked ad group ids
            var currentIds = (await _adGroupRepository.GetLinkedWithCampaignAsync(campaign.Id)).Select(x => x.Id);
            
            // Get adgroup links
            operation.AdGroupCampaignLinksAdd = new List<AdGroupCampaignLinkingEntry>();
            foreach (var idToLink in form.LinkedAdGroupIds.Except(currentIds))
            {
                operation.AdGroupCampaignLinksAdd.Add(new AdGroupCampaignLinkingEntry { AdGroupId = idToLink });
            }

            // Get adgroup unlinks
            operation.AdGroupCampaignLinksRemove = new List<AdGroupCampaignLinkingEntry>();
            foreach (var idToUnlink in currentIds.Except(form.LinkedAdGroupIds))
            {
                operation.AdGroupCampaignLinksRemove.Add(new AdGroupCampaignLinkingEntry { AdGroupId = idToUnlink });
            }

            return operation;
        }

    }
}
