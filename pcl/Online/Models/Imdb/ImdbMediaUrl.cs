﻿using System;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbMediaUrl : BaseEntity
    {
        public virtual Uri Url { get; set; }
        public virtual string ImdbId { get; set; }
    }
}