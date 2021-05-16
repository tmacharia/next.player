using System;
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
    }
    public class BaseOnlineModel : IBaseOnlineModel
    {
        public virtual Uri Url { get; set; }
        public MetaSource Source { get; set; }
        public virtual string Name { get; set; }
        public virtual string ImdbId { get; set; }
        public virtual Uri Poster { get; set; }
        public virtual string Plot { get; set; }
        public virtual int? Runtime { get; set; }
        public virtual DateTime? ReleaseDate { get; set; }
    }
}