using Itequia.SpeedCode.Entities.Base;
using Itequia.SpeedCode.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Persistence
{
    public class Repository : IRepository
    {
        protected readonly DbContext _db;
        private bool _disposed;

        public Repository(DbContext db)
        {            
            _db = db;
            //db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task AddAsync<T>(T entity) where T:BaseEntity
        {
            await _db.Set<T>().AddAsync(entity);
        }
        public async Task AddAsync<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            await _db.Set<T>().AddRangeAsync(entities);
        }
        public void Add<T>(T entity) where T: BaseEntity
        {
            _db.Set<T>().Add(entity);
        }      
        public void Add<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            _db.Set<T>().AddRange(entities);
        }

        public void Delete<T>(T entity) where T : BaseEntity
        {
            _db.Set<T>().Remove(entity);
        }

        public void Delete<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            _db.Set<T>().RemoveRange(entities);
        }

        public IQueryable<T> GetAll<T>(string[] includes = null) where T : BaseEntity
        {
            IQueryable<T> result;
            if (includes != null && includes.Length > 0)
            {
                result = includes
                    .Aggregate(_db.Set<T>().AsQueryable(),
                        (current, include) => current.Include(include)
                    );
            }
            else
            {
                result = _db.Set<T>().AsQueryable();
            }
            return result;
        }

        public IQueryable<T> GetManyQueryable<T>(System.Linq.Expressions.Expression<Func<T, bool>> condition, params string[] includes) where T : BaseEntity
        {
            IQueryable<T> query = _db.Set<T>();

            query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.Where(condition).AsQueryable();
        }

        public async Task<T> GetById<T>(Guid id, params string[] includes) where T : BaseEntity
        {
            IQueryable<T> query = _db.Set<T>();

            query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetById<T>(Guid id) where T : BaseEntity
        {
            return await _db.Set<T>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<T> Get<T>(Guid id) where T : BaseEntity
        {
            return _db.Set<T>().Where(x => x.Id == id);
        }

        public IQueryable<T> GetByIds<T>(List<Guid> ids) where T : BaseEntity
        {
            return _db.Set<T>().Where(x => ids.Contains(x.Id));
        }

        public void Update<T>(T entity) where T : BaseEntity
        {
            _db.Entry(entity).State = EntityState.Modified;

        }

        public void Update<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            foreach (var entity in entities)
                _db.Entry(entity).State = EntityState.Modified;
        }

        public DbContext GetContext()
        {
            return this._db;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            _disposed = true;
        }
        
        //public void ClearChangesTracking()
        //{
        //    _db.ChangeTracker.Clear();
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
