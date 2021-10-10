using System;
using System.Collections.Generic;
using Common;
using Next.PCL.Enums;

namespace Next.PCL.Entities
{
    public class MetaID : RootEntity<string>, IMetaID, IEquatable<MetaID>, IEqualityComparer<MetaID>
    {
        public MetaID()
        { }
        public MetaID(MetaSource metaSource) : this()
        {
            Source = metaSource;
        }
        public MetaID(MetaSource metaSource, string id)
            : this(metaSource)
        {
            Id = id;
        }
        public MetaSource Source { get; set; }

        public bool Equals(MetaID x)
        {
            if (x == null)
                return false;
            if (Source != x.Source)
                return false;
            if (Id.EqualsOIC(x.Id))
                return true;
            return false;
        }

        public bool Equals(MetaID x, MetaID y)
        {
            return x.Equals(y);
        }
        
        public int GetHashCode(MetaID obj)
        {
            return obj.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is MetaID mu)
                return Equals(mu);
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            if (Id.IsValid())
            {
                if(int.TryParse(Id, out int n))
                {
                    return n;
                }
            }
            return base.GetHashCode();
        }
    }
}