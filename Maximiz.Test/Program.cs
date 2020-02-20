using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Maximiz.Test
{
    class Program
    {

        private static IServiceCollection services;

        static void Main(string[] args)
        {
            services = new ServiceCollection();

            var configuration = new ConfigurationBuilder();

        }


    }
}
