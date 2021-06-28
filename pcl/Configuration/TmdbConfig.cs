namespace Next.PCL.Configurations
{
    public class TmdbConfig
    {
        public TmdbConfig()
        {
            Language = "en";
            IncludeAdult = true;
            TvShowSpecials = true;
        }
        public string Language { get; set; }
        public bool IncludeAdult { get; set; }
        public bool TvShowSpecials { get; set; }
    }
}