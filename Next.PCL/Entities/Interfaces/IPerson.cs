using System.Collections.Generic;

namespace Next.PCL.Entities
{
    public interface IPerson
    {
        Role Role { get; set; }
        string Name { get; set; }
        List<MetaUrl> Urls { get; set; }
        List<MetaImage> Images { get; set; }
    }
}