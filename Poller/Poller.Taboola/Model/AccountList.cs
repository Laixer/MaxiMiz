using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Poller.Taboola.Model
{
    [DataContract]
    public class AccountList
    {
        /// <summary>
        /// The items in this result.
        /// </summary>
        [DataMember(Name = "results")]
        public IEnumerable<Account> Items { get; set; }
    }
}
