using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Extensions;
using Next.PCL.Html;
using Next.PCL.Infra;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Imdb;
using Next.PCL.Services;

namespace Next.PCL.Online
{
    public class Imdb : BaseOnline, IMetaReviewsProvider
    {
        private readonly ImdbParser _parser;
        protected readonly INaiveCache _appCache;

        public MetaSource Source => MetaSource.IMDB;

        public Imdb(IHttpOnlineClient httpOnlineClient, INaiveCache lazyCache)
            :base(httpOnlineClient)
        {
            _appCache = lazyCache;
            _parser = new ImdbParser(httpOnlineClient);
        }

        public async Task<ImdbModel> GetImdbAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(GenerateUrl(imdbId), cancellationToken);
            return _parser.ParseImdb(html);
        }
        public async Task<List<ImdbEpisode>> GetEpisodesAsync(string imdbId, int season, CancellationToken cancellationToken = default)
        {
            string suffix = $"/episodes?season={season}";
            var url = GenerateUrl(imdbId, suffix);
            string html = await GetAsync(url, cancellationToken);
            return _parser.ParseSeasonEpisodes(html, season).ToList();
        }
        public async Task<ImdbEpisode> GetEpisodeAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(GenerateUrl(imdbId), cancellationToken);
            return _parser.ParseEpisode(html);
        }
        
        public async Task<List<ImdbImage>> GetImageGalleryAsync(string imdbId, uint max = 5, CancellationToken cancellationToken = default)
        {
            max = max <= 5 ? max : 5;
            string html = await GetAsync(GenerateUrl(imdbId, "mediaindex"), cancellationToken);
            return _parser.ParseImageGallery(html).Take((int)max).ToList();
        }
        public async Task<List<MetaImage>> GetImageSetsAsync(string imdbId, string[] imageIds, CancellationToken cancellationToken = default)
        {
            var list = new List<MetaImage>();
            foreach (var id in imageIds)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var images = await GetImageSetAsync(imdbId, id, cancellationToken);
                if (images.IsNotNullOrEmpty())
                    list.AddRange(images);
            }
            return list;
        }
        public async Task<List<MetaImage>> GetImageSetAsync(string imdbId, string imageId, CancellationToken cancellationToken = default)
        {
            var url = GenerateUrl(imdbId, $"mediaviewer/{imageId}");
            Console.WriteLine(url);
            string html = "";
            //var doc = await _parser.GetHtmlDocumentAsync(url, cancellationToken, true);
            html = await GetAsync(url, cancellationToken);
            return _parser.ParseImdbImages(html).ToList();
        }
        public async Task<List<ImdbVideo>> GetVideoGalleryAsync(string imdbId, uint max = 5, CancellationToken cancellationToken = default)
        {
            max = max <= 5 ? max : 5;
            string html = await GetAsync(GenerateUrl(imdbId, "videogallery"), cancellationToken);
            return _parser.ParseVideoGallery(html).Take((int)max).ToList();
        }


        public async Task<List<ReviewComment>> GetReviewsAsync(string imdbId, MetaType metaType = MetaType.TvShow, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(GenerateUrl(imdbId, "reviews"), cancellationToken);
            return _parser.ParseReviews(html).Cast<ReviewComment>().ToList();
        }
        public async Task<List<ImdbUserList>> GetUserListsWithAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            Uri url = GenerateUrl(imdbId, null, "lists");

            string cacheKey = string.Format("userlists-containing-{0}", imdbId);

            Func<Task<List<ImdbUserList>>> factory = async () =>
            {
                var ulists = await _parser.ParseUserListsAsync(url, cancellationToken);
                return ulists.OrderBy(x => x.TitlesCount)
                            .ThenByDescending(x => x.Name.Length)
                            .ToList();
            };

            return await _appCache.GetOrAddAsync(cacheKey, factory);
        }
        public async Task<List<ImdbSuggestion>> GetSuggestionsAsync(string imdbId, int page = 1, CancellationToken cancellationToken = default)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            List<ImdbUserList> ulists = await GetUserListsWithAsync(imdbId, cancellationToken);

            ImdbUserList selected = ulists[page - 1];

            string cacheKey = string.Format("userlist-{0}", selected.ListId);

            Func<Task<List<ImdbSuggestion>>> factory = async () =>
            {
                string html = await GetAsync(GenerateUrl(selected.ListId, null, "list"), cancellationToken);

                var list = _parser.ParseSuggestions2(html);

                return list.Where(x => !x.ImdbId.EqualsOIC(imdbId))
                    .OrderByDescending(x => x.Score)
                    .ToList();
            };

            return await _appCache.GetOrAddAsync(cacheKey, factory);
        }
        
        public async Task<List<GeographicLocation>> GetLocationsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(GenerateUrl(imdbId, "locations"), cancellationToken);
            return _parser.ParseFilmingLocations(html)
                          .OrderBy(x => x.Name)
                          .ThenBy(x => x.Inner.Count)
                          .ToList();
        }

        internal Uri GenerateUrl(string imdbId, string suffix = default, string prefix = "title")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}/{1}/{2}", SiteUrls.IMDB, prefix, imdbId);
            if (suffix.IsValid())
            {
                if (!suffix.StartsWith("/"))
                    sb.Append("/");
                sb.Append(suffix);
            }
            return new Uri(sb.ToString());
        }
    }
}