﻿using System;
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
        /// <param name="core">The object to convert</param>
        /// <returns>The converted object</returns>
        public AccountTaboola Convert(AccountCore core)
        {
            AccountDetails details = Json.Deserialize
               <AccountDetails>(core.Details);

            return new AccountTaboola
            {
                Name = core.Name ?? DefaultName,
                AccountId = core.SecondaryId ?? DefaultAccountId,
                PartnerTypes = details.PartnerTypes,
                Type = details.Type,
                Currency = core.Currency ?? DefaultCurrency,
                CampaignTypes = details.CampaignTypes
            };
        }

        /// <summary>
        /// Convert taboola account to core model.
        /// </summary>
        /// <param name="external">The object to convert</param>
        /// <returns>The converted object</returns>
        public AccountCore Convert(AccountTaboola external)
        {
            string details = Json.Serialize(new AccountDetails
            {
                Id = external.Id,
                PartnerTypes = external.PartnerTypes?.Select(
                    s => s.ToLowerInvariant()).ToArray(),
                Type = external.Type.ToLower(),
                CampaignTypes = external.CampaignTypes?.Select(
                    s => s.ToLowerInvariant()).ToArray(),
            });

            return new AccountCore
            {
                SecondaryId = external.AccountId ?? DefaultAccountId,
                Publisher = DefaultPublisher,
                Name = external.Name ?? DefaultName,
                Currency = external.Currency ?? DefaultCurrency,
                Details = details
            };
        }

    }
}
