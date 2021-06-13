namespace Next.PCL.Configurations
{
    public class TvdbConfig
    {
        public TvdbConfig()
        {
            Language = "eng";
            TvShowSpecials = true;
            ActorsWithNoImages = false;
            TvShowSeasonsWithNoEpisodes = false;
        }
        public string Language { get; set; }
        public bool TvShowSpecials { get; set; }
        public bool TvShowSeasonsWithNoEpisodes { get; set; }
        public bool ActorsWithNoImages { get; set; }
    }
}