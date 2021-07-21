using System;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbEpisode : NamedEntity
    {
        public ImdbEpisode()
        {
            Rating = new BaseRating();
        }
        public int Number { get; set; }
        public string Notation { get; set; }
        public virtual Uri Url { get; set; }
        public virtual Uri Poster { get; set; }
        public virtual string ImdbId { get; set; }
        public virtual string Plot { get; set; }
        public virtual int? Runtime { get; set; }
        public virtual DateTime? ReleaseDate { get; set; }
        public virtual BaseRating Rating { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1} - {2}, {3}, {4:dd MMM yyyy}" +
                "\n=> {5}" +
                "\n\t{6}" +
                "\n\t{7}",
                Number, Notation, Name, ImdbId, ReleaseDate,
                Rating.ToString(),
                Url,
                Poster);
        }
    }
}