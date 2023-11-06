using Itequia.SpeedCode.Persistence.Configuration;
using Itequia.SpeedCode.Persistence.Interfaces.Mongo;
using Itequia.SpeedCode.Persistence.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Persistence.Mongo
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : MongoBaseEntity
    {
        #region Private properties
        public readonly MongoConfiguration _configuration;
        private IMongoCollection<TEntity> collection;
        private readonly string connectionString;
        private readonly string database;
        #endregion

        public MongoRepository(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("MongoConfiguration").Get<MongoConfiguration>();
            connectionString = _configuration.ConnectionString;
            database = _configuration.Database;
            GetDatabase();
        }

        private void GetDatabase()
        {
            var client = new MongoClient(connectionString);
            collection = client.GetDatabase(database, null).GetCollection<TEntity>(typeof(TEntity).Name.ToLower());
        }

        public bool Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<TEntity>> GetAll()
        {
            var result = await collection.FindAsync<TEntity>(_ => true);
            return result.ToList();
        }

        public TEntity GetById(ObjectId _id)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x._id, _id);
            return collection.Find(d => d._id == _id)
                             .FirstOrDefault();
        }

        public async Task<ObjectId> Insert(TEntity entity)
        {
            entity._id = ObjectId.GenerateNewId();
            await collection.InsertOneAsync(entity);
            return entity._id;
        }

        public async Task<ObjectId> Insert(TEntity entity, ObjectId id)
        {
            entity._id = id;
            await collection.InsertOneAsync(entity);
            return entity._id;
        }

        public async Task<IList<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            return await collection.Find(predicate).ToListAsync();
        }

        public async Task<bool> Update(TEntity entity)
        {

            if (entity._id == null)
                await Insert(entity);

            var result = await collection.ReplaceOneAsync(x => x._id == entity._id, entity);
            return result.MatchedCount > 0;
        }
    }
}
