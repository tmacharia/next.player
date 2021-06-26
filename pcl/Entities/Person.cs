using System;
using System.Collections.Generic;
using Next.PCL.Metas;

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

        public List<MetaUrl> Urls { get; set; }
        public List<MetaImage> Images { get; set; }
    }
}