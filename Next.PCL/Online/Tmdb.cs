using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Next.PCL.Extensions;
using TMDbLib.Client;
using TMDbLib.Objects.Search;

namespace Next.PCL.Online
{
    public class Tmdb : BaseOnline
    {
        private readonly TMDbClient _client;

        public Tmdb(string apiKey)
        {
            _client = new TMDbClient(apiKey);
        }

        #region Search & Lookup
        internal async Task<List<SearchCompany>> SearchCompanyAsync(string q, CancellationToken token = default)
        {
            var res = await _client.SearchCompanyAsync(q, cancellationToken: token);
            return res.GetList();
        }
        public async Task<List<SearchTv>> SearchShowAsync(string q, int year = 0, bool nsfw = false, CancellationToken token = default)
        {
            var res = await _client.SearchTvShowAsync(q, firstAirDateYear: year, includeAdult: nsfw, cancellationToken: token);
            return res.GetList();
        }
        public async Task<List<SearchMovie>> SearchMovieAsync(string q, int year = 0, bool nsfw = false, CancellationToken token = default)
        {
            var res = await _client.SearchMovieAsync(q, year: year, includeAdult: nsfw, cancellationToken: token);
            return res.GetList();
        }
        
        internal async Task<List<SearchBase>> SearchAsync(string q, int year = 0, bool nsfw = false, CancellationToken token = default)
        {
            var res = await _client.SearchMultiAsync(q, year: year, includeAdult: nsfw, cancellationToken: token);
            return res.GetList();
        }
        #endregion

    }
}