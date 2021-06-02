using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using HtmlAgilityPack;
using Next.PCL.Extensions;
using Next.PCL.Online.Models.Imdb;

namespace Next.PCL.Html
{
    public class ImdbParser : BaseParser
    {
        public async Task<List<ImdbReview>> GetReviewsAsync(string imdbId, CancellationToken token = default)
        {
            var reviews = new List<ImdbReview>();
            if (!imdbId.IsValid())
                return reviews;

            string url = string.Format("{0}/title/{1}/reviews", SiteUrls.IMDB, imdbId);
            string html = await GetAsync(new Uri(url), token);

            var doc = ConvertToHtmlDoc(html);
            var nodes = doc.FindAll("//div[@class='lister-item-content']");
            if (nodes.IsNotEmpty())
            {
                foreach (var node in nodes)
                {
                    var rv = ParseImdbReview(node);
                    if (rv != null)
                    {
                        reviews.Add(rv);
                    }
                }
            }
            return reviews;
        }
        internal ImdbReview ParseImdbReview(HtmlNode node)
        {
            var doc = ConvertToHtmlDoc(node.OuterHtml);

            var titl = doc.Find("//a[@class='title']");
            var date = doc.Find("//span[@class='review-date']");
            var revw = doc.Find("//div[@class='text show-more__control']");
            var spns = doc.FindAll("//span[@class='rating-other-user-rating']/span");

            var imdb = new ImdbReview
            {
                Title = titl.ParseText(),
                Review = revw.ParseText(),
                Timestamp = date.ParseDateTime()
            };
            if (spns.IsNotEmpty())
            {
                var a = spns.FirstOrDefault(x => !x.HasAttributes);
                var d = a.ParseDouble();
                if (d.HasValue)
                    imdb.Score = d.Value;
            }
            return imdb;
        }
    }
}