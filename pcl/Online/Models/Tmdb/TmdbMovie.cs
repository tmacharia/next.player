using System.Collections.Generic;
using Next.PCL.Metas;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;

namespace Next.PCL.Online.Models
{
    public class TmdbMovie : Movie, ITmdbEntity
    {
        public TmdbMovie()
        {
            Posters = new List<MetaImage>();
        }
        public List<MetaImage> Posters { get; set; }
    }
    public class TmdbShow : TvShow, ITmdbEntity
    {
        public TmdbShow()
        {
            Posters = new List<MetaImage>();
        }
        public List<MetaImage> Posters { get; set; }
    }
    public class TmdbSeason : TvSeason, IPosterPath
    {
        public TmdbSeason()
        {
            Posters = new List<MetaImage>();
        }
        public List<MetaImage> Posters { get; set; }
    }
    public interface ITmdbEntity : IPosterPath
    {
        List<Genre> Genres { get; set; }
        List<MetaImage> Posters { get; set; }
        ResultContainer<Video> Videos { get; set; }
        List<ProductionCountry> ProductionCountries { get; set; }
        List<ProductionCompany> ProductionCompanies { get; set; }
    }
}