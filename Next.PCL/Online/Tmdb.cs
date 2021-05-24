using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Next.PCL.Extensions;
using TMDbLib.Client;
using TMDbLib.Objects.Search;
using Next.PCL.Enums;
using TMDbLib.Objects.General;
using Next.PCL.Online.Models;
using System.Linq;
using System.IO;
using Common;
using TMDbLib.Objects.Companies;
using Next.PCL.Exceptions;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;

namespace Next.PCL.Online
{
    public class Tmdb : BaseOnline
    {
        private readonly TMDbClient _client;

        public Tmdb(string apiKey)
        {
            if (!apiKey.IsValid())
                throw new ApiKeyException("TMdb Api key is required.");
            _client = new TMDbClient(apiKey);
        }

        public async Task<Movie> GetMovieAsync(int id = 0, string imdbId = default, CancellationToken token = default)
        {
            Movie mov = null;
            if (id > 0)
                mov = await _client.GetMovieAsync(id, extraMethods: MovieMethods.ExternalIds, cancellationToken: token);
            else if(imdbId.IsValid())
                mov = await _client.GetMovieAsync(imdbId, MovieMethods.ExternalIds, cancellationToken: token);
            return mov;
        }
        public async Task<TvShow> GetShowAsync(int id, CancellationToken token = default)
        {
            TvShow tv = await _client.GetTvShowAsync(id, TvShowMethods.ExternalIds, cancellationToken: token);
            return tv;
        }

        public async Task<List<TmdbCrew>> GetCrewAsync(int id, MetaType type, CancellationToken token = default)
        {
            if (type == MetaType.Movie)
            {
                var res = await _client.GetMovieCreditsAsync(id, token);
                return res.Crew.ToList2();
            }
            else if (type == MetaType.TvShow)
            {
                var res = await _client.GetTvShowCreditsAsync(id, null, token);
                return res.Crew.ToList2();
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

        public async Task<Company> GetCompanyAsync(int id, CancellationToken token = default)
        {
            var res = await _client.GetCompanyAsync(id, cancellationToken: token);
            return res;
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

        #region Private Section
        private async Task<TMDbConfig> GetConfigAsync()
        {
            string path = Path.Combine(FileSys.SettingsFolder, "tmdb.json");
            FileInfo file = new FileInfo(path);

            if (file.Exists && file.LastWriteTime >= GlobalClock.Now.AddHours(-24))
            {
                var config = File.ReadAllText(path).DeserializeTo<TMDbConfig>();
                _client.SetConfig(config);
                return config;
            }
            else
            {
                var config = await _client.GetConfigAsync();
                File.WriteAllText(path, config.ToJson());
                return config;
            }
        }
        #endregion
    }
}