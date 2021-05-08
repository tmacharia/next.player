using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Next.PCL.Entities
{
    public class Person : NamedEntity, IPerson, IEditableEntity
    {
        public Person() :base()
        {
            Urls = new List<MetaUrl>();
            Images = new List<MetaImage>();
        }
        public Role Role { get; set; }

        [JsonProperty(
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Character { get; set; }
        public DateTime? LastModified { get; set; }

        public List<MetaUrl> Urls { get; set; }
        public List<MetaImage> Images { get; set; }
    }
}