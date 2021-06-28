using Next.PCL.Enums;

namespace Next.PCL.Entities
{
    public class FilmMaker : Person, IFilmMaker
    {
        public FilmMaker() :base()
        { }
        public FilmMaker(Profession profession) :this()
        {
            Profession = profession;
        }
        public FilmMaker(Person person, Profession profession)
            :base(person)
        {
            Profession = profession;
        }
        public Profession Profession { get; set; }
        public override string Role => Profession.ToString();
    }
}