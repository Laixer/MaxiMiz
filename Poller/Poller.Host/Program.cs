using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poller.GoogleAds;
using Poller.Taboola;

namespace Poller.Host
{
    internal sealed class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Commandline arguments.</param>
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("appsettings2.json", optional: true);
                    configHost.AddEnvironmentVariables(prefix: "POLLER_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddRemotePublisher<GooglePoller>();
                    services.AddRemotePublisher<TaboolaPoller, TaboolaPollerOptions>();
                    services.AddHostedService<RemoteApplicationService>();
                    services.AddNpgsql("MaxiMizDatabase");
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}
