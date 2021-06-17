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


        internal async Task<List<ImdbReview>> GetAndParseReviewsAsync(string imdbId, CancellationToken token = default)
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
        private ImdbReview ParseImdbReview(HtmlNode node)
        {
            var link = node.ExtendFind("a[@class='title']");
            var display = node.ExtendFind("div[@class='display-name-date']");
            var user = display.ExtendFind("span/a");
            var content = node.ExtendFind("div[@class='content']");

            var review = new ImdbReview
            {
                Title = link.ParseText(),
                Url = link.GetHref().ParseToUri(),
                Reviewer = user.ParseText(),
                ReviewerUrl = user.GetHref().ParseToUri(),
                Review = content.Element("div").ParseText(),
                Timestamp = display.ExtendFind("span[@class='review-date']").ParseDateTime(),
                Score = node.ExtendFind("div/span[@class='rating-other-user-rating']/span")?.ParseDouble()
            };

            string stats = content.ExtendFindAll("div")?.FirstContainingClass("actions")?.FirstChild?.ParseText();
            if (stats.IsValid())
            {
                stats = stats.Replace("out of", "");
                stats = stats.Replace("found this helpful", "");
                stats = stats.TrimEnd('.').Trim();

                var tks = stats.SplitByAndTrim(" ").ToArray();
                if(tks.Length == 2)
                {
                    review.MarkedAsUseful = tks[0].ParseToInt();
                    review.TotalEngagement = tks[1].ParseToInt();
                }
            }

            return review;
        }
    }
}