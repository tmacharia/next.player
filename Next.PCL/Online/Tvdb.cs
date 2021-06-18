using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Next.PCL.AutoMap;
using Next.PCL.Configurations;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Extensions;
using Next.PCL.Html;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Tvdb;

namespace Next.PCL.Online
{
    public class Tvdb : BaseOnline
    {
        private readonly IMapper _mapper;
        private readonly TvDbParser _parser;

        internal TvdbConfig Config
        {
            get { return _parser.Config; }
            private set { _parser.Config = value; }
        }

        public Tvdb()
        {
            _parser = new TvDbParser();
            _mapper = new Mapper(AutomapperConfig.Configure());
        }
        public Tvdb(TvdbConfig tvdbConfig, IMapper autoMapper)
        {
            _mapper = autoMapper;
            _parser = new TvDbParser(tvdbConfig);
        }

        public async Task<TvDbShow> GetShowAsync(string tvSlugName, CancellationToken token = default)
        {
            var url = string.Format("{0}/series/{1}", SiteUrls.TVDB, tvSlugName).ParseToUri();
            string html = await GetAsync(url, token);
            return _parser.ParseShow(html, url);
        }
        public async Task<TvDbMovie> GetMovieAsync(string movieSlugName, CancellationToken token = default)
        {
            var url = string.Format("{0}/movies/{1}", SiteUrls.TVDB, movieSlugName).ParseToUri();
            string html = await GetAsync(url, token);
            return _parser.ParseMovie(html, url);
        }

        public async Task<IEnumerable<TvdbCrew>> GetCrewAsync(Uri uri, CancellationToken token = default)
        {
            var url = string.Format("{0}/people", uri.OriginalString.TrimEnd('/')).ParseToUri();
            string html = await GetAsync(url, token);
            return _parser.ParseCrew(html);
        }
        public async Task<IEnumerable<TvdbPerson>> GetCastAsync(Uri uri, CancellationToken token = default)
        {
            var url = string.Format("{0}/people", uri.OriginalString.TrimEnd('/')).ParseToUri();
            string html = await GetAsync(url, token);
            return _parser.ParseCast(html);
        }
        public async Task<IEnumerable<TvdbPerson>> GetCastAndCrewAsync(Uri uri, CancellationToken token = default)
        {
            var url = string.Format("{0}/people", uri.OriginalString.TrimEnd('/')).ParseToUri();
            string html = await GetAsync(url, token);
            var doc = _parser.ConvertToHtmlDoc(html);

            var cast = _parser.ParseCast(null, doc);
            var crew = _parser.ParseCrew(null, doc);

            return cast.Concat(crew);
        }

        public Task<Company> GetCompanyAsync(string companySlugName, CancellationToken token = default)
        {
            var url = string.Format("{0}/companies/{1}", SiteUrls.TVDB, companySlugName).ParseToUri();
            return GetCompanyAsync(url, token);
        }
        public async Task<Company> GetCompanyAsync(Uri companyUrl, CancellationToken token = default)
        {
            string html = await GetAsync(companyUrl, token);
            return _parser.ParseCompany(html, companyUrl);
        }
        public async Task<IEnumerable<TinyTvdbModel>> GetMoviesByCompanyAsync(Uri companyUrl, CancellationToken token = default)
        {
            string html = await GetAsync(companyUrl, token);
            return _parser.ParseCompanyMedias(html, MetaType.Movie);
        }
        public async Task<IEnumerable<TinyTvdbModel>> GetShowsByCompanyAsync(Uri companyUrl, CancellationToken token = default)
        {
            string html = await GetAsync(companyUrl, token);
            return _parser.ParseCompanyMedias(html, MetaType.TvShow);
        }
        public Task<IEnumerable<TinyTvdbModel>> GetShowsByCompanyAsync(string companySlugName, CancellationToken token = default)
        {
            var url = string.Format("{0}/companies/{1}", SiteUrls.TVDB, companySlugName).ParseToUri();
            return GetShowsByCompanyAsync(url, token);
        }
        public Task<IEnumerable<TinyTvdbModel>> GetMoviesByCompanyAsync(string companySlugName, CancellationToken token = default)
        {
            var url = string.Format("{0}/companies/{1}", SiteUrls.TVDB, companySlugName).ParseToUri();
            return GetMoviesByCompanyAsync(url, token);
        }


        #region Images Section
        public async Task<List<MetaImage>> GetArtworksAsync(Uri uri, CancellationToken token = default)
        {
            var images = new List<MetaImage>();

            images.AddRange(await GetAllPostersAsync(uri, token));
            if (token.IsCancellationRequested)
                return images;
            images.AddRange(await GetAllBackdropsAsync(uri, token));
            if (token.IsCancellationRequested)
                return images;
            images.AddRange(await GetAllIconsAsync(uri, token));
            if (token.IsCancellationRequested)
                return images;
            images.AddRange(await GetAllBannersAsync(uri, token));
            
            return images;
        }
        public Task<List<MetaImage>> GetAllIconsAsync(Uri uri, CancellationToken token = default)
        {
            return _parser.GetAndParseImagesAsync(uri, MetaImageType.Icon, token);
        }
        public Task<List<MetaImage>> GetAllBannersAsync(Uri uri, CancellationToken token = default)
        {
            return _parser.GetAndParseImagesAsync(uri, MetaImageType.Banner, token);
        }
        public Task<List<MetaImage>> GetAllPostersAsync(Uri uri, CancellationToken token = default)
        {
            return _parser.GetAndParseImagesAsync(uri, MetaImageType.Poster, token);
        }
        public Task<List<MetaImage>> GetAllBackdropsAsync(Uri uri, CancellationToken token = default)
        {
            return _parser.GetAndParseImagesAsync(uri, MetaImageType.Backdrop, token);
        }
        #endregion

        public Task<TvdbSeason> GetSeasonAsync(string tvSlugName, int season, CancellationToken token = default)
        {
            var url = string.Format("{0}/series/{1}/seasons/official/{2}", SiteUrls.TVDB, tvSlugName, season)
                            .ParseToUri();
            return GetSeasonAsync(url, token);
        }
        public async Task<TvdbSeason> GetSeasonAsync(Uri seasonUrl, CancellationToken token = default)
        {
            string html = await GetAsync(seasonUrl, token);
            return _parser.ParseSeason(html, seasonUrl);
        }

        public Task<TvdbEpisode> GetEpisodeAsync(string tvSlugName, int episodeID, CancellationToken token = default)
        {
            var url = string.Format("{0}/series/{1}/episodes/{2}", SiteUrls.TVDB, tvSlugName, episodeID)
                            .ParseToUri();
            return GetEpisodeAsync(url, token);
        }
        public async Task<TvdbEpisode> GetEpisodeAsync(string tvSlugName, int season, int episode, bool fullGet = false, CancellationToken token = default)
        {
            var url = string.Format("{0}/series/{1}/seasons/official/{2}", SiteUrls.TVDB, tvSlugName, season)
                            .ParseToUri();
            string html = await GetAsync(url, token);

            var ep = _parser.ParseSeasonEpisodes(html)
                          .FirstOrDefault(x => x.Number == episode);

            if (fullGet)
                return await GetEpisodeAsync(ep.Url, token);

            return ep;
        }
        public async Task<TvdbEpisode> GetEpisodeAsync(Uri episodeUrl, CancellationToken token = default)
        {
            string html = await GetAsync(episodeUrl, token);
            return _parser.ParseEpisode(html, episodeUrl);
        }
    }
}