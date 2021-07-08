using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Next.PCL.Exceptions;
using Next.PCL.Online.Models.Yts;
using Next.PCL.Services;

namespace Next.PCL.Online
{
    public class Yts : BaseOnline, IMetaServiceProvider<YtsMovie>
    {
        public Yts(IHttpOnlineClient httpOnlineClient)
            :base(httpOnlineClient)
        { }

        public async Task<YtsMovie> GetMovieByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var res = await RequestAsync<YtsMovieSingleResponse>($"/movie_details.json?movie_id={id}&with_images=true", cancellationToken);
            return res.Movie;
        }
        public async Task<List<YtsMovie>> GetSuggestionsAsync(int id, CancellationToken cancellationToken = default)
        {
            var res = await RequestAsync<YtsMovieListResponse>($"/movie_suggestions.json?movie_id={id}", cancellationToken);
            return res.Movies;
        }
        public async Task<List<YtsMovie>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            var res = await RequestAsync<YtsMovieListResponse>($"/list_movies.json?query_term={query}", cancellationToken);
            return res.Movies;
        }

        internal async Task<TResponse> RequestAsync<TResponse>(string route, CancellationToken cancellationToken = default) 
            where TResponse : class
        {
            if (!route.StartsWith("/"))
                route = $"/{route}";

            Uri url = new Uri(string.Format("{0}{1}", SiteUrls.YTS, route));

            string json = await GetAsync(url, cancellationToken);
            if (json.IsValid())
            {
                var mx = json.DeserializeTo<BaseYtsResponse<TResponse>>();
                if (!mx.IsSuccess)
                    throw new OnlineException(mx.StatusMessage);
                return mx.Data;
            }
            return null;
        }
    }
}