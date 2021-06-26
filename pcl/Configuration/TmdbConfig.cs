namespace Next.PCL.Configurations
{
    public class TmdbConfig
    {
        public TmdbConfig()
        {
            Language = "en";
            TvShowSpecials = true;
        }
        public string Language { get; set; }
        public bool TvShowSpecials { get; set; }
    }
}