using Itequia.SpeedCode.Logger.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Itequia.SpeedCode.Logger
{
    public class Log<T> where T : class
    {

        public Log()
        {
            this.CreateAt = DateTimeOffset.Now;

        }

        [BsonId]
        public ObjectId Id { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public LogLevel Level { get; set; }
        public string ApplicationName { get; set; }
        public string Environment { get; set; }
        public List<T> Details { get; set; }

    }
}
