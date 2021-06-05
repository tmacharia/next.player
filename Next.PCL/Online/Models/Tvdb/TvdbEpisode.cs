using System;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbEpisode : INamedEntity
    {
        public int Id { get; set; }
        public Uri Url { get; set; }
        public string Name { get; set; }
        public string Notation { get; set; }
        public int? Number { get; set; }
        public int? Runtime { get; set; }
        public Uri Poster { get; set; }
        public string Plot { get; set; }
        public DateTime? AirDate { get; set; }
    }
}