using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Used to indicate the objective of a given campaign.
    /// This field is required in the Taboola Campaign API.
    /// </summary>
    public enum MarketingObjective
    {

        [DataMember(Name="brand_awareness")]
        BrandAwareness,

        [DataMember(Name = "leads_generation")]
        LeadsGeneration,

        [DataMember(Name = "online_purchases")]
        OnlinePurchases,

        [DataMember(Name = "drive_website_traffic")]
        DriveWebsiteTraffic,

        [DataMember(Name = "mobile_app_install")]
        MobileAppInstall

    }
}
