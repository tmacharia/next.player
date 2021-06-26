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
        public override string ToString()
        {
            return string.Format("{0:yyyy} \n" +
                ":> TIT - {1}\n" +
                ":> ORG - {2}", ReleaseDate, Title, OriginalTitle);
        }
    }
    public class TmdbShow : TvShow, ITmdbEntity
    {
        public TmdbShow()
        {
            Posters = new List<MetaImage>();
        }
        public List<MetaImage> Posters { get; set; }

        public override string ToString()
        {
            return string.Format("{0:yyyy} > {1:yyyy} \n" +
                ":> {2}\n" +
                ":> {3}", FirstAirDate, LastAirDate, Name, OriginalName);
        }
    }
    public class TmdbSeason : TvSeason, IPosterPath
    {
        public TmdbSeason()
        {
            Posters = new List<MetaImage>();
        }
        public List<MetaImage> Posters { get; set; }
        public override string ToString()
        {
            return string.Format("{0}, {1} episodes\n" +
                "{2:MMM yyy} - {3:MMM yyy}", Name, Episodes.Count, AirDate, Id);
        }
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