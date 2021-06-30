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
    }
}