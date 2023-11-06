using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Itequia.SpeedCode.Logger.Configuration
{
    public class LogConfiguration
    {
        public string DatabaseName { get; set; }
        public IMongoClient Client { get; set; }
        public LogLevel MinLevel { get; set; }
    }
}
