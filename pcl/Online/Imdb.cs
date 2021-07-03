using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Next.PCL.Entities;
using Next.PCL.Html;
using Next.PCL.Infra;
using Next.PCL.Online.Models.Imdb;

namespace Next.PCL.Online
{
    public class Imdb : BaseOnline
    {
        private readonly ImdbParser _parser;
        private readonly ConcurrentDictionary<string, object> _cache;

        public Imdb(IHttpOnlineClient httpOnlineClient)
            :base(httpOnlineClient)
        {
            _parser = new ImdbParser(httpOnlineClient);
            _cache = new ConcurrentDictionary<string, object>();
        }

        public async Task<ImdbModel> GetImdbAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(GenerateUrl(imdbId), cancellationToken);
            return _parser.ParseImdb(html);
        }
        
        public async Task<List<ImdbReview>> GetReviewsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(GenerateUrl(imdbId, "reviews"), cancellationToken);
            return _parser.ParseReviews(html).ToList();
        }
        public async Task<List<ImdbUserList>> GetUserListsWithAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            Uri url = GenerateUrl(imdbId, null, "lists");
            string key = string.Format("{0}-{1}", imdbId, nameof(ImdbUserList).ToLower());

            if (_cache.ContainsKey(key))
            {
                return (List<ImdbUserList>)_cache[key];
            }
            else
            {
                var ulists = await _parser.ParseUserListsAsync(url, cancellationToken);
                var list = ulists.OrderBy(x => x.TitlesCount)
                            .ThenByDescending(x => x.Name.Length)
                            .ToList();

                _cache.TryAdd(key, list);
                return list;
            }
        }
        public async Task<List<ImdbSuggestion>> GetSuggestionsAsync(string imdbId, int page = 1, CancellationToken cancellationToken = default)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            List<ImdbUserList> ulists;

            string key = string.Format("{0}-{1}", imdbId, nameof(ImdbUserList).ToLower());

            if (_cache.ContainsKey(key))
            {
                ulists = (List<ImdbUserList>)_cache[key];
            }
            else
            {
                ulists = await GetUserListsWithAsync(imdbId, cancellationToken);
                _cache.TryAdd(key, ulists);
            }

            ImdbUserList selected = ulists[page - 1];

            string html = await GetAsync(GenerateUrl(selected.ListId, null, "list"), cancellationToken);

            var list = _parser.ParseSuggestions2(html);

            return list.Where(x => !x.ImdbId.EqualsOIC(imdbId))
                .OrderByDescending(x => x.Score)
                .ToList();
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