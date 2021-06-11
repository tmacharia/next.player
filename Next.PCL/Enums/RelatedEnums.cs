using System.ComponentModel;

namespace Next.PCL.Enums
{
    public enum MetaSource
    {
        [Description("Imdb")]           IMDB = 0X284,
        [Description("The Movie DB")]   TMDB = 0X295,
        [Description("The TV DB")]      TVDB = 0X304,
        [Description("Open Movie DB")]  OMDB = 0X290,
        [Description("TV Maze")]        TVMAZE = 0X471,
        [Description("YTS")]            YTS_MX = 0X421
    }
    public enum OtherSiteDomain
    {
        Unknown     = 0,
        IMDB        = 1,
        TMDB        = 2,
        TVDB        = 3,
        OMDB        = 4,
        TVMAZE      = 5,
        YTS_MX      = 6,
        Twitter     = 7,
        Facebook    = 8,
        Instagram   = 9,
        Reddit      = 10,
        Zap2It      = 11,
        Fansite     = 12,
        OfficialSite = 13,
    }
    public enum MetaType
    {
        Movie   = 1,
        TvShow  = 2,
        Unknown = 0
    }
    public enum Resolution
    {
        /// <summary>
        /// 720 x 480 / WVGA
        /// </summary>
        WVGA    = 480,
        /// <summary>
        /// 1280 x 720 / 720p
        /// </summary>
        HD      = 720,
        /// <summary>
        /// 1920 x 1080 / 1080p
        /// </summary>
        FullHD  = 1080,
        /// <summary>
        /// 3840 x 2160p
        /// </summary>
        UltraHD = 2160
    }
    public enum StreamingPlatform
    {
        Youtube,
        Netflix,
        [Description("Amazon Prime Video")] PrimeVideo,
        [Description("Apple TV")] AppleTV,
        ABC,
        CBS,
        HBO,
        [Description("HBO Max")] HBOMax,
        Hulu
    }
    public enum MetaImageType
    {
        Image       = 0,
        Poster      = 1,
        Backdrop    = 2,
        Thumbnail   = 3,
        Screenshot  = 4,
        Banner      = 5,
        Icon        = 6,
        Logo        = 7,
        Typography  = 8,
        Profile     = 9,
        Still       = 10
    }
    public enum MetaVideoType
    {
        Clip    = 0,
        Trailer = 1,
        Teaser  = 2,
        Extras  = 3
    }
    public enum Profession
    {
        Director = 0,
        Writer   = 1,
        Producer = 2,
        Other    = 3
    }
    public enum Gender
    {
        Male    = 1,
        Female  = 2,
        Unknown = 0,
    }
    public enum CompanyService
    {
        Production = 0,
        Distributor = 1
    }
    public enum MetaStatus
    {
        Released = 0,
        Airing = 1,
        Ended = 2,
        InProduction = 3,
    }
}