using System;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbEpisode : NamedEntity
    {
        public int Number { get; set; }
        public string Notation { get; set; }
        public virtual Uri Url { get; set; }
        public virtual Uri Poster { get; set; }
        public virtual string ImdbId { get; set; }
        public virtual string Plot { get; set; }
        public virtual int? Runtime { get; set; }
        public virtual DateTime? ReleaseDate { get; set; }
    }
}