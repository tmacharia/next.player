namespace Next.PCL.Services
{
    public interface ISearchQueryFormatter
    {
        QueryFormatResult CleanAndFormat(string query);
    }
}