using System;
using System.Collections.Generic;
using Next.PCL.Extensions;
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
        public Person(Person person) :this()
        {
            if(person != null)
            {
                Id = person.Id;
                Name = person.Name;
                if (person.Urls.IsNotNullOrEmpty())
                    Urls.AddRange(person.Urls);
                if (person.Images.IsNotNullOrEmpty())
                    Images.AddRange(person.Images);
            }
        }
        public virtual string Role { get; set; }
        public DateTime? LastModified { get; set; }

        public List<MetaUrl> Urls { get; set; }
        public List<MetaImage> Images { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1} as {2}", Id, Name, Role);
        }
    }
}