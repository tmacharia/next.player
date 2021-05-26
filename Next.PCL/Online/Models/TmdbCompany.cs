using System.Collections.Generic;
using Next.PCL.Metas;

namespace Next.PCL.Online.Models
{
    public class TmdbCompany
    {
        public TmdbCompany()
        {
            Logos = new List<MetaImage>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Headquarters { get; set; }
        public string Homepage { get; set; }
        public string LogoPath { get; set; }
        public string OriginCountry { get; set; }

        public List<MetaImage> Logos { get; set; }
    }
}