using System;
using System.Collections.Generic;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbSeason : INamedEntity
    {
        public TvdbSeason()
        {
            Posters = new List<Uri>();
            Episodes = new List<TvdbEpisode>();
        }
        public int Id { get; set; }
        public Uri Url { get; set; }
        public string Name { get; set; }
        public int? Number { get; set; }
        public string Plot { get; set; }
        public DateTime? AirDate { get; set; }

        public List<Uri> Posters { get; set; }
        public List<TvdbEpisode> Episodes { get; set; }
    }
}