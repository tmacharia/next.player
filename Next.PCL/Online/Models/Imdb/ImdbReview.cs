using System;
using System.Collections.Generic;
using System.Text;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbReview
    {
        public double Score { get; set; }
        public string Title { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Review { get; set; }
    }
}