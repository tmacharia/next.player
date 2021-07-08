namespace Next.PCL.Services
{
    public abstract class BaseService
    {
        protected readonly ISearchQueryFormatter _searchQueryFormatter;

        public BaseService(ISearchQueryFormatter searchQueryFormatter)
        {
            _searchQueryFormatter = searchQueryFormatter;
        }
    }
    public interface IMetaServiceProvider
    {

    }
}