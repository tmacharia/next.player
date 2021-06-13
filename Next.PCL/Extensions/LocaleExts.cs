using Common;
using Next.PCL.Entities;

namespace Next.PCL.Extensions
{
    internal static class LocaleExts
    {
        internal static GeographicLocation ToGeoLocale(this string name)
        {
            if (name.IsValid())
            {
                return new GeographicLocation(name);
            }
            return null;
        }
    }
}