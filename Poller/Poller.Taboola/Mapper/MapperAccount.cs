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

        private const string DefaultAccountId = "invalid-account-id";
        private const string DefaultPublisher = "taboola";
        private const string DefaultName = "default-name";
        private const string DefaultCurrency = "invalid";

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
                Id = details.Id,
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

        /// <summary>
        /// Convert a range from Taboola to Core.
        /// </summary>
        /// <param name="list">Taboola list</param>
        /// <returns>Core list</returns>
        public IEnumerable<AccountCore> ConvertAll(
            IEnumerable<AccountTaboola> list)
        {
            List<AccountCore> result = new List<AccountCore>();
            foreach (var x in list)
            {
                result.Add(Convert(x));
            }
            return result;
        }

        /// <summary>
        /// Convert a range from Core to Taboola.
        /// </summary>
        /// <param name="list">Core list</param>
        /// <returns>Taboola list</returns>
        public IEnumerable<AccountTaboola> ConvertAll(
            IEnumerable<AccountCore> list)
        {
            List<AccountTaboola> result = new List<AccountTaboola>();
            foreach (var x in list)
            {
                result.Add(Convert(x));
            }
            return result;
        }

    }
}
