using System;
using System.Collections.Generic;
using Next.PCL.Entities;
using Next.PCL.Metas;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbMediaUrl : BaseEntity
    {
        public virtual Uri Url { get; set; }
        public virtual string ImdbId { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\n{1}", ImdbId, Url);
        }
    }
    public class ImdbImage : ImdbMediaUrl
    {
        public ImdbImage()
        {
            Sizes = new List<MetaImage>();
        }
        public virtual MetaImage TinyImage { get; set; }

        public virtual List<MetaImage> Sizes { get; set; }
    }
}