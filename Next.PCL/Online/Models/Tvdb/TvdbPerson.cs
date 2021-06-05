using System;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbPerson : INamedEntity
    {
        public TvdbPerson()
        { }
        public TvdbPerson(TvdbPerson p)
        {
            Id = p.Id;
            Url = p.Url;
            Name = p.Name;
        }
        public int Id { get; set; }
        public Uri Url { get; set; }
        public string Name { get; set; }
    }
}