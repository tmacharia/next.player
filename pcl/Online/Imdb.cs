using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            var uri = new Uri(string.Format("{0}/title/{1}", SiteUrls.IMDB, imdbId));
            string html = await GetAsync(uri, cancellationToken);
            return _parser.ParseImdb(html);
        }
        public async Task<List<ImdbSuggestion>> GetSuggestionsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(string.Format("{0}/title/{1}", SiteUrls.IMDB, imdbId));
            string html = await GetAsync(uri, cancellationToken);
            return _parser.ParseSuggestions(html).ToList();
        }

        public async Task<List<ImdbReview>> GetReviewsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(string.Format("{0}/title/{1}/reviews", SiteUrls.IMDB, imdbId));
            string html = await GetAsync(uri, cancellationToken);
            return _parser.ParseReviews(html).ToList();
        }
        public async Task<List<GeographicLocation>> GetLocationsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(string.Format("{0}/title/{1}/locations", SiteUrls.IMDB, imdbId));
            string html = await GetAsync(uri, cancellationToken);
            return _parser.ParseFilmingLocations(html)
                          .OrderBy(x => x.Name)
                          .ThenBy(x => x.Inner.Count)
                          .ToList();
        }
    }
}