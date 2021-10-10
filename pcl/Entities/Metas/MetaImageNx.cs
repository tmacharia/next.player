using System;
using System.Collections.Generic;
using Common;
using Newtonsoft.Json;
using Next.PCL.Entities;
using Next.PCL.Enums;

namespace Next.PCL.Metas
{
    public class MetaImageNx : IMetaImage, IEquatable<MetaImageNx>, IEqualityComparer<MetaImageNx>
    {
        public MetaImageNx()
        { }
        public MetaImageNx(int h, int w)
        {
            Width = (ushort)w;
            Height = (ushort)h;
        }
        public MetaImageNx(MetaImageType type, MetaSource src, Uri uri = default)
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


        public bool Equals(MetaImageNx x)
        {
            if (x == null)
                return false;
            if (Source != x.Source)
                return false;
            if (Type != x.Type)
                return false;
            if (Resolution != x.Resolution)
                return false;
            if (Width != x.Width)
                return false;
            if (Height != x.Height)
                return false;
            if (Url == null && x.Url == null)
                return true;

            if (Url != null)
            {
                if (x.Url != null)
                {
                    return Url.OriginalString.EqualsOIC(x.Url.OriginalString);
                }
            }
            return false;
        }
        public bool Equals(MetaImageNx x, MetaImageNx y)
        {
            return x.Equals(y);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}x{2}, {3}, {4}", Type, Width, Height, Resolution, Url);
        }
        public int GetHashCode(MetaImageNx obj)
        {
            return obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + Source.GetHashCode();
                hash = (hash * 23) + Type.GetHashCode();
                hash = (hash * 23) + Resolution.GetHashCode();
                hash = (hash * 23) + Width.GetHashCode();
                hash = (hash * 23) + Height.GetHashCode();
                hash = (hash * 23) + Url?.GetHashCode() ?? 0;
                return hash;
            }
        }
    }
}