namespace Next.PCL.Entities
{
    public class MetaRatings : BaseRating, IMetaRatings
    {
        public int ImdbVotes { get; set; }
        public int TmdbVotes { get; set; }
        public double ImdbScore { get; set; }
        public double TmdbScore { get; set; }
        public double Tomatometer { get; set; }
        public double CriticsScore { get; set; }
        public double AudienceScore { get; set; }
    }
}