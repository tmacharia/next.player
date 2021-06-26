using Common;
using TMDbLib.Objects.Reviews;

namespace Next.PCL.Online.Models
{
    public class TmdbReview : ReviewBase
    {
        public override string ToString()
        {
            return string.Format("{0}. {1}, {2}", Id, Author, Content.Truncate(30));
        }
    }
}