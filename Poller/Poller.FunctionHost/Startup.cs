using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Poller.Taboola;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

// Register namepace to also build configuration
[assembly: FunctionsStartup(typeof(Poller.FunctionHost.Startup))]

namespace Poller.FunctionHost
{

    /// <summary>
    /// Configures our dependency injection.
    /// </summary>
    public class Startup : FunctionsStartup
    {

        /// <summary>
        /// Sets up our dependency injection.
        /// </summary>
        /// <param name="builder">The host builder for Azure Functions</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddTaboolaPollerOptions();
            builder.Services.AddSingleton<TaboolaPoller>();
            //builder.Services.AddPoller<TaboolaPoller, TaboolaPollerOptions>();
            builder.Services.AddDbProvider("MaximizDatabase");
            builder.Services.AddEventBusProvider("MaximizServiceBusListen");
            // builder.Services.AddMemoryCache();
        }

    }
}
