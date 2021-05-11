using System;
using Newtonsoft.Json;

namespace Next.PCL.Entities
{
    public class MetaVideo : IMetaVideo
    {
        public Uri Url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Width { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Height { get; set; }
        public MetaSource Source { get; set; }
        public MetaVideoType Type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Resolution Resolution { get; set; }
        public StreamingPlatform Platform { get; set; }
    }
}