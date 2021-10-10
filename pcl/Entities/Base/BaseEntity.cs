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
            return other != null && Id == other.Id;
        }
        public bool Equals(BaseEntity x, BaseEntity y)
        {
            return x != null && x.Equals(y);
        }
        public int GetHashCode(BaseEntity obj)
        {
            return obj != null ? obj.Id : 0;
        }
    }
}