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
            Images = new List<MetaImage>();
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

        public List<MetaImage> Images { get; set; }
    }
}