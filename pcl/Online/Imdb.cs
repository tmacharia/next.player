using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Next.PCL.Entities;
using Next.PCL.Html;
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
        public async Task<List<ImdbSuggestion>> GetSuggestionsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            var list = await _parser.ParseSuggestionsAsync(GenerateUrl(imdbId), cancellationToken);
            return list.ToList();
        }

        public async Task<List<ImdbReview>> GetReviewsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(GenerateUrl(imdbId, "reviews"), cancellationToken);
            return _parser.ParseReviews(html).ToList();
        }
        public async Task<List<GeographicLocation>> GetLocationsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            string html = await GetAsync(GenerateUrl(imdbId, "locations"), cancellationToken);
            return _parser.ParseFilmingLocations(html)
                          .OrderBy(x => x.Name)
                          .ThenBy(x => x.Inner.Count)
                          .ToList();
        }

        internal Uri GenerateUrl(string imdbId, string suffix = default)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}/title/{1}", SiteUrls.IMDB, imdbId);
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