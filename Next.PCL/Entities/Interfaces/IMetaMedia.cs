using System;
using System.Collections.Generic;
using Next.PCL.Enums;
using Next.PCL.Metas;

namespace Next.PCL.Entities
{
    public interface IMetaMedia : IResolution
    {
        Uri Url { get; set; }
        MetaSource Source { get; set; }
    }
    public interface IMetaImage : IMetaMedia
    {
        MetaImageType Type { get; set; }
    }
    public interface IMetaImages
    {
        IList<MetaImage> Images { get; set; }
    }
    public interface IMetaVideos
    {
        IList<MetaVideo> Videos { get; set; }
    }
    public interface IMetaVideo : IMetaMedia
    {
        string Key { get; set; }
        MetaVideoType Type { get; set; }
        StreamingPlatform Platform { get; set; }
    }
}