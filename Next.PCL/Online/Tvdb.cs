using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Next.PCL.Extensions;
using Next.PCL.Html;
using Next.PCL.Online.Models.Tvdb;

namespace Next.PCL.Online
{
    public class Tvdb : BaseOnline
    {
        private readonly TvDbParser _parser;

        public Tvdb()
        {
            _parser = new TvDbParser();
        }

        public Task<TvdbEpisode> GetEpisodeAsync(string tvSlugName, int episodeID, CancellationToken token = default)
        {
            var url = string.Format("{0}/series/{1}/episodes/{2}", SiteUrls.TVDB, tvSlugName, episodeID)
                            .ParseToUri();

            return GetEpisodeAsync(url, token);
        }
        public async Task<TvdbEpisode> GetEpisodeAsync(string tvSlugName, int season, int episode, CancellationToken token = default)
        {
            var url = string.Format("{0}/series/{1}/seasons/official/{2}", SiteUrls.TVDB, tvSlugName, season)
                            .ParseToUri();
            string html = await GetAsync(url, token);

            var eps = _parser.ParseSeasonEpisodes(html);

            return eps.FirstOrDefault(x => x.Number == episode);
        }
        public async Task<TvdbEpisode> GetEpisodeAsync(Uri episodeUrl, CancellationToken token = default)
        {
            string html = await GetAsync(episodeUrl, token);
            var ep = _parser.ParseEpisode(html, episodeUrl);
            return ep;
        }
    }
}