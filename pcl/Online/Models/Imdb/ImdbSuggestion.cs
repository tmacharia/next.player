using System;
using Next.PCL.Entities;
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
    public class ImdbUserList : INamedEntity
    {
        public string ListId { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }
        public Uri ImageUrl { get; set; }
        public int? TitlesCount { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1:N0}, {2}", ListId, TitlesCount, Name);
        }
    }
}