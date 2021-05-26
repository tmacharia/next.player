using System;
using Newtonsoft.Json;
using Next.PCL.Entities;
using Next.PCL.Enums;

namespace Next.PCL.Metas
{
    public class MetaVideo : IMetaVideo
    {
        public string Key { get; set; }
        public Uri Url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ushort Width { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ushort Height { get; set; }
        public MetaSource Source { get; set; }
        public MetaVideoType Type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Resolution Resolution { get; set; }
        public StreamingPlatform Platform { get; set; }
    }
}