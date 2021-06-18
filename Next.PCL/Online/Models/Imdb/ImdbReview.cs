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

        public int? MarkedAsUseful { get; set; }
        public int? TotalEngagement { get; set; }

        public double? QuorumScore
        {
            get
            {
                if(MarkedAsUseful.HasValue && TotalEngagement.HasValue)
                {
                    if(MarkedAsUseful <= TotalEngagement)
                    {
                        return ((double)MarkedAsUseful / (double)TotalEngagement) * 100;
                    }
                }
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format("{0:N0}/10 by {1}, {2:N2}% agreed", Score, Reviewer, QuorumScore);
        }
    }
}