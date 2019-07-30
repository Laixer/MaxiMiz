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
        /// </summary>
        public string SecondaryId { get; set; }

        /// <summary>
        /// Publisher network.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Account name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Currency used in this account.
        /// </summary>
        public string Currency { get; set; }
    }
}
