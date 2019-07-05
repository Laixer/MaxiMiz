using System;
using System.Threading.Tasks;
using MaxiMiz.Poller.Poller;

namespace MaxiMiz.Poller
{
    internal static class Program
    {
        static async Task Main()
        {
            using (var poller = new TaboolaPoller())
            {
                var result = await poller.GetTopCampaignReportAsync();
            }

            Console.ReadKey();
        }
    }
}
