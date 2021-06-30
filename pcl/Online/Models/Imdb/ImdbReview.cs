using Next.PCL.Entities;
using System;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbReview : ReviewComment
    {
        public Uri AuthorUrl { get; set; }

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
            return string.Format("{0:N0}/10 by {1}, {2:N2}% agreed", Score, Author, QuorumScore);
        }
    }
}