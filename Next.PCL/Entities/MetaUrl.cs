using System;
using System.Collections.Generic;
using Common;

namespace Next.PCL.Entities
{
    public class MetaUrl : IMetaUrl, IEquatable<MetaUrl>, IEqualityComparer<MetaUrl>
    {
        public Uri Url { get; set; }
        public MetaSource Source { get; set; }

        public bool Equals(MetaUrl x)
        {
            if (x == null)
                return false;
            if (Source != x.Source)
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
    }
}