using Itequia.SpeedCode.Logger.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Itequia.SpeedCode.Logger.Extensions
{
    public static class MongoLoggerExtension
    {

        public static void Log(this ILogger logger, Logs logData)
        {

            logger.Log(logData.Level,
                       new EventId(1),
                       logData,
                       null,
                       (m, n) => { return ""; });
        }

        public static void Log(this ILogger logger, LogLevel level, List<LogDetail> details)
        {

            logger.Log(level,
                       new EventId(1),
                       details,
                       null,
                       (m, n) => { return ""; });
        }
    }
}
