using Common;
using Next.PCL.Entities;

namespace Next.PCL.Extensions
{
    internal static class LocaleExts
    {
        internal static GeographicLocation ToGeoLocale(this string name, bool isCountry = false)
        {
            if (name.IsValid())
            {
                return new GeographicLocation(name)
                {
                    IsCountry = isCountry
                };
            }
            return null;
        }
    }
}