using System;
using System.Collections.Generic;
using Next.PCL.Entities;
using Next.PCL.Metas;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbSeason : INamedEntity
    {
        public TvdbSeason()
        {
            Posters = new List<MetaImage>();
            Episodes = new List<TvdbEpisode>();
        }
        public int Id { get; set; }
        public Uri Url { get; set; }
        public string Name { get; set; }
        public int? Number { get; set; }
        public string Plot { get; set; }
        public DateTime? AirDate { get; set; }
        public DateTime? LastAirDate { get; set; }

        public List<MetaImage> Posters { get; set; }
        public List<TvdbEpisode> Episodes { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Number, Name);
        }
    }
}