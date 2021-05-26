using System.Collections.Generic;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;

namespace Next.PCL.Online.Models
{
    public class TmdbMovie : Movie, ITmdbEntity
    {
        
    }
    public class TmdbShow : TvShow, ITmdbEntity
    {

    }
    public interface ITmdbEntity
    {
        List<Genre> Genres { get; set; }
        ResultContainer<Video> Videos { get; set; }
        List<ProductionCountry> ProductionCountries { get; set; }
        List<ProductionCompany> ProductionCompanies { get; set; }
    }
}