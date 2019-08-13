using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maximiz.Model.Entity;
using Poller.Helper;
using Poller.Taboola.Model;
using AccountCore = Maximiz.Model.Entity.Account;
using AccountTaboola = Poller.Taboola.Model.Account;


namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Our converter for Accounts.
    /// </summary>
    class MapperAccount : IMapper<AccountTaboola, AccountCore>
    {

        private readonly string DefaultAccountId = "invalid-account-id";
        private readonly string DefaultPublisher = "taboola";
        private readonly string DefaultName = "default-name";
        private readonly string DefaultCurrency = "invalid";

        /// <summary>
        /// Convert core model to taboola account.
        /// </summary>
        /// <param name="from">The object to convert</param>
        /// <returns>The converted object</returns>
        public AccountTaboola Convert(AccountCore from)
        {
            AccountDetails details = Json.Deserialize
               <AccountDetails>(from.Details);

            return new AccountTaboola
            {
                Name = from.Name ?? DefaultName,
                AccountId = from.SecondaryId ?? DefaultAccountId,
                PartnerTypes = details.PartnerTypes,
                Type = details.Type,
                Currency = from.Currency ?? DefaultCurrency,
                CampaignTypes = details.CampaignTypes
            };
        }

        /// <summary>
        /// Convert taboola account to core model.
        /// </summary>
        /// <param name="from">The object to convert</param>
        /// <returns>The converted object</returns>
        public AccountCore Convert(AccountTaboola from)
        {
            string details = Json.Serialize(new AccountDetails
            {
                Id = from.Id,
                PartnerTypes = from.PartnerTypes?.Select(
                    s => s.ToLowerInvariant()).ToArray(),
                Type = from.Type.ToLower(),
                CampaignTypes = from.CampaignTypes?.Select(
                    s => s.ToLowerInvariant()).ToArray(),
            });

            return new AccountCore
            {
                SecondaryId = from.AccountId ?? DefaultAccountId,
                Publisher = DefaultPublisher,
                Name = from.Name ?? DefaultName,
                Currency = from.Currency ?? DefaultCurrency,
                Details = details
            };
        }

    }
}
