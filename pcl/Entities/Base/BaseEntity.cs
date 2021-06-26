using Common;

namespace Next.PCL.Entities
{
    public abstract class BaseEntity : RootEntity<int>, IBaseEntity
    {
        public BaseEntity() : base()
        {
            Id = Constants.random.Next();
        }
    }
}