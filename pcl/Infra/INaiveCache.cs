using System;
using System.Threading.Tasks;

namespace Next.PCL.Infra
{
    public interface INaiveCache
    {
        bool ContainsKey(string key);

        TItem Get<TItem>(string key);

        Task<TItem> GetOrAddAsync<TItem>(string key, Func<Task<TItem>> executor);

        bool TryAdd(string key, object obj);
    }
}