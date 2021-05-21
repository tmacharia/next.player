using System.Collections.Generic;
using Next.PCL.Online.Models;
using TMDbLib.Objects.General;

namespace Next.PCL.Extensions
{
    internal static class TmdbExts
    {
        internal static List<T> GetList<T>(this SearchContainer<T> container)
        {
            if (container != null && container.Results != null)
                return container.Results;
            return null;
        }
        internal static TmdbCast ToTmdbCast(this TMDbLib.Objects.TvShows.Cast cast)
        {
            if (cast != null)
                return null;
            return null;
        }
    }
}