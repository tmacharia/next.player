using System;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Imdb
{
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