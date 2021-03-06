using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Next.PCL.AutoMap;
using Next.PCL.Configurations;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Exceptions;
using Next.PCL.Extensions;
using Next.PCL.Html;
using Next.PCL.Infra;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Tvdb;
using Next.PCL.Services;

namespace Next.PCL.Online
{
    public class Tvdb : BaseOnline, IMetaSearchProvider<TvDbShow>, IMetaCompaniesProvider
    {
        private readonly IMapper _mapper;
        private readonly TvDbParser _parser;
        protected readonly INaiveCache _appCache;

        internal TvdbConfig Config
        {
            get { return _parser.Config; }
            private set { _parser.Config = value; }
        }
        public MetaSource Source => MetaSource.TVDB;

        public Tvdb(IHttpOnlineClient httpOnlineClient, INaiveCache lazyCache)
            :base(httpOnlineClient)
        {
            _appCache = lazyCache;
            _mapper = new Mapper(AutomapperConfig.Configure());
            _parser = new TvDbParser(httpOnlineClient, _mapper);
        }
        public Tvdb(IHttpOnlineClient httpOnlineClient, IMapper autoMapper, INaiveCache lazyCache)
            : base(httpOnlineClient)
        {
            _mapper = autoMapper;
            _appCache = lazyCache;
            _parser = new TvDbParser(httpOnlineClient, autoMapper);
        }
        public Tvdb(TvdbConfig tvdbConfig, IHttpOnlineClient httpOnlineClient, IMapper autoMapper, INaiveCache lazyCache)
            :base(httpOnlineClient)
        {
            _mapper = autoMapper;
            _appCache = lazyCache;
            _parser = new TvDbParser(tvdbConfig, httpOnlineClient, autoMapper);
        }

        public Task<TvDbShow> GetShowAsync(string tvSlugName, CancellationToken cancellationToken = default)
        {
            tvSlugName = Sluggify(tvSlugName);
            var url = string.Format("{0}/series/{1}", SiteUrls.TVDB, tvSlugName).ParseToUri();
            return GetShowAsync(url, cancellationToken);
        }
        public async Task<TvDbShow> GetShowAsync(Uri tvShowUrl, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(tvShowUrl, cancellationToken);
            return _parser.ParseShow(html, tvShowUrl);
        }
        public Task<TvDbMovie> GetMovieAsync(string movieSlugName, CancellationToken cancellationToken = default)
        {
            movieSlugName = Sluggify(movieSlugName);
            var url = string.Format("{0}/movies/{1}", SiteUrls.TVDB, movieSlugName).ParseToUri();
            return GetMovieAsync(url, cancellationToken);
        }
        public async Task<TvDbMovie> GetMovieAsync(Uri movieUrl, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(movieUrl, cancellationToken);
            return _parser.ParseMovie(html, movieUrl);
        }

        public async Task<IEnumerable<FilmMaker>> GetCrewAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            string cacheKey = string.Format("tvdb-people-{0}", uri);

            Func<Task<string>> factory = () =>
            {
                var url = string.Format("{0}/people", uri.OriginalString.TrimEnd('/')).ParseToUri();
                return GetAsync(url, cancellationToken);
            };

            string html = await _appCache.GetOrAddAsync(cacheKey, factory);

            return _parser.ParseCrew(html);
        }
        public async Task<IEnumerable<Cast>> GetCastAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            string cacheKey = string.Format("tvdb-people-{0}", uri);

            Func<Task<string>> factory = () =>
            {
                var url = string.Format("{0}/people", uri.OriginalString.TrimEnd('/')).ParseToUri();
                return GetAsync(url, cancellationToken);
            };

            string html = await _appCache.GetOrAddAsync(cacheKey, factory);

            return _parser.ParseCast(html);
        }
        public async Task<IEnumerable<Person>> GetCastAndCrewAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            string cacheKey = string.Format("tvdb-people-{0}", uri);

            Func<Task<string>> factory = () =>
            {
                var url = string.Format("{0}/people", uri.OriginalString.TrimEnd('/')).ParseToUri();
                return GetAsync(url, cancellationToken);
            };

            string html = await _appCache.GetOrAddAsync(cacheKey, factory);

            var doc = _parser.ConvertToHtmlDoc(html);

            var cast = _parser.ParseCast(null, doc);
            var crew = _parser.ParseCrew(null, doc);

            return cast.Concat(crew.Cast<Person>());
        }

        public Task<Company> GetCompanyAsync(string companySlugName, CancellationToken cancellationToken = default)
        {
            companySlugName = Sluggify(companySlugName);

            var url = string.Format("{0}/companies/{1}", SiteUrls.TVDB, companySlugName).ParseToUri();
            return GetCompanyAsync(url, cancellationToken);
        }
        public async Task<Company> GetCompanyAsync(Uri companyUrl, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(companyUrl, cancellationToken);
            return _parser.ParseCompany(html, companyUrl);
        }
        public async Task<IEnumerable<TinyTvdbModel>> GetMoviesByCompanyAsync(Uri companyUrl, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(companyUrl, cancellationToken);
            return _parser.ParseCompanyMedias(html, MetaType.Movie);
        }
        public async Task<IEnumerable<TinyTvdbModel>> GetShowsByCompanyAsync(Uri companyUrl, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(companyUrl, cancellationToken);
            return _parser.ParseCompanyMedias(html, MetaType.TvShow);
        }
        public Task<IEnumerable<TinyTvdbModel>> GetShowsByCompanyAsync(string companySlugName, CancellationToken cancellationToken = default)
        {
            companySlugName = Sluggify(companySlugName);

            var url = string.Format("{0}/companies/{1}", SiteUrls.TVDB, companySlugName).ParseToUri();
            return GetShowsByCompanyAsync(url, cancellationToken);
        }
        public Task<IEnumerable<TinyTvdbModel>> GetMoviesByCompanyAsync(string companySlugName, CancellationToken cancellationToken = default)
        {
            companySlugName = Sluggify(companySlugName);

            var url = string.Format("{0}/companies/{1}", SiteUrls.TVDB, companySlugName).ParseToUri();
            return GetMoviesByCompanyAsync(url, cancellationToken);
        }


        #region Images Section
        public async Task<List<MetaImageNx>> GetArtworksAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            var images = new List<MetaImageNx>();

            images.AddRange(await GetAllPostersAsync(uri, cancellationToken)); //1.
            if (cancellationToken.IsCancellationRequested)
                return images;
            images.AddRange(await GetAllBackdropsAsync(uri, cancellationToken)); //2.
            if (cancellationToken.IsCancellationRequested)
                return images;
            images.AddRange(await GetAllIconsAsync(uri, cancellationToken)); //3.
            if (cancellationToken.IsCancellationRequested)
                return images;
            images.AddRange(await GetAllClearArtsAsync(uri, cancellationToken)); //4.
            if (cancellationToken.IsCancellationRequested)
                return images;
            images.AddRange(await GetAllClearLogosAsync(uri, cancellationToken)); //5.
            if (cancellationToken.IsCancellationRequested)
                return images;
            images.AddRange(await GetAllBannersAsync(uri, cancellationToken)); //6.
            
            return images;
        }
        public Task<List<MetaImageNx>> GetAllIconsAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return _parser.GetAndParseImagesAsync(uri, MetaImageType.Icon, cancellationToken);
        }
        public Task<List<MetaImageNx>> GetAllBannersAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return _parser.GetAndParseImagesAsync(uri, MetaImageType.Banner, cancellationToken);
        }
        public Task<List<MetaImageNx>> GetAllPostersAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return _parser.GetAndParseImagesAsync(uri, MetaImageType.Poster, cancellationToken);
        }
        public Task<List<MetaImageNx>> GetAllBackdropsAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return _parser.GetAndParseImagesAsync(uri, MetaImageType.Backdrop, cancellationToken);
        }
        public Task<List<MetaImageNx>> GetAllClearArtsAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return _parser.GetAndParseImagesAsync(uri, MetaImageType.ClearArt, cancellationToken);
        }
        public Task<List<MetaImageNx>> GetAllClearLogosAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return _parser.GetAndParseImagesAsync(uri, MetaImageType.ClearLogo, cancellationToken);
        }
        #endregion

        public Task<TvdbSeason> GetSeasonAsync(string tvSlugName, int season, CancellationToken cancellationToken = default)
        {
            tvSlugName = Sluggify(tvSlugName);

            var url = string.Format("{0}/series/{1}/seasons/official/{2}", SiteUrls.TVDB, tvSlugName, season)
                            .ParseToUri();
            return GetSeasonAsync(url, cancellationToken);
        }
        public async Task<TvdbSeason> GetSeasonAsync(Uri seasonUrl, CancellationToken cancellationToken = default)
        {
            string cacheKey = string.Format("tvdb-season-{0}", seasonUrl);

            Func<Task<string>> factory = () => GetAsync(seasonUrl, cancellationToken);

            string html = await _appCache.GetOrAddAsync(cacheKey, factory);

            return _parser.ParseSeason(html, seasonUrl);
        }

        public Task<TvdbEpisode> GetEpisodeAsync(string tvSlugName, int episodeID, CancellationToken cancellationToken = default)
        {
            tvSlugName = Sluggify(tvSlugName);

            var url = string.Format("{0}/series/{1}/episodes/{2}", SiteUrls.TVDB, tvSlugName, episodeID)
                            .ParseToUri();
            return GetEpisodeAsync(url, cancellationToken);
        }
        public async Task<TvdbEpisode> GetEpisodeAsync(string tvSlugName, int season, int episode, bool fullGet = false, CancellationToken cancellationToken = default)
        {
            tvSlugName = Sluggify(tvSlugName);

            var seasonUrl = string.Format("{0}/series/{1}/seasons/official/{2}", SiteUrls.TVDB, tvSlugName, season)
                            .ParseToUri();

            string cacheKey = string.Format("tvdb-season-{0}", seasonUrl);

            Func<Task<string>> factory = () => GetAsync(seasonUrl, cancellationToken);

            string html = await _appCache.GetOrAddAsync(cacheKey, factory);

            var ep = _parser.ParseSeasonEpisodes(html)
                          .FirstOrDefault(x => x.Number == episode);

            if (fullGet)
                return await GetEpisodeAsync(ep.Url, cancellationToken);

            return ep;
        }
        public async Task<TvdbEpisode> GetEpisodeAsync(Uri episodeUrl, CancellationToken cancellationToken = default)
        {
            Console.WriteLine(episodeUrl);
            string html = await GetAsync(episodeUrl, cancellationToken);
            return _parser.ParseEpisode(html, episodeUrl);
        }

        public string Sluggify(string name)
        {
            if (name.Contains('-'))
                name = name.Replace('-', ' ');

            name = Regex.Replace(name, "[^A-Za-z0-9 ]", "");

            return name
                .Replace(" ", "-")
                .ToLower();
        }

        public Task<List<TvDbShow>> SearchAsync(string query, MetaType metaType = MetaType.TvShow, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public override Task<string> GetAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            try
            {
                return base.GetAsync(uri, cancellationToken);
            }
            catch (Exception xe)
            {
                if (xe is OnlineException)
                {
                    return Task.FromResult(string.Empty);
                }
                throw;
            }
        }
    }
}