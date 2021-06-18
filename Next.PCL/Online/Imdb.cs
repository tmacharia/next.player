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
        public Imdb()
        {
            _parser = new ImdbParser();
        }
        
        public async Task<List<ImdbReview>> GetReviewsAsync(string imdbId, CancellationToken token = default)
        {
            var uri = new Uri(string.Format("{0}/title/{1}/reviews", SiteUrls.IMDB, imdbId));
            string html = await GetAsync(uri, token);
            return _parser.ParseReviews(html).ToList();
        }
        public async Task<List<GeographicLocation>> GetLocationsAsync(string imdbId, CancellationToken token = default)
        {
            var uri = new Uri(string.Format("{0}/title/{1}/locations", SiteUrls.IMDB, imdbId));
            string html = await GetAsync(uri, token);
            return _parser.ParseFilmingLocations(html).ToList();
        }
    }
}