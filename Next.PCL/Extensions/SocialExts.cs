using System;
using System.Linq;
using Common;
using Next.PCL.Entities;
using Next.PCL.Enums;

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
                return uri.OriginalString.SplitByAndTrim("?v=").Last();
            }
            return string.Empty;
        }
        internal static MetaUrl ParseToMetaUrl(this Uri uri, MetaSource metaSource)
        {
            if (uri != null)
            {
                return new MetaUrl(metaSource)
                {
                    Url = uri,
                    Domain = uri.ParseToSiteDomain(uri.OriginalString)
                };
            }
            return null;
        }
    }
}