using Next.PCL.Enums;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbSuggestion : BaseOnlineModel
    {
        public ImdbSuggestion() :base()
        {
            Source = MetaSource.IMDB;
        }
        public double? Score { get; set; }

        public override string ToString()
        {
            return string.Format("{0:N1}, {1}({2:yyyy})", Score, Name, ReleaseDate);
        }
    }
}