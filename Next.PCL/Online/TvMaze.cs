using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Next.PCL.Enums;
using Next.PCL.Exceptions;
using Next.PCL.Online.Models;

namespace Next.PCL.Online
{
    /// <summary>
    /// Free, fast and clean REST client for TvMaze API that's easy to use.<br/>
    /// API calls are rate limited to allow at least 20 calls every 10 seconds per IP address.
    /// </summary>
    /// <remarks>
    /// Use of the TVmaze API is licensed by 
    /// <a href="https://creativecommons.org/licenses/by-sa/4.0/">CC BY-SA</a>. 
    /// This means the data can freely be used for any purpose, 
    /// as long as TVmaze is properly credited as source and your application complies with the ShareAlike provision.
    /// You can satisfy the attribution requirement by linking back to TVmaze from within your application or website,
    /// for example using the URLs available in the API.
    /// </remarks>
    public class TvMaze : BaseOnline
    {
        public TvMaze(IHttpOnlineClient httpOnlineClient)
            :base(httpOnlineClient)
        { }

        public Task<TvMazeModel> GetShowByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return RequestAsync<TvMazeModel>($"/shows/{id}", cancellationToken);
        }
        public Task<List<TvMazeSeason>> GetSeasonsAsync(int showId, CancellationToken cancellationToken = default)
        {
            return RequestAsync<List<TvMazeSeason>>($"/shows/{showId}/seasons", cancellationToken);
        }
        public Task<List<TvMazeEpisode>> GetEpisodesAsync(int seasonId, CancellationToken cancellationToken = default)
        {
            return RequestAsync<List<TvMazeEpisode>>($"/seasons/{seasonId}/episodes", cancellationToken);
        }
        public Task<TvMazeEpisode> GetEpisodeAsync(int showId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
        {
            string route = string.Format("/shows/{0}/episodebynumber?season={1}&number={2}", showId, seasonNumber, episodeNumber);
            return RequestAsync<TvMazeEpisode>(route, cancellationToken);
        }


        public Task<List<TvMazeImage>> GetShowImagesAsync(int showId, CancellationToken cancellationToken = default)
        {
            return RequestAsync<List<TvMazeImage>>($"/shows/{showId}/images", cancellationToken);
        }


        public Task<List<TvMazeCast>> GetCastAsync(int showId, CancellationToken cancellationToken = default)
        {
            return RequestAsync<List<TvMazeCast>>($"/shows/{showId}/cast", cancellationToken);
        }
        public Task<List<TvMazeCrew>> GetCrewAsync(int showId, CancellationToken cancellationToken = default)
        {
            return RequestAsync<List<TvMazeCrew>>($"/shows/{showId}/crew", cancellationToken);
        }


        #region Search & Lookup
        public Task<TvMazeModel> SingleSearchAsync(string query, CancellationToken cancellationToken = default)
        {
            return RequestAsync<TvMazeModel>($"/singlesearch/shows?q={query}", cancellationToken);
        }
        public Task<List<TvMazeSearchModel>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            return RequestAsync<List<TvMazeSearchModel>>($"/search/shows?q={query}", cancellationToken);
        }
        public Task<TvMazeModel> LookupAsync(string imdbId = default, int tvdbId = default, CancellationToken cancellationToken = default)
        {
            string route = "/lookup/shows?";
            if (imdbId.IsValid())
                route += "imdb=" + imdbId;
            else if (tvdbId > 0)
                route += "thetvdb=" + tvdbId;
            else
                throw new NextArgumentException($"One param is required. Either {nameof(imdbId)} or {nameof(tvdbId)}");

            try
            {
                return RequestAsync<TvMazeModel>(route, cancellationToken);
            }
            catch (Exception ex)
            {
                if(ex is OnlineException oe && oe.StatusCode == 404)
                    return null;
                throw;
            }
        }
        #endregion


        internal async Task<TResponse> RequestAsync<TResponse>(string route, CancellationToken cancellationToken = default)
            where TResponse : class
        {
            if (!route.StartsWith("/"))
                route = $"/{route}";

            Uri url = new Uri(string.Format("{0}{1}", SiteUrls.TVMAZE, route));
            try
            {
                string json = await GetAsync(url, cancellationToken);
                if (json.IsValid())
                    return json.DeserializeTo<TResponse>();
            }
            catch (Exception ex)
            {
                if (ex is OnlineException oe && oe.StatusCode == 429)
                {
                    throw new ApiRateLimitException(MetaSource.TVMAZE, "API rate limit reached of 20 calls every 10 seconds per IP address.", oe);
                }
                else
                {
                    throw ex;
                }
            }
            return null;
        }
    }
}