using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Poller.Model
{
    [DataContract]
    public class AllowedAccounts
    {
        /// <summary>
        /// The items in this result.
        /// </summary>
        [DataMember(Name = "results")]
        public IEnumerable<Account> Items { get; set; }
    }
}
