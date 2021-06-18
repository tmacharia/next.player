using System.Collections.Generic;

namespace Next.PCL.Entities
{
    public class GeographicLocation : NamedEntity<string>, IGeoLocale
    {
        public GeographicLocation()
        {
            Inner = new List<GeographicLocation>();
        }
        public GeographicLocation(string name)
            :this()
        {
            Name = name;
        }

        public bool IsContinent { get; set; }
        public bool IsCountry { get; set; }

        public List<GeographicLocation> Inner { get; set; }

        public override string ToString()
        {
            return string.Format("{0} | {1} inner places", Name, Inner.Count);
        }
    }
}