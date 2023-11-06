using Itequia.SpeedCode.Logger.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Itequia.SpeedCode.Logger
{
    public class MongoLoggerProvider : ILoggerProvider
    {
        private readonly string applicationName;
        private readonly LogConfiguration configuration;
        private readonly string environmentName;
        private readonly Func<string, LogLevel, bool> filter;

        public MongoLoggerProvider(Func<string, LogLevel, bool> filter,
                                   LogConfiguration configuration,
                                   string applicationName,
                                   string environmentName)
        {
            this.filter = filter;
            this.configuration = configuration;
            this.applicationName = applicationName;
            this.environmentName = environmentName;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MongoLogger(categoryName, filter, configuration.Client, environmentName, applicationName, configuration.DatabaseName);
        }
        public void Dispose()
        {
        }
    }
}
