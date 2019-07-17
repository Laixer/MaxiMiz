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
                    configHost.AddJsonFile("appsettings.json", optional: true);
                    configHost.AddEnvironmentVariables(prefix: "POLLER_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    hostContext.HostingEnvironment.ApplicationName = "Poller.Host";

                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddRemotePublisher<GoogleAdsPoller, GoogleAdsPollerOptions>();
                    services.AddRemotePublisher<TaboolaPoller, TaboolaPollerOptions>();
                    services.AddHostedService<RemoteApplicationService>();
                    services.AddNpgsql("MaxiMizDatabase");
                    services.AddMemoryCache();

                    services.Configure<RemoteApplicationServiceOptions>(options =>
                    {
                        options.PublisherRefreshInterval = 15;
                    });
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
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
