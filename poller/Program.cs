
using System;
using System.Configuration;

namespace poller
{
    static class Program
    {
        internal static void Main()
        {
            // GooglePoller gp = new GooglePoller();
            // TaboolaPoller p = new TaboolaPoller();
            ConfigurationManager.AppSettings.Get("TaboolaApi");
            // var y = ConfigurationManager.AppSettings.Get("GoogleAdsApi");
            try
            {
                var z = ConfigurationManager.GetSection("TaboolaApi");
            }
            catch (ConfigurationErrorsException cee)
            {
                throw cee;
            }
            // await p.getOAuth2Response();
            Console.Write("");
            // await p.GetTopCampaignReport();
        }

    }
}

