using System;
using System.Collections.Generic;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models
{
    public interface IBaseOnlineModel : INamedEntity, IMetaUrl
    {
        string ImdbId { get; set; }
        Uri Poster { get; set; }
        int? Runtime { get; set; }
        string Plot { get; set; }
        DateTime? ReleaseDate { get; set; }
        List<string> Genres { get; set; }
    }
    public class BaseOnlineModel : IBaseOnlineModel
    {
        public BaseOnlineModel()
        {
            Genres = new List<string>();
        }
        public virtual Uri Url { get; set; }
        public MetaSource Source { get; set; }
        public virtual string Name { get; set; }
        public virtual string ImdbId { get; set; }
        public virtual Uri Poster { get; set; }
        public virtual string Plot { get; set; }
        public virtual int? Runtime { get; set; }
        public virtual DateTime? ReleaseDate { get; set; }

        public virtual List<string> Genres { get; set; }

        public override string ToString()
        {
            return string.Format("{0} | {1}, {2} mins | {3}", Name, ImdbId, Runtime, Source);
        }
    }
}