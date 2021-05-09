namespace Next.PCL.Entities
{
    public class BaseRating : EditableEntity, IBaseRating
    {
        public int Votes { get; set; }
        public double Score { get; set; }
    }
}