using System;
using System.Collections.Generic;
using Next.PCL.Entities;
using Next.PCL.Metas;
using TMDbLib.Objects.Search;

namespace Next.PCL.Online.Models
{
    public class TmdbCompany : INamedEntity
    {
        public TmdbCompany()
        {
            Logos = new List<MetaImage>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Headquarters { get; set; }
        public string Homepage { get; set; }
        public string LogoPath { get; set; }
        public string OriginCountry { get; set; }

        public List<MetaImage> Logos { get; set; }
    }
    public class TmdbSearch : SearchMovieTvBase, INamedEntity, IPosterPath
    {
        public TmdbSearch()
        {
            Posters = new List<MetaImage>();
        }
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public List<MetaImage> Posters { get; set; }

        public override string ToString()
        {
            return string.Format("{0}. {1}, {2:yyyy}", Id, Name, ReleaseDate);
        }
    }
    public interface IPosterPath
    {
        string PosterPath { get; set; }
    }
}