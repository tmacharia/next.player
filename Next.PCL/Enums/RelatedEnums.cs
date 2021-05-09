using System.ComponentModel;

namespace Next.PCL.Entities
{
    public enum MetaSource
    {
        [Description("Imdb")]           IMDB = 0,
        [Description("The Movie DB")]   TMDB = 1,
        [Description("The TV DB")]      TVDB = 2,
        [Description("Open Movie DB")]  OMDB = 3,
        [Description("TV Maze")]        TVMAZE = 4,
        [Description("YTS")]            YTS_MX = 5
    }
    public enum Resolution
    {
        [Description("Standard Definition")]    SD = 480,
        [Description("High Definition")]        HD = 720,
        [Description("Ultra High Definition")]  UHD = 1080,
        [Description("Quad High Definition")]   QuadHD,
        [Description("4K")]                     _4K
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
        Logo        = 7
    }
    public enum MetaVideoType
    {
        Trailer = 0,
        Teaser  = 1,
        Clip    = 2
    }
    public enum Profession
    {
        Director    = 0,
        Writer      = 1,
        Producer    = 2,
    }
    public enum CompanyService
    {
        Production = 0,
        Distributor = 1
    }
    public enum MetaStatus
    {
        [Description("Release")]Released = 0,
        [Description("Airing")] Airing = 1,
        [Description("Ended")] Ended = 2,
        [Description("In Production")] InProduction = 3,
    }
}