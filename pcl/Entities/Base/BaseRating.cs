namespace Next.PCL.Entities
{
    public class BaseRating : EditableEntity, IBaseRating
    {
        public virtual int Votes { get; set; }
        public virtual double Score { get; set; }

        public override string ToString()
        {
            return string.Format("{0:N1}/10 {1:N0} votes", Score, Votes);
        }
    }
}