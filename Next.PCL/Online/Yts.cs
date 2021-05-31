using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Next.PCL.Exceptions;
using Next.PCL.Online.Models.Yts;

namespace Next.PCL.Online
{
    public class Yts : BaseOnline
    {
        public async Task<YtsMovie> GetMovieByIdAsync(int id, CancellationToken token = default)
        {
            var res = await RequestAsync<YtsMovieSingleResponse>($"/movie_details.json?movie_id={id}&with_images=true", token);
            return res.Movie;
        }
        public async Task<List<YtsMovie>> GetSuggestionsAsync(int id, CancellationToken token = default)
        {
            var res = await RequestAsync<YtsMovieListResponse>($"/movie_suggestions.json?movie_id={id}", token);
            return res.Movies;
        }
        public async Task<List<YtsMovie>> SearchAsync(string query, CancellationToken token = default)
        {
            var res = await RequestAsync<YtsMovieListResponse>($"/list_movies.json?query_term={query}", token);
            return res.Movies;
        }



        internal async Task<TResponse> RequestAsync<TResponse>(string route, CancellationToken token = default) 
            where TResponse : class
        {
            if (!route.StartsWith("/"))
                route = $"/{route}";

            Uri url = new Uri(string.Format("{0}{1}", SiteUrls.YTS, route));

            string json = await GetAsync(url, token);
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