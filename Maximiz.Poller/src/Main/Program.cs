using System;
using System.Threading.Tasks;
using MaxiMiz.Poller.Poller;
using MaxiMiz.Poller.Poller.Abstract;

namespace MaxiMiz.Poller.Main
{
    static class Program
    {
        internal static async Task Main()
        {
            ITaboolaPoller taboolaPoller = new TaboolaPoller();

            var result = await taboolaPoller.GetTopCampaignReport();
            Console.WriteLine();
        }
    }
}

