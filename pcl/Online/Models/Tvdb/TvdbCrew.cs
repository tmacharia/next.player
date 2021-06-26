using Next.PCL.Enums;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbCrew : TvdbPerson
    {
        public TvdbCrew(Profession profession) :base()
        {
            Profession = profession;
        }
        public TvdbCrew(TvdbPerson p):base(p)
        { }
        public Profession Profession { get; set; }
        public override string Role => Profession.ToString();
    }
}