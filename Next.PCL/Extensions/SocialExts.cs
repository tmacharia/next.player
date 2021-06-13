using System;
using Common;

namespace Next.PCL.Extensions
{
    internal static class SocialExts
    {
        internal static Uri GetYoubetubeUrl(string key)
        {
            if (key.IsValid())
            {
                return new Uri(string.Format("watch?v={0}", key));
            }
            return null;
        }
        internal static string GetYoubetubeKey(Uri uri)
        {
            if(uri != null)
            {
                return uri.GetQueryStrings()["v"];
            }
            return string.Empty;
        }
    }
}