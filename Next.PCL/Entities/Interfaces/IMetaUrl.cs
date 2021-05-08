using System;

namespace Next.PCL.Entities
{
    public interface IMetaUrl
    {
        Uri Url { get; set; }
        MetaSource Source { get; set; }
    }
}