﻿using System;
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
        private readonly INaiveCache _appCache;

        public Imdb(IHttpOnlineClient httpOnlineClient, INaiveCache lazyCache = default)
            :base(httpOnlineClient)
        {
            _parser = new ImdbParser(httpOnlineClient);
            _appCache = lazyCache ?? new NaiveMemoryCache();
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

            string listsWithTitleKey = string.Format("userlists-containing-{0}", imdbId);

            Func<Task<List<ImdbUserList>>> factory = async () =>
            {
                var ulists = await _parser.ParseUserListsAsync(url, cancellationToken);
                return ulists.OrderBy(x => x.TitlesCount)
                            .ThenByDescending(x => x.Name.Length)
                            .ToList();
            };

            return await _appCache.GetOrAddAsync(listsWithTitleKey, factory);
        }
        public async Task<List<ImdbSuggestion>> GetSuggestionsAsync(string imdbId, int page = 1, CancellationToken cancellationToken = default)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            List<ImdbUserList> ulists = await GetUserListsWithAsync(imdbId, cancellationToken);

            ImdbUserList selected = ulists[page - 1];

            string userListKey = string.Format("userlist-{0}", selected.ListId);

            Func<Task<List<ImdbSuggestion>>> factory = async () =>
            {
                string html = await GetAsync(GenerateUrl(selected.ListId, null, "list"), cancellationToken);

                var list = _parser.ParseSuggestions2(html);

                return list.Where(x => !x.ImdbId.EqualsOIC(imdbId))
                    .OrderByDescending(x => x.Score)
                    .ToList();
            };

            return await _appCache.GetOrAddAsync(userListKey, factory);
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