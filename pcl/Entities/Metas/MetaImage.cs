using System;
using Newtonsoft.Json;
using Next.PCL.Entities;
using Next.PCL.Enums;

namespace Next.PCL.Metas
{
    public class MetaImage : IMetaImage
    {
        public MetaImage()
        { }
        public MetaImage(int h, int w)
        {
            Width = (ushort)w;
            Height = (ushort)h;
        }
        public MetaImage(MetaImageType type, MetaSource src, Uri uri = default)
        {
            Type = type;
            Source = src;
            if (uri != null)
                Url = uri;
        }
        public Uri Url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ushort Width { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ushort Height { get; set; }
        public MetaSource Source { get; set; }
        public MetaImageType Type { get; set; }
        public Resolution Resolution { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}x{2}, {3}", Type, Width, Height, Url);
        }
    }
}