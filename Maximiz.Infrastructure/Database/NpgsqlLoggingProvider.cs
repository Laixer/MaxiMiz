using Microsoft.Extensions.Logging;
using Npgsql.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Maximiz.Infrastructure.Database
{
    public sealed class NpgsqlLoggingProvider : INpgsqlLoggingProvider
    {

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger logger;

        public NpgsqlLoggingProvider(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        NpgsqlLogger INpgsqlLoggingProvider.CreateLogger(string name)
        {
            return new MyNpgsqlLogger(name, _loggerFactory);
        }
    }

    public sealed class MyNpgsqlLogger : NpgsqlLogger
    {

        private readonly ILogger logger;

        public MyNpgsqlLogger(string name, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(name);
        }

        public override bool IsEnabled(NpgsqlLogLevel level) 
            => true;

        public override void Log(NpgsqlLogLevel level, int connectorId, string msg, Exception exception = null) 
            => logger.Log(LogLevel.Trace, msg);
    }
}
