namespace Next.PCL.Entities
{
    public class MetaID : RootEntity<string>, IMetaID
    {
        public MetaSource Source { get; set; }
    }
}