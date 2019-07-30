using System;

namespace Maximiz.Model
{
    /// <summary>
    /// Network account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Account identifier.
        /// </summary>
        public Guid Id { get; set; }

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
