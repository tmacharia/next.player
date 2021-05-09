using System;
using System.Collections.Generic;

namespace Next.PCL.Entities
{
    public interface IMetaUrl
    {
        Uri Url { get; set; }
        MetaSource Source { get; set; }
    }
    public interface IUrls
    {
        IList<MetaUrl> Urls { get; set; }
    }
}