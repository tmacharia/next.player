namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbCast : TvdbPerson
    {
        public TvdbCast() :base()
        { }
        public TvdbCast(TvdbPerson p) : base(p)
        { }
        public string Role { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1} as {2}", Id, Name, Role);
        }
    }
}