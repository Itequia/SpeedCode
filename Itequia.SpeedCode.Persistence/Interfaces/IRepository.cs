
using Itequia.SpeedCode.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Persistence.Interfaces
{
    public interface IRepository: IDisposable
    {

        DbContext GetContext();
        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Add<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// Add list of entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void Add<T>(IEnumerable<T> entities) where T : BaseEntity;

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Delete<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// Delete list of entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void Delete<T>(IEnumerable<T> entities) where T : BaseEntity;

        /// <summary>
        /// Get all entities with includes
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<T> GetAll<T>(string[] includes = null) where T : BaseEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<T> GetManyQueryable<T>(System.Linq.Expressions.Expression<Func<T, bool>> condition, params string[] includes) where T : BaseEntity;

        /// <summary>
        /// Get entity by primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<T> GetById<T>(Guid id, params string[] includes) where T : BaseEntity;

        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetById<T>(Guid id) where T : BaseEntity;

        /// <summary>
        /// Get IQueryable by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<T> Get<T>(Guid id) where T : BaseEntity;

        /// <summary>
        /// Get entity list by list of primary keys
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IQueryable<T> GetByIds<T>(List<Guid> ids) where T : BaseEntity;

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Update<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// Update list of entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void Update<T>(IEnumerable<T> entities) where T : BaseEntity;

        /// <summary>
        /// Dispose method.
        /// </summary>
        void Dispose();

        //void ClearChangesTracking();
    }
}
