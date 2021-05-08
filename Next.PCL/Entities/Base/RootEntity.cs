using System;

namespace Next.PCL.Entities
{
    public abstract class RootEntity<TKey>
    {
        public RootEntity()
        {
            Timestamp = GlobalClock.Now;
        }
        public virtual TKey Id { get; set; }
        public virtual DateTime Timestamp { get; set; }
    }
}