using System;
using System.Collections.Generic;
using Next.PCL.Entities;
using Next.PCL.Metas;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbPerson : INamedEntity
    {
        public TvdbPerson()
        {
            Images = new List<MetaImageNx>();
        }
        public TvdbPerson(TvdbPerson p) :this()
        {
            Id = p.Id;
            Url = p.Url;
            Name = p.Name;
        }
        public int Id { get; set; }
        public Uri Url { get; set; }
        public string Name { get; set; }
        public virtual string Role { get; set; }

        public List<MetaImageNx> Images { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1} as {2}", Id, Name, Role);
        }
    }
}