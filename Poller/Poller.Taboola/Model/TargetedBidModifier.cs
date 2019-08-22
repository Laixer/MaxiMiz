using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Represents a single bid modifier.
    /// </summary>
    [DataContract]
    internal class TargetedBidModifier
    {

        /// <summary>
        /// The name of the target to associated 
        /// this bid boost modifier with. When used
        /// in the context of the <see cref="PublisherBidModifier"/>
        /// the target is the publisher name.
        /// </summary>
        [DataMember(Name = "target")]
        public string Target { get; set; }

        /// <summary>
        /// Modifier for our cost per click value.
        /// This must be between 0.5 and 1.5.
        /// </summary>
        [DataMember(Name = "cpc_modification")]
        public double CpcModification { get; set; }

    }
}
