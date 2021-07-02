using System;
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
        public Imdb(IHttpOnlineClient httpOnlineClient)
            :base(httpOnlineClient)
        {
            _parser = new ImdbParser(httpOnlineClient);
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
            var list = await _parser.ParseUserListsAsync(GenerateUrl(imdbId, "", "lists"), cancellationToken);
            return list.OrderBy(x => x.TitlesCount)
                .ThenByDescending(x => x.Name.Length)
                .ToList();
        }
        public async Task<List<ImdbSuggestion>> GetSuggestionsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            var ulists = await GetUserListsWithAsync(imdbId, cancellationToken);

            int k = Randomizer.Instance.Next(0, ulists.Count);
            ImdbUserList selected = ulists[k];

            string html = await GetAsync(GenerateUrl(selected.ListId, "", "list"), cancellationToken);

            var list = _parser.ParseSuggestions2(html);

            return list.OrderByDescending(x => x.Score).ToList();
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