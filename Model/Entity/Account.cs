using Maximiz.Model.Enums;
using System;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Network account.
    /// </summary>
    [Serializable]
    public class Account : Entity<Guid>
    {
        /// <summary>
        /// Network identifier for this object.
        /// This is the string integer we use
        /// with the Taboola API.
        /// </summary>
        public string SecondaryId { get; set; }

        /// <summary>
        /// Publisher network.
        /// </summary>
        public Publisher Publisher { get; set; }
        public string PublisherText { get => Publisher.GetEnumMemberName(); }

        /// <summary>
        /// Account name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Currency used in this account.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// JSON string containing unused data which
        /// we do have to store.
        /// </summary>
        public string Details { get; set; }
    }
}
