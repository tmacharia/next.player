using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        
        public Task<List<ImdbReview>> GetReviewsAsync(string imdbId, CancellationToken token = default)
        {
            return _parser.GetAndParseReviewsAsync(imdbId, token);
        }
    }
}