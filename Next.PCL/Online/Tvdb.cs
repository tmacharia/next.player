using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Next.PCL.Enums;
using Next.PCL.Extensions;
using Next.PCL.Html;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Tvdb;

namespace Next.PCL.Online
{
    public class Tvdb : BaseOnline
    {
        private readonly TvDbParser _parser;

        public Tvdb()
        {
            _parser = new TvDbParser();
        }

        public async Task<TvdbModel> GetShowAsync(string tvSlugName, CancellationToken token = default)
        {
            var url = string.Format("{0}/series/{1}", SiteUrls.TVDB, tvSlugName).ParseToUri();
            string html = await GetAsync(url, token);
            return _parser.ParseShow(html, url);
        }
        public async Task<List<string>> GetCastAndCrewAsync(Uri uri, CancellationToken token = default)
        {
            var url = string.Format("{0}/people", uri.OriginalString.TrimEnd('/')).ParseToUri();
            string html = await GetAsync(url, token);
            return _parser.ParseCrew(html);
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