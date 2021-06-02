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
        internal async Task<List<string>> GetImageIds(string imdbId, CancellationToken token = default)
        {
            var list = new List<string>();
            if (!imdbId.IsValid())
                return list;

            var uri = new Uri(string.Format("{0}/title/{1}/mediaindex", SiteUrls.IMDB, imdbId));
            var doc = await GetHtmlDocumentAsync(uri, token);

            var nodes = doc.FindAll("//div[@id='media_index_thumbnail_grid']/a");
            if (nodes.IsNotNullOrEmpty())
            {
                foreach (var node in nodes)
                {
                    string href = node.GetHref();
                    if (href.IsValid() && href.StartsWith("/title"))
                    {
                        string id = href.SplitByAndTrim("?").First().Split('/').Last();
                        if (id.IsValid())
                            list.Add(id);
                    }
                }
            }
            return list;
        }


        public async Task<List<ImdbReview>> GetReviewsAsync(string imdbId, CancellationToken token = default)
        {
            var reviews = new List<ImdbReview>();
            if (!imdbId.IsValid())
                return reviews;

            var uri = new Uri(string.Format("{0}/title/{1}/reviews", SiteUrls.IMDB, imdbId));
            var doc = await GetHtmlDocumentAsync(uri, token);

            var nodes = doc.FindAll("//div[@class='lister-item-content']");
            if (nodes.IsNotNullOrEmpty())
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
            if (spns.IsNotNullOrEmpty())
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