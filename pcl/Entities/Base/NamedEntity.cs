namespace Next.PCL.Entities
{
    public class NamedEntity : BaseEntity, INamedEntity
    {
        public virtual string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    public class NamedEntity<TKey> : RootEntity<TKey>, INamedEntity
    {
        public virtual string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}