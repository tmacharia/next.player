using System.Collections.Concurrent;

namespace Next.PCL.Infra
{
    public interface INextMemoryCache
    {
        bool ContainsKey(string key);
        bool TryGet<TItem>(string key, out TItem item);
        bool TryGet(string key, out object obj);
        bool TryAdd(string key, object obj);
    }
    public class NextMemoryCache : INextMemoryCache
    {
        private readonly ConcurrentDictionary<string, object> _naiveCache;

        public NextMemoryCache()
        {
            _naiveCache = new ConcurrentDictionary<string, object>();
        }

        public bool ContainsKey(string key) => _naiveCache.ContainsKey(key);
        public bool TryGet(string key, out object obj) => _naiveCache.TryGetValue(key, out obj);
        public bool TryGet<TItem>(string key, out TItem item)
        {
            if(TryGet(key, out object obj))
            {
                item = (TItem)obj;
                return true;
            }
            item = default(TItem);
            return false;
        }
        public bool TryAdd(string key, object obj) => _naiveCache.TryAdd(key, obj);
    }
}