using System;
using Newtonsoft.Json;

namespace Next.PCL.Entities
{
    public class MetaImage : IMetaImage
    {
        public Uri Url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Width { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Height { get; set; }
        public MetaSource Source { get; set; }
        public MetaImageType Type { get; set; }
        public Resolution Resolution { get; set; }
    }
}