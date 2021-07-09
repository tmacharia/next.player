using Next.PCL.Online;
using System.Threading;
using System.Threading.Tasks;

namespace Next.PCL.Services
{
    public class MovieService : BaseMetaService
    {
        public MovieService(
            ISearchQueryFormatter searchQueryFormatter,
            Yts yts, Tmdb tmdb, Omdb omdb, Tvdb tvdb, Imdb imdb, TvMaze tvMaze)
            : base(searchQueryFormatter,yts,tmdb,omdb,tvdb,imdb,tvMaze)
        {

        }

        public async Task SearchAsync(string q, CancellationToken cancellationToken = default)
        {
            var query = _searchQueryFormatter.CleanAndFormat(q);

            var ytsResponse = await _yts.SearchAsync(query.Term, cancellationToken: cancellationToken);
        }
    }
}