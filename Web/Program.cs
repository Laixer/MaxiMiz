using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Maximiz
{

    /// <summary>
    /// Application main entry class to setup host building.
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Application main entry point.
        /// </summary>
        /// <param name="args">The command line arguments</param>
        public static void Main(string[] args)
            => CreateWebHostBuilder(args).Build().Run();

        /// <summary>
        /// Instantiates a host builder.
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <returns>A web host builder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

    }
}
