using System;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbReview
    {
        public Uri Url { get; set; }
        public double? Score { get; set; }
        public string Title { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Review { get; set; }
        public string Reviewer { get; set; }
        public Uri ReviewerUrl { get; set; }

        public override string ToString()
        {
            return string.Format("{0:N1}/10 by {1}", Score, Reviewer);
        }
    }
}