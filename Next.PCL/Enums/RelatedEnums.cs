namespace Next.PCL.Entities
{
    public enum MetaSource
    {
        IMDB = 0,
        TMDB = 1,
        TVDB = 2,
        OMDB = 3,
        TVMAZE = 4,
        YTS_MX = 5
    }
    public enum Resolution
    {
        SD = 480,
        HD = 720,
        UHD = 1080,
        QuadHD,
        _4K
    }
    public enum MetaImageType
    {
        Image = 0,
        Poster = 1,
        Backdrop = 2,
        Thumbnail = 3,
        Screenshot = 4,
    }
    public enum Role
    {
        Actor = 0,
        Producer = 1,
        Director = 2,
        Writer = 3,
    }
}