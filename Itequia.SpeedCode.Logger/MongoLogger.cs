using Itequia.SpeedCode.Logger.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Itequia.SpeedCode.Logger
{


    public interface IMongoLogger : ILogger
    {

    }

    public class MongoLogger : IMongoLogger, ILogger
    {
        private readonly string applicationName;
        private readonly string environmentName;
        private readonly IMongoCollection<Logs> logCollection;
        private readonly string name;
        private Func<string, LogLevel, bool> filter;

        public MongoLogger(string name,
                           Func<string, LogLevel, bool> filter,
                           IMongoClient mongoClient,
                           string environmentName,
                           string applicationName, string databaseName)
        {
            Filter = filter ?? ((category, logLevel) => true);
            this.environmentName = environmentName;
            this.applicationName = applicationName;
            this.name = name;
            var db = mongoClient.GetDatabase(databaseName ?? "MongoDbLog");
            logCollection = db.GetCollection<Logs>("logs");
        }

        private Func<string, LogLevel, bool> Filter
        {
            get { return filter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                filter = value;
            }
        }

        public void Log<TState>(LogLevel logLevel,
                                EventId eventId,
                                TState state,
                                Exception exception,
                                Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var title = formatter(state, exception);

            var exceptinon = exception?.ToString();


            Logs log = new Models.Logs();


            if (state.GetType()?.GenericTypeArguments != null &&
                state.GetType()?.BaseType == typeof(ILoggable))
            {
                if (state.GetType() == typeof(Logs))
                {
                    log = BsonSerializer.Deserialize<Logs>(JsonConvert.SerializeObject(state));
                    log.ApplicationName = applicationName;
                    log.Environment = environmentName;
                }
                else
                {
                    log.Details = JsonConvert.DeserializeObject<List<LogDetail>>(JsonConvert.SerializeObject(state));
                }                
            }
            else
            {
                log = new Logs()
                {
                    
                    Message = title,
                    Exception = exceptinon,
                    Level = logLevel,
                    ApplicationName = applicationName,
                    Environment = environmentName
                };
            }

            logCollection.InsertOne(log);
        }

        /// <summary>
        ///     Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return Filter(name, logLevel);
        }

        /// <summary>
        ///     Begins a logical operation scope.
        /// </summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
