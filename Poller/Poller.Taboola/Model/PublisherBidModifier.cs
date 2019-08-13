using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// An object representing the targeted
    /// publisher's bid modifiers.
    /// </summary>
    [DataContract]
    internal class PublisherBidModifier
    {

        /// <summary>
        /// Represents all targeted publisher's bid
        /// modifiers.
        /// 
        /// Null represents no change to targeted
        /// bid modifiers.
        /// 
        /// Empty array represents no targeted bid
        /// modifiers.
        /// </summary>
        [DataMember(Name = "values")]
        public TargetedBidModifier[] Values {get; set;}

    }
}
