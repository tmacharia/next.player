using System.Collections.Generic;
using System.Linq;

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
            if (Inner.Any())
                return string.Format("{0} | {1}", Name, string.Join(";", Inner.Select(x => x.Name)));
            return Name;
        }
    }
}