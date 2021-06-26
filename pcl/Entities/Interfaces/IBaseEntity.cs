namespace Next.PCL.Entities
{
    public interface IBaseEntity
    {
        int Id { get; set; }
    }
    public interface IRootEntity<TKey>
    {
        TKey Id { get; set; }
    }
}