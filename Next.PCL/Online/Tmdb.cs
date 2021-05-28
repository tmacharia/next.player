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
using Next.PCL.Metas;
using AutoMapper;
using TMDbLib.Objects.Reviews;

namespace Next.PCL.Online
{
    public class Tmdb : BaseOnline
    {
        private readonly IMapper _mapper;
        private readonly TMDbClient _client;

        public Tmdb(string apiKey, IMapper mapper)
        {
            if (!apiKey.IsValid())
                throw new ApiKeyException("TMdb Api key is required.");
            _client = new TMDbClient(apiKey);
            _mapper = mapper;
        }

        public async Task<TmdbMovie> GetMovieAsync(int id = 0, string imdbId = default, CancellationToken token = default)
        {
            Movie res = null;
            if (id > 0)
                res = await _client.GetMovieAsync(id, extraMethods: MovieMethods.ExternalIds, cancellationToken: token);
            else if(imdbId.IsValid())
                res = await _client.GetMovieAsync(imdbId, MovieMethods.ExternalIds, cancellationToken: token);

            var mov = _mapper.Map<TmdbMovie>(res);
            return mov;
        }
        public async Task<List<MetaImage>> GetMovieImagesAsync(int id, CancellationToken token = default)
        {
            ImagesWithId res = await _client.GetMovieImagesAsync(id, cancellationToken: token);
            return res.GetAllImages(_client);
        }
        public async Task<List<MetaVideo>> GetMovieVideosAsync(int id, CancellationToken token = default)
        {
            var res = await _client.GetMovieVideosAsync(id, cancellationToken: token);
            return res.GetVideos();
        }

        public async Task<TmdbShow> GetShowAsync(int id, CancellationToken token = default)
        {
            TvShow res = await _client.GetTvShowAsync(id, TvShowMethods.ExternalIds, cancellationToken: token);
            var show = _mapper.Map<TmdbShow>(res);
            return show;
        }
        public async Task<List<MetaImage>> GetShowImagesAsync(int id, CancellationToken token = default)
        {
            ImagesWithId res = await _client.GetTvShowImagesAsync(id, cancellationToken: token);
            return res.GetAllImages(_client);
        }
        public async Task<List<MetaVideo>> GetShowVideosAsync(int id, CancellationToken token = default)
        {
            var res = await _client.GetTvShowVideosAsync(id, cancellationToken: token);
            return res.GetVideos();
        }
        public async Task<List<MetaImage>> GetSeasonImagesAsync(int showId, int season, CancellationToken token = default)
        {
            PosterImages res = await _client.GetTvSeasonImagesAsync(showId,season, cancellationToken: token);
            return res.GetPosters(_client);
        }
        public async Task<List<MetaImage>> GetEpisodeImagesAsync(int showId, int season, int episode, CancellationToken token = default)
        {
            StillImages res = await _client.GetTvEpisodeImagesAsync(showId, season,episode, cancellationToken: token);
            return res.GetStills(_client);
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
        public async Task<List<ReviewBase>> GetReviewsAsync(int id, MetaType type, CancellationToken token = default)
        {
            if (type == MetaType.Movie)
            {
                var res = await _client.GetMovieReviewsAsync(id, cancellationToken: token);
                return res.GetList();
            }
            else if (type == MetaType.TvShow)
            {
                var res = await _client.GetTvShowReviewsAsync(id, cancellationToken: token);
                return res.GetList();
            }
            return null;
        }

        public async Task<TmdbCompany> GetCompanyAsync(int id, CancellationToken token = default)
        {
            var res = await _client.GetCompanyAsync(id, cancellationToken: token);
            var comp = _mapper.Map<TmdbCompany>(res);
            comp.Logos.AddRange(comp.GetLogos(_client));
            return comp;
        }
        public async Task<TmdbCompany> GetNetworkAsync(int id, CancellationToken token = default)
        {
            var res = await _client.GetNetworkAsync(id, cancellationToken: token);
            var comp = _mapper.Map<TmdbCompany>(res);
            var imgs = await _client.GetNetworkImagesAsync(id);
            comp.Logos.AddRange(imgs.Logos.AsMetaImages(MetaImageType.Logo, _client));
            return comp;
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