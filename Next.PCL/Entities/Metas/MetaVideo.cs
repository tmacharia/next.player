﻿using System;
using Newtonsoft.Json;
using Next.PCL.Enums;

namespace Next.PCL.Entities
{
    public class MetaVideo : IMetaVideo
    {
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