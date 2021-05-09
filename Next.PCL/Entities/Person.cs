using System;
using System.Collections.Generic;

namespace Next.PCL.Entities
{
    public class Person : NamedEntity, IPerson, IEditableEntity
    {
        public Person() :base()
        {
            Urls = new List<MetaUrl>();
            Images = new List<MetaImage>();
        }
        public DateTime? LastModified { get; set; }

        public IList<MetaUrl> Urls { get; set; }
        public IList<MetaImage> Images { get; set; }
    }
}