namespace Next.PCL.Configurations
{
    public class TvdbConfig
    {
        public TvdbConfig()
        {
            Language = "eng";
            TvShowSpecials = true;
            IgnoreActorsWithNoImages = true;
            IgnoreMediasWithNoImages = true;
            TvShowSeasonsWithNoEpisodes = false;
        }
        public string Language { get; set; }
        public bool TvShowSpecials { get; set; }
        public bool TvShowSeasonsWithNoEpisodes { get; set; }
        public bool IgnoreActorsWithNoImages { get; set; }
        public bool IgnoreMediasWithNoImages { get; set; }
    }
}