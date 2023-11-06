using System;
using System.Collections.Generic;
using Itequia.SpeedCode.Logger.Enums;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Itequia.SpeedCode.Logger.Models
{
    public class Logs : ILoggable
    {

        public Logs()
        {
            this.CreateAt = DateTimeOffset.Now;
        }
        public DateTimeOffset CreateAt { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public LogLevel Level { get; set; }
        public ApplicationEnum ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string MicroserviceName { get; set; }
        public string Environment { get; set; }        
        public List<LogDetail> Details { get; set; }
        public string ObjectId { get; set; }
    }


    public class LogDetail
    {
     
        public DateTime Date { get; set; }
        public string Step { get; set; }
        public string ProcessPercent { get; set; }
        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public string JsonObject { get; set; }

        public string PrimaryObjectId { get; set; }
    }
}
