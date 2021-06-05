using System;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<TvdbEpisode> GetEpisodeAsync(Uri episodeUrl, CancellationToken token = default)
        {
            string html = await GetAsync(episodeUrl, token);
            var ep = _parser.ParseEpisode(html, episodeUrl);
            return ep;
        }
    }
}