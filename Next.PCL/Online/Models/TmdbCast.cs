using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace Next.PCL.Online.Models
{
    public class TmdbCast : Cast, ITmdbProfile
    {

    }
    public class TmdbCrew : Crew, ITmdbProfile
    {

    }
    public interface ITmdbProfile
    {
        string ProfilePath { get; set; }
    }
}