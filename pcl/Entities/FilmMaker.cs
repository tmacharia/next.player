using Next.PCL.Enums;

namespace Next.PCL.Entities
{
    public class FilmMaker : Person, IFilmMaker
    {
        public Profession Role { get; set; }
    }
}