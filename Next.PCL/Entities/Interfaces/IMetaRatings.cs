namespace Next.PCL.Entities
{
    public interface IMetaRatings : IBaseRating
    {
        /// <summary>
        /// Total no. of IMDB votes.
        /// </summary>
        int ImdbVotes { get; set; }
        /// <summary>
        /// Total no. of TMDB votes.
        /// </summary>
        int TmdbVotes { get; set; }
        /// <summary>
        /// IMDB score out of 10 based.
        /// </summary>
        double ImdbScore { get; set; }
        /// <summary>
        /// TheMovieDb score out of 100 based.
        /// </summary>
        double TmdbScore { get; set; }
        /// <summary>
        /// Roten tomatoes freshness level out of 100.
        /// </summary>
        double Tomatometer { get; set; }
        /// <summary>
        /// Roten tomatoes critics score out of 100.
        /// </summary>
        double CriticsScore { get; set; }
        /// <summary>
        /// Roten tomatoes audience score out of 100.
        /// </summary>
        double AudienceScore { get; set; }
    }
}