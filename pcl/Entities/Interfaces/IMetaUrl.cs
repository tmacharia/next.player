using System;
using System.Collections.Generic;
using Next.PCL.Enums;

namespace Next.PCL.Entities
{
    public interface IMetaUrl
    {
        Uri Url { get; set; }
        MetaSource Source { get; set; }
    }
    public interface IUrls
    {
        List<MetaUrl> Urls { get; set; }
    }
}