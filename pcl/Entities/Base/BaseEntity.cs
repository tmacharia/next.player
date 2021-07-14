using System;
using System.Collections.Generic;
using Common;

namespace Next.PCL.Entities
{
    public abstract class BaseEntity : RootEntity<int>, IBaseEntity, IEquatable<BaseEntity>, IEqualityComparer<BaseEntity>
    {
        public BaseEntity() : base()
        {
            Id = Constants.random.Next();
        }

        public bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            return Id == other.Id;
        }

        public bool Equals(BaseEntity x, BaseEntity y)
        {
            if (x == null)
                return false;

            return x.Equals(y);
        }

        public int GetHashCode(BaseEntity obj)
        {
            return obj.Id;
        }
    }
}