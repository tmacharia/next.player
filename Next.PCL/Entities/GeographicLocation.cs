namespace Next.PCL.Entities
{
    public class GeographicLocation : NamedEntity<string>, IGeoLocale
    {
        public GeographicLocation()
        { }
        public GeographicLocation(string name)
        {
            Name = name;
        }

        public bool IsContinent { get; set; }
        public bool IsCountry { get; set; }
    }
}