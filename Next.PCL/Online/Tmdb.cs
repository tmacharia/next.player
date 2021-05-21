using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Next.PCL.Extensions;
using TMDbLib.Client;
using TMDbLib.Objects.Search;
using Next.PCL.Entities;
using TMDbLib.Objects.General;
using Next.PCL.Online.Models;
using System.Linq;

namespace Next.PCL.Online
{
    public class Tmdb : BaseOnline
    {
        private readonly TMDbClient _client;

        public Tmdb(string apiKey)
        {
            _client = new TMDbClient(apiKey);
        }

        public async Task<List<Crew>> GetCrewAsync(int id, MetaType type, CancellationToken token = default)
        {
            if (type == MetaType.Movie)
            {
                var res = await _client.GetMovieCreditsAsync(id, token);
                return res.Crew;
            }
            else if (type == MetaType.TvShow)
            {
                var res = await _client.GetTvShowCreditsAsync(id, null, token);
                return res.Crew;
            }
            return null;
        }
        public async Task<List<TmdbCast>> GetCastAsync(int id, MetaType type, CancellationToken token = default)
        {
            if(type == MetaType.Movie)
            {
                var res = await _client.GetMovieCreditsAsync(id, token);
                return res.Cast.Select(x => (TmdbCast)x).ToList();
            }
            else if (type == MetaType.TvShow)
            {
                var res = await _client.GetTvShowCreditsAsync(id, null, token);
                return res.Cast.Select(x => x.ToTmdbCast()).ToList();
            }
            return null;
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