using Maximiz.Core.Utility;
using Maximiz.Model;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Model.Operations;
using Maximiz.ViewModels.CampaignGroupWizard;
using System;
using System.Collections.Generic;
using System.Linq;

using static Maximiz.Mapper.MapperEnum;

namespace Maximiz.Operations
{

    /// <summary>
    /// Extracts <see cref="MyOperation"/> objects from form posts.
    /// </summary>
    internal static class FormOperationExtractor
    {

        /// <summary>
        /// Extracts a <see cref="MyOperation"/> object from a <see cref="CampaignGroupFormAllViewModel"/>
        /// form post.
        /// </summary>
        /// <param name="form"><see cref="CampaignGroupFormAllViewModel"/></param>
        /// <returns><see cref="MyOperation"/></returns>
        internal static MyOperation Extract(CampaignGroupFormAllViewModel form)
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
            
            return operation;
        }

    }
}
