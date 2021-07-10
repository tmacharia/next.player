using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Next.PCL.Extensions;
using TMDbLib.Client;
using TMDbLib.Objects.Search;
using Next.PCL.Enums;
using TMDbLib.Objects.General;
using Next.PCL.Online.Models;
using System.IO;
using Common;
using Next.PCL.Exceptions;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;
using Next.PCL.Metas;
using AutoMapper;
using Next.PCL.Configurations;
using Next.PCL.Services;

namespace Next.PCL.Online
{
    public class Tmdb : IMetaReviewsProvider
    {
        private readonly IMapper _mapper;
        private readonly TMDbClient _client;

        internal TmdbConfig Config { get; set; }
        public MetaSource Source => MetaSource.TMDB;

        public Tmdb(string apiKey, IMapper mapper, TmdbConfig tmdbConfig = default)
        {
            if (!apiKey.IsValid())
                throw new ApiKeyException("TMdb Api key is required.");
            _client = new TMDbClient(apiKey);
            Config = tmdbConfig ?? new TmdbConfig();
            _mapper = mapper;
        }

        public async Task<TmdbMovie> GetMovieAsync(int id = 0, string imdbId = default, CancellationToken cancellationToken = default)
        {
            Movie res = null;
            if (id > 0)
                res = await _client.GetMovieAsync(id, extraMethods: MovieMethods.ExternalIds, cancellationToken: cancellationToken);
            else if(imdbId.IsValid())
                res = await _client.GetMovieAsync(imdbId, MovieMethods.ExternalIds, cancellationToken: cancellationToken);

            var map = _mapper.Map<TmdbMovie>(res);
            map.Posters.AddRange(map.GetPosters(_client));
            return map;
        }
        public async Task<List<MetaImage>> GetMovieImagesAsync(int id, CancellationToken cancellationToken = default)
        {
            ImagesWithId res = await _client.GetMovieImagesAsync(id, cancellationToken: cancellationToken);
            return res.GetAllImages(_client);
        }
        public async Task<List<MetaVideo>> GetMovieVideosAsync(int id, CancellationToken cancellationToken = default)
        {
            var res = await _client.GetMovieVideosAsync(id, cancellationToken: cancellationToken);
            return res.GetVideos();
        }

        public async Task<TmdbShow> GetShowAsync(int id, CancellationToken cancellationToken = default)
        {
            TvShow res = await _client.GetTvShowAsync(id, TvShowMethods.ExternalIds, cancellationToken: cancellationToken);
            var map = _mapper.Map<TmdbShow>(res);
            map.Posters.AddRange(map.GetPosters(_client));
            return map;
        }
        public async Task<List<MetaImage>> GetShowImagesAsync(int id, CancellationToken cancellationToken = default)
        {
            ImagesWithId res = await _client.GetTvShowImagesAsync(id, cancellationToken: cancellationToken);
            return res.GetAllImages(_client);
        }
        public async Task<List<MetaVideo>> GetShowVideosAsync(int id, CancellationToken cancellationToken = default)
        {
            var res = await _client.GetTvShowVideosAsync(id, cancellationToken: cancellationToken);
            return res.GetVideos();
        }
        public async Task<TmdbSeason> GetSeasonAsync(int showId, int season, CancellationToken cancellationToken = default)
        {
            TvSeason res = await _client.GetTvSeasonAsync(showId, season, cancellationToken: cancellationToken);
            var map = _mapper.Map<TmdbSeason>(res);
            map.Posters.AddRange(map.GetPosters(_client));
            return map;
        }
        public async Task<List<MetaImage>> GetSeasonImagesAsync(int showId, int season, CancellationToken cancellationToken = default)
        {
            PosterImages res = await _client.GetTvSeasonImagesAsync(showId,season, cancellationToken: cancellationToken);
            return res.GetPosters(_client);
        }
        public async Task<List<MetaImage>> GetEpisodeImagesAsync(int showId, int season, int episode, CancellationToken cancellationToken = default)
        {
            StillImages res = await _client.GetTvEpisodeImagesAsync(showId, season,episode, cancellationToken: cancellationToken);
            return res.GetStills(_client);
        }

        public async Task<Entities.Credits> GetCreditsAsync(int id, MetaType type, CancellationToken cancellationToken = default)
        {
            Entities.Credits credits = new Entities.Credits();
            if (type == MetaType.Movie)
            {
                var res = await _client.GetMovieCreditsAsync(id, cancellationToken);
                credits.Crew.AddRange(GetCrew(res.Crew));
                foreach (var c in res.Cast)
                {
                    if (c.ProfilePath.IsValid())
                    {
                        var cast = _mapper.Map<Entities.Cast>(c);
                        cast = cast.AddProfileImages(c.ProfilePath, _client);
                        credits.Cast.Add(cast);
                    }
                }
            }
            else if (type == MetaType.TvShow)
            {
                var res = await _client.GetTvShowCreditsAsync(id, null, cancellationToken);
                credits.Crew.AddRange(GetCrew(res.Crew));
                foreach (var c in res.Cast)
                {
                    if (c.ProfilePath.IsValid())
                    {
                        var cast = _mapper.Map<Entities.Cast>(c);
                        cast = cast.AddProfileImages(c.ProfilePath, _client);
                        credits.Cast.Add(cast);
                    }
                }
            }
            return credits;
        }
        private IEnumerable<Entities.FilmMaker> GetCrew(List<Crew> crews)
        {
            foreach (var c in crews)
            {
                var filmMaker = _mapper.Map<Entities.FilmMaker>(c);
                filmMaker.Profession = c.Department.ParseToProfession();
                if (filmMaker.Profession != Profession.Other)
                {
                    filmMaker = filmMaker.AddProfileImages(c.ProfilePath, _client);
                    yield return filmMaker;
                }
            }
        }

        public async Task<List<Entities.ReviewComment>> GetReviewsAsync(string metaId, MetaType type, CancellationToken cancellationToken = default)
        {
            int? id = metaId.ParseToInt();
            if (id.HasValue)
            {
                if (type == MetaType.Movie)
                {
                    var res = await _client.GetMovieReviewsAsync(id.Value, cancellationToken: cancellationToken);
                    return _mapper.Map<List<Entities.ReviewComment>>(res.GetList());
                }
                else if (type == MetaType.TvShow)
                {
                    var res = await _client.GetTvShowReviewsAsync(id.Value, cancellationToken: cancellationToken);
                    return _mapper.Map<List<Entities.ReviewComment>>(res.GetList());
                }
            }
            return null;
        }

        public async Task<Entities.Company> GetCompanyAsync(int id, CancellationToken cancellationToken = default)
        {
            var res = await _client.GetCompanyAsync(id, cancellationToken: cancellationToken);
            var comp = _mapper.Map<Entities.Company>(res);
            comp.Urls.AddToThis(res.Homepage.ParseToUri().ParseToMetaUrl(MetaSource.TMDB));
            comp.Images.AddRange(_client.ExtractImages(res, MetaImageType.Logo, x => x.LogoPath, x => x.Config.Images.LogoSizes));
            return comp;
        }
        public async Task<Entities.Company> GetNetworkAsync(int id, CancellationToken cancellationToken = default)
        {
            var res = await _client.GetNetworkAsync(id, cancellationToken: cancellationToken);
            var imgs = await _client.GetNetworkImagesAsync(id);
            var comp = _mapper.Map<Entities.Company>(res);
            comp.Service = CompanyService.Network;
            comp.Urls.AddToThis(res.Homepage.ParseToUri().ParseToMetaUrl(MetaSource.TMDB));
            comp.Images.AddRange(imgs.Logos.AsMetaImages(MetaImageType.Logo, _client));
            return comp;
        }

        #region Search & Lookup
        internal async Task<List<SearchCompany>> SearchCompanyAsync(string q, CancellationToken cancellationToken = default)
        {
            var res = await _client.SearchCompanyAsync(q, cancellationToken: cancellationToken);
            return res.GetList();
        }
        public async Task<List<TmdbSearch>> SearchShowAsync(string q, int year = 0, bool nsfw = false, CancellationToken cancellationToken = default)
        {
            var res = await _client.SearchTvShowAsync(q, firstAirDateYear: year, includeAdult: nsfw, cancellationToken: cancellationToken);
            var map = _mapper.Map<List<TmdbSearch>>(res.GetList());
            map.ForEach(x => x.Posters.AddRange(x.GetPosters(_client)));
            return map;
        }
        public async Task<List<TmdbSearch>> SimilarShowsAsync(int id, CancellationToken cancellationToken = default)
        {
            var res = await _client.GetTvShowSimilarAsync(id, cancellationToken: cancellationToken);
            var map = _mapper.Map<List<TmdbSearch>>(res.GetList());
            map.ForEach(x => x.Posters.AddRange(x.GetPosters(_client)));
            return map;
        }
        public async Task<List<TmdbSearch>> SearchMovieAsync(string q, int year = 0, bool nsfw = false, CancellationToken cancellationToken = default)
        {
            var res = await _client.SearchMovieAsync(q, year: year, includeAdult: nsfw, cancellationToken: cancellationToken);
            var map = _mapper.Map<List<TmdbSearch>>(res.GetList());
            map.ForEach(x => x.Posters.AddRange(x.GetPosters(_client)));
            return map;
        }
        public async Task<List<TmdbSearch>> SimilarMoviesAsync(int id, CancellationToken cancellationToken = default)
        {
            var res = await _client.GetMovieSimilarAsync(id, cancellationToken: cancellationToken);
            var map = _mapper.Map<List<TmdbSearch>>(res.GetList());
            map.ForEach(x => x.Posters.AddRange(x.GetPosters(_client)));
            return map;
        }

        internal async Task<List<SearchBase>> SearchAsync(string q, int year = 0, bool nsfw = false, CancellationToken cancellationToken = default)
        {
            var res = await _client.SearchMultiAsync(q, year: year, includeAdult: nsfw, cancellationToken: cancellationToken);
            return res.GetList();
        }
        #endregion

        #region Private Section
        public async Task<TMDbConfig> ConfigureAsync()
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