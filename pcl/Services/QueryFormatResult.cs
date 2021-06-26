namespace Next.PCL.Services
{
    public struct QueryFormatResult
    {
        public string Term { get; set; }
        public int? Year { get; set; }

        public override string ToString()
        {
            if (!Year.HasValue)
                return Term;
            return string.Format("{0} | {1}", Term, Year);
        }
    }
}