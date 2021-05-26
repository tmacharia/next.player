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
    }
}