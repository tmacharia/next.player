using System;

namespace Next.PCL.Entities
{
    public class MetaUrl : IMetaUrl
    {
        public Uri Url { get; set; }
        public MetaSource Source { get; set; }
    }
}