using Next.PCL.Enums;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbCrew : TvdbPerson
    {
        public TvdbCrew(TvdbPerson p):base(p)
        { }
        public Profession Role { get; set; }
    }
}