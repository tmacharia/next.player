namespace Next.PCL.Entities
{
    public interface IBaseRating
    {
        int Votes { get; set; }
        double Score { get; set; }
    }
}