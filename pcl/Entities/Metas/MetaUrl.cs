using System;
using System.Collections.Generic;
using Common;
using Next.PCL.Enums;

namespace Next.PCL.Entities
{
    public class MetaUrl : IMetaUrl, IEquatable<MetaUrl>, IEqualityComparer<MetaUrl>
    {
        public MetaUrl()
        { }
        public MetaUrl(MetaSource metaSource) : this()
        {
            Source = metaSource;
        }
        public MetaUrl(MetaSource metaSource, Uri uri) 
            : this(metaSource)
        {
            Url = uri;
        }
        public Uri Url { get; set; }
        public MetaSource Source { get; set; }
        public OtherSiteDomain Domain { get; set; }

        public bool Equals(MetaUrl x)
        {
            if (x == null)
                return false;
            if (Source != x.Source)
                return false;
            if (Domain != x.Domain)
                return false;
            if (Url == null && x.Url == null)
                return true;

            if(Url != null)
            {
                if(x.Url != null)
                {
                    return Url.OriginalString.EqualsOIC(x.Url.OriginalString);
                }
            }
            return false;
        }

        public bool Equals(MetaUrl x, MetaUrl y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(MetaUrl obj)
        {
            return obj.GetHashCode();
        }
        
        public override string ToString()
        {
            return string.Format("[{0}, {1}]\t{2}", Domain, Source, Url);
        }
        public override bool Equals(object obj)
        {
            if (obj is MetaUrl mu)
                return Equals(mu);
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = (hash * 23) + Source.GetHashCode();
                hash = (hash * 23) + Domain.GetHashCode();
                hash = (hash * 23) + Url?.GetHashCode() ?? 0;
                return hash;
            }
        }
    }
}