using System;
using System.Collections.Generic;
using Next.PCL.Entities;
using Next.PCL.Metas;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbEpisode : INamedEntity
    {
        public TvdbEpisode()
        {
            Crews = new List<FilmMaker>();
            Guests = new List<Cast>();
            Images = new List<MetaImageNx>();
        }
        public int Id { get; set; }
        public Uri Url { get; set; }
        public string Name { get; set; }
        public string Notation { get; set; }
        public int? Number { get; set; }
        public int? Runtime { get; set; }
        public string Plot { get; set; }
        public DateTime? AirDate { get; set; }

        public List<FilmMaker> Crews { get; set; }
        public List<Cast> Guests { get; set; }
        public List<MetaImageNx> Images { get; set; }
    }
}