using System;
using Common;
using Next.PCL.Enums;

namespace Next.PCL
{
    public static class SiteUrls
    {
        public const string IMDB = "https://imdb.com";
        public const string OMDB = "http://www.omdbapi.com";
        public const string TVDB = "https://thetvdb.com";
        public const string TMDB = "https://www.themoviedb.org";
        public const string TVMAZE = "http://api.tvmaze.com";
        public const string YTS = "https://yts.mx/api/v2";

        public static OtherSiteDomain ParseToSiteDomain(this Uri uri, string hint = default)
        {
            return ParseToSiteDomain(uri?.OriginalString, hint);
        }
        public static OtherSiteDomain ParseToSiteDomain(string url, string hint = default)
        {
            if (url.IsValid())
            {
                if (url.Contains("imdb.com"))
                    return OtherSiteDomain.IMDB;

                if (url.Contains("omdbapi.com"))
                    return OtherSiteDomain.OMDB;

                if (url.Contains("thetvdb.com"))
                    return OtherSiteDomain.TVDB;

                if (url.Contains("tvmaze.com"))
                    return OtherSiteDomain.TVMAZE;

                if (url.Contains("themoviedb.org"))
                    return OtherSiteDomain.TMDB;

                if (url.Contains("yts.mx"))
                    return OtherSiteDomain.YTS_MX;

                if (url.Contains("twitter.com"))
                    return OtherSiteDomain.Twitter;

                if (url.Contains("facebook.com"))
                    return OtherSiteDomain.Facebook;

                if (url.Contains("instagram.com"))
                    return OtherSiteDomain.Instagram;

                if (url.Contains("reddit.com"))
                    return OtherSiteDomain.Reddit;

                if (url.Contains("zap2it.com"))
                    return OtherSiteDomain.Zap2It;
                if (url.Contains("reddit.com"))
                    return OtherSiteDomain.Reddit;

                if (hint.IsValid())
                {
                    if (hint.Matches("official"))
                        return OtherSiteDomain.OfficialSite;
                    if (hint.Matches("fan site"))
                        return OtherSiteDomain.Fansite;
                }
            }
            return OtherSiteDomain.Unknown;
        }
    }
}