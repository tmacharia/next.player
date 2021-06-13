namespace Next.PCL.Entities
{
    public interface IGeoLocale : INamedEntity
    {
        bool IsContinent { get; set; }
        bool IsCountry { get; set; }
    }
}