using System;
using System.Collections.Generic;
using Next.PCL.Extensions;
using Next.PCL.Metas;

namespace Next.PCL.Entities
{
    public class RootPersonEntity : NamedEntity, IPerson, IEditableEntity
    {
        public RootPersonEntity() : base()
        {
            Urls = new List<MetaUrl>();
            Images = new List<MetaImageNx>();
        }
        public RootPersonEntity(Person person) : this()
        {
            if (person != null)
            {
                Id = person.Id;
                Name = person.Name;
                if (person.Urls.IsNotNullOrEmpty())
                    Urls.AddRange(person.Urls);
                if (person.Images.IsNotNullOrEmpty())
                    Images.AddRange(person.Images);
            }
        }
        public DateTime? LastModified { get; set; }

        public List<MetaUrl> Urls { get; set; }
        public List<MetaImageNx> Images { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Id, Name);
        }
    }
}