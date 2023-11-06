using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Itequia.SpeedCode.Persistence.Models
{
    
    public class MongoBaseEntity
    {

        [BsonId]
        public ObjectId _id
        {
            get
            {

                if (string.IsNullOrEmpty(documentIdentifier))
                    documentIdentifier = ObjectId.GenerateNewId().ToString();

                return new ObjectId(documentIdentifier);

            }
            set
            {
                if (value == null)
                    documentIdentifier = string.Empty;
                else
                    documentIdentifier = value.ToString();
            }
        }

        public string documentIdentifier { get; set; }
    }

}
