using System;
using System.Collections.Generic;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Metas;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TinyTvdbModel : INamedEntity, IMetaUrl
    {
        public TinyTvdbModel()
        {
            Source = MetaSource.TVDB;
            Posters = new List<MetaImageNx>();
        }
        public Uri Url { get; set; }
        public string Name { get; set; }
        public MetaSource Source { get; set; }
        public List<MetaImageNx> Posters { get; set; }
    }
}