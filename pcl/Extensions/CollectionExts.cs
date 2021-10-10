using System.Collections.Generic;
using System.Linq;

namespace Next.PCL.Extensions
{
    public static class CollectionExts
    {
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> ts)
        {
            if (ts != null && ts.Any())
                return true;
            return false;
        }
        public static void AddToThis<T>(this List<T> ts, T item)
        {
            if (item != null)
                ts.Add(item);
        }
        public static void AddToThis<T>(this List<T> ts, IEnumerable<T> items)
        {
            if (items.IsNotNullOrEmpty())
            {
                foreach (var item in items)
                {
                    ts.AddToThis(item);
                }
            }
        }
    }
}