using System;

namespace Next.PCL.Entities
{
    public interface IIdEntity<TKey>
    {
        TKey Id { get; set; }
    }
    public abstract class RootEntity<TKey> : IIdEntity<TKey>
    {
        public RootEntity()
        {
            Timestamp = GlobalClock.Now;
        }
        public virtual TKey Id { get; set; }
        public virtual DateTime Timestamp { get; set; }
    }
}