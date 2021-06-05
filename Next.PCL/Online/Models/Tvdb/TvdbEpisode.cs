using System;
using System.Collections.Generic;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbEpisode : INamedEntity
    {
        public TvdbEpisode()
        {
            Crews = new List<TvdbCrew>();
            Guests = new List<TvdbCast>();
        }
        public int Id { get; set; }
        public Uri Url { get; set; }
        public string Name { get; set; }
        public string Notation { get; set; }
        public int? Number { get; set; }
        public int? Runtime { get; set; }
        public Uri Poster { get; set; }
        public string Plot { get; set; }
        public DateTime? AirDate { get; set; }

        public List<TvdbCrew> Crews { get; set; }
        public List<TvdbCast> Guests { get; set; }
    }
}