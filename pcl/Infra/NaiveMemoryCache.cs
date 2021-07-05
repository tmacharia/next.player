using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Next.PCL.Infra
{
    public class NaiveMemoryCache : INaiveCache
    {
        private readonly ConcurrentDictionary<string, object> _naiveCache;

        public NaiveMemoryCache()
        {
            _naiveCache = new ConcurrentDictionary<string, object>();
        }

        public bool ContainsKey(string key) => _naiveCache.ContainsKey(key);

        public TItem Get<TItem>(string key)
        {
            if (_naiveCache.TryGetValue(key, out object obj))
                return (TItem)obj;
            return default(TItem);
        }

        
        public async Task<TItem> GetOrAddAsync<TItem>(string key, Func<Task<TItem>> executor)
        {
            if (ContainsKey(key))
                return Get<TItem>(key);
            else
            {
                var result = await executor.Invoke();
                return (TItem)_naiveCache.AddOrUpdate(key, result, (a, b) => result);
            }
        }

        public bool TryAdd(string key, object obj) => _naiveCache.TryAdd(key, obj);
    }
}