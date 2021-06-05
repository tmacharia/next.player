namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbCast : TvdbPerson
    {
        public TvdbCast(TvdbPerson p) : base(p)
        { }
        public string Role { get; set; }
    }
}