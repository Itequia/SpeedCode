using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Caching
{
    /// <summary>
    /// Cache in memory service implementation
    /// </summary>
    public class CacheInMemoryService : Interfaces.ICacheInMemoryService
    {

        private IMemoryCache memoryCache;
        private ConcurrentDictionary<object, SemaphoreSlim> locks = new ConcurrentDictionary<object, SemaphoreSlim>();
        private List<string> keyList = new List<string>();

        public CacheInMemoryService(IMemoryCache _memoryCache)
        {
            this.memoryCache = _memoryCache;
        }

        public async Task<T> GetOrCreate<T>(object key, Func<Task<T>> createItem) where T : class
        {
            T cacheEntry;
            if (!this.memoryCache.TryGetValue(key, out cacheEntry))
            {
                SemaphoreSlim cacheLock = locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
                await cacheLock.WaitAsync();

                try
                {
                    if(!memoryCache.TryGetValue(key, out cacheEntry))
                    {
                        cacheEntry = await createItem();
                        memoryCache.Set(key, cacheEntry);
                        AddCacheKey(key.ToString());
                    }
                }
                finally
                {
                    cacheLock.Release();
                }
            }
            return cacheEntry;
        }

        public async Task<T> GetOrCreate<T>(object key, DateTimeOffset expirationDate, Func<Task<T>> createItem) where T : class
        {
            T cacheEntry;
            if (!this.memoryCache.TryGetValue(key, out cacheEntry))
            {
                SemaphoreSlim cacheLock = locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
                await cacheLock.WaitAsync();

                try
                {
                    if (!memoryCache.TryGetValue(key, out cacheEntry))
                    {
                        cacheEntry = await createItem();
                        memoryCache.Set(key, cacheEntry, expirationDate);
                        AddCacheKey(key.ToString());
                    }
                }
                finally
                {
                    cacheLock.Release();
                }
            }
            return cacheEntry;
        }

        public async Task<T> Update<T>(object key, Func<Task<T>> updateItem) where T : class
        {
            T cacheEntry;
            if (this.memoryCache.TryGetValue(key, out cacheEntry))
            {
                SemaphoreSlim cacheLock = locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
                await cacheLock.WaitAsync();

                try
                {
                    if (memoryCache.TryGetValue(key, out cacheEntry))
                    {
                        memoryCache.Remove(key);
                        cacheEntry = await updateItem();
                        memoryCache.Set(key, cacheEntry);
                    }
                }
                finally
                {
                    cacheLock.Release();
                }
            }
            return cacheEntry;
        }

        public Task<bool> Remove(object key)
        {
            bool value = true;
            try
            {
                this.memoryCache.Remove(key);
                RemoveCacheKey(key.ToString());
            }
            catch(Exception ex)
            {
                value = false;
            }            
            return Task.FromResult(value);
        }

        public Task<bool> RemoveAll()
        {                       
            foreach(var key in keyList.ToArray())
            {
                Remove(key);
            }                 
            return Task.FromResult(true);
        }

        private void RemoveCacheKey(string key)
        {
            if (keyList.Contains(key))
                keyList.Remove(key.ToString());
        }

        private void AddCacheKey(string key)
        {
            if (!keyList.Contains(key))
                keyList.Add(key);
        }
    }
}
