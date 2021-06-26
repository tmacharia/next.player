using Next.PCL.Online;

namespace Next.PCL.Services
{
    public abstract class BaseMetaService : BaseService
    {
        protected readonly Yts _yts;
        protected readonly Tmdb _tmdb;
        protected readonly Omdb _omdb;
        protected readonly Tvdb _tvdb;
        protected readonly Imdb _imdb;
        protected readonly TvMaze _tvmaze;

        public BaseMetaService(
            ISearchQueryFormatter searchQueryFormatter,
            Yts yts, Tmdb tmdb, Omdb omdb, Tvdb tvdb, Imdb imdb, TvMaze tvMaze)
            : base(searchQueryFormatter)
        {

        }
    }
}