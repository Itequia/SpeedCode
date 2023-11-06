using Itequia.SpeedCode.Persistence.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Persistence.Interfaces.Mongo
{
    public interface IMongoRepository<TEntity> where TEntity : MongoBaseEntity
    {
        Task<ObjectId> Insert(TEntity entity);
        Task<ObjectId> Insert(TEntity entity, ObjectId id);
        Task<bool> Update(TEntity entity);
        bool Delete(TEntity entity);
        Task<IList<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);
        Task<IList<TEntity>> GetAll();
        TEntity GetById(ObjectId documentId);
    }
}
