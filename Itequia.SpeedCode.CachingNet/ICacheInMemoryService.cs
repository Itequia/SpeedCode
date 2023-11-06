using System;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.CachingNet
{
    public interface ICacheInMemoryService
    {
        Task<T> GetOrCreate<T>(object key, Func<Task<T>> createItem) where T : class;
        Task<T> GetOrCreate<T>(object key, DateTimeOffset expirationDate, Func<Task<T>> createItem) where T : class;
        Task<T> Update<T>(object key, Func<Task<T>> updateItem) where T : class;
        Task<bool> Remove(object key);
    }
}
