using System.Collections.Generic;
using System.Linq;
using Poller.Helper;
using Poller.Taboola.Model;
using AccountCore = Maximiz.Model.Entity.Account;
using AccountTaboola = Poller.Taboola.Model.Account;
using CorePublisher = Maximiz.Model.Enums.Publisher;


namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Our converter for Accounts.
    /// </summary>
    class MapperAccount : IMapper<AccountTaboola, AccountCore>
    {

        private const string DefaultString = "default";
        private const int DefaultNumber = -1;
        private const string DefaultJson = "{}";
        private const string DefaultAccountIdNumber = "xxxxxxxx";
        private const string DefaultAccountIdName = "invalid-account-id";
        private const CorePublisher ThisPublisher = CorePublisher.Taboola;
        private const string DefaultCurrency = "XXX";

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
                Name = core.Name ?? DefaultString,
                AccountId = core.SecondaryId ?? DefaultAccountIdName,
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
                NameHumanReadable = external.Name
            });

            return new AccountCore
            {
                SecondaryId = external.Id ?? DefaultAccountIdNumber,
                Publisher = ThisPublisher,
                Name = external.AccountId ?? DefaultAccountIdName,
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
            IList<AccountCore> result = new List<AccountCore>();
            foreach (var x in list.AsParallel())
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
            IList<AccountTaboola> result = new List<AccountTaboola>();
            foreach (var x in list.AsParallel())
            {
                result.Add(Convert(x));
            }
            return result;
        }

    }
}
