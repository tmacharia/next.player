using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Structs;
using HtmlAgilityPack;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Extensions;
using Next.PCL.Online;
using Next.PCL.Online.Models.Imdb;
using Next.PCL.Static;

namespace Next.PCL.Html
{
    public class ImdbParser : BaseParser
    {
        public ImdbParser(IHttpOnlineClient httpOnlineClient)
            :base(httpOnlineClient)
        { }

        internal async Task<List<string>> GetImageIds(string imdbId, CancellationToken cancellationToken = default)
        {
            var list = new List<string>();
            if (!imdbId.IsValid())
                return list;

            var uri = new Uri(string.Format("{0}/title/{1}/mediaindex", SiteUrls.IMDB, imdbId));
            var doc = await GetHtmlDocumentAsync(uri, cancellationToken);

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


        internal IEnumerable<ImdbSuggestion> ParseSuggestions(string html, HtmlDocument htmlDocument = default)
        {
            var doc = htmlDocument ?? ConvertToHtmlDoc(html);

            var nodes = doc.FindAll("//div[@class='rec_overview']");
            if (nodes.IsNotNullOrEmpty())
            {
                foreach (var node in nodes)
                {
                    var rv = ParseSingleImdbSuggestion(node);
                    if (rv != null)
                        yield return rv;
                }
            }
        }

        internal IEnumerable<ImdbReview> ParseReviews(string html, HtmlDocument htmlDocument = default)
        {
            var doc = htmlDocument ?? ConvertToHtmlDoc(html);

            var nodes = doc.FindAll("//div[@class='lister-item-content']");
            if (nodes.IsNotNullOrEmpty())
            {
                foreach (var node in nodes)
                {
                    var rv = ParseSingleImdbReview(node);
                    if (rv != null)
                        yield return rv;
                }
            }
        }
        internal IEnumerable<GeographicLocation> ParseFilmingLocations(string html, HtmlDocument htmlDocument = default)
        {
            var doc = htmlDocument ?? ConvertToHtmlDoc(html);

            var nodes = doc.GetElementbyId("filming_locations")?.ExtendFindAll("div")?.WhereClassContains("soda").ToList();

            foreach (var node in nodes)
            {
                var rv = ParseSingleFilmingLocation(node);
                if (rv != null)
                    yield return rv;
            }
        }


        internal ImdbModel ParseImdb(string html, HtmlDocument htmlDocument = default)
        {
            var doc = htmlDocument ?? ConvertToHtmlDoc(html);

            var json = doc.Find("//script[@type='application/ld+json']")?.ParseText();
            if (json.IsValid())
            {
                var model = json.DeserializeTo<ImdbModel>();
                model.Url = doc.GetMetaProp("og:url").ParseToUri();
                model.ImdbId = model.Url.AbsolutePath.SplitByAndTrim("/").Last();
                if(model.Trailer != null && model.Trailer.EmbedUrl != null)
                {
                    model.Trailer.EmbedUrl = (SiteUrls.IMDB + model.Trailer.EmbedUrl.OriginalString).ParseToUri();
                }
                var blocks = doc.GetElementbyId("titleDetails")?.ExtendFindAll("div");
                if(blocks != null && blocks.Any())
                {
                    model.OtherSites = GetLinks(blocks, ImdbKeys.OfficialSites)
                                    .Select(x => x.ParseToMetaUrl(MetaSource.IMDB)).ToList();
                    model.ProductionCompanies = GetLinks(blocks, ImdbKeys.ProductionCo)
                                    .Select(x => x.ParseToCompany(MetaSource.IMDB, CompanyService.Production)).ToList();
                    model.ProductionCountries = GetLinks(blocks, ImdbKeys.Country)
                                    .Select(x => x.ParseText())
                                    .Select(x => x.ToGeoLocale(true)).ToList();
                    if(model.Type == MetaType.Movie)
                    {
                        model.Revenue = new MetaRevenue();
                        model.Revenue.Budget = GetAsNumber(blocks, ImdbKeys.Budget);
                        model.Revenue.CumulativeGross = GetAsNumber(blocks, ImdbKeys.WorldGross);
                    }
                    else if(model.Type == MetaType.TvShow)
                    {
                        model.Runtime = doc.Find("//div[@class='title_wrapper']/div/time")?.GetAttrib("datetime").ParseToRuntime();
                        if (!model.Runtime.HasValue)
                            model.Runtime = GetAsText(blocks, ImdbKeys.Runtime, "/time").ParseToRuntime();
                    }
                }
                return model;
            }

            return null;
        }

        private ImdbSuggestion ParseSingleImdbSuggestion(HtmlNode node)
        {
            string imdbId = node.GetAttrib("data-tconst");
            var rec_poster = node.ExtendFind("div[@class='rec_poster']");
            var rec_info = node.ExtendFind("div[@class='rec_details']/div[@class='rec-info']");
            var rec_title = rec_info.ExtendFind("div/div[@class='rec-title']");
            var rec_genres = rec_info.ExtendFind("div/div[@class='rec-cert-genre']");
            var rec_rating = rec_info.ExtendFind("div/div[@class='rec-rating']");

            var suggestion = new ImdbSuggestion();
            suggestion.ImdbId = imdbId;
            suggestion.Name = rec_title.ExtendFind("a").ParseText();
            suggestion.Poster = rec_poster.ExtendFind("/a/img").GetAttrib("src").ParseToUri();
            suggestion.Url = string.Format("{0}/title/{1}", SiteUrls.IMDB, imdbId).ParseToUri();
            suggestion.Plot = rec_rating.ExtendFind("div[@class='rec-outline']/p").ParseText();
            suggestion.Genres = rec_genres.ParseText().Replace('|', ',').SplitByAndTrim(",").ToList();
            suggestion.Score = rec_rating.ExtendFind("div/span[@class='rating-rating']/span").ParseDouble();
            suggestion.ReleaseDate = new DateTime(rec_title.Elements("span").Last().ParseText('(', ')').ParseToInt().Value, 1, 1);


            return suggestion;
        }
        private ImdbReview ParseSingleImdbReview(HtmlNode node)
        {
            var link = node.ExtendFind("a[@class='title']");
            var display = node.ExtendFind("div[@class='display-name-date']");
            var user = display.ExtendFind("span/a");
            var content = node.ExtendFind("div[@class='content']");

            var review = new ImdbReview
            {
                Title = link.ParseText(),
                Url = link.GetHref().ParseToUri(),
                Author = user.ParseText(),
                AuthorUrl = user.GetHref().ParseToUri(),
                Content = content.Element("div").ParseText(),
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
        private GeographicLocation ParseSingleFilmingLocation(HtmlNode node)
        {
            var places = node?.ExtendFind("dt/a")?.ParseText().SplitByAndTrim(",").ToArray();

            if(places.Length <= 0)
                return null;

            var geo = new GeographicLocation
            {
                Name = places.Last(),
                IsCountry = true
            };
            if (places.Length > 1)
            {
                geo.Inner.Add(new GeographicLocation()
                {
                    IsCountry = false,
                    Name = string.Join(",", places.Take(places.Length - 1))
                });
            }
            return geo;
        }

        private double? GetAsNumber(HtmlNodeCollection nodes, string name)
        {
            string s = string.Join(" ", GetNodes(nodes, name, null).Where(x => x.Name != "h4").Select(x => x.ParseText())).Trim();
            s = s.SplitByAndTrim(" ")?.FirstOrDefault();
            if (s.IsValid()) {
                while (true) {
                    if (!char.IsNumber(s[0])) {
                        s = s.Substring(1);
                    } else {
                        break;
                    }
                }
                return s.ParseToDouble();
            }
            return null;
        }
        private string GetAsText(HtmlNodeCollection nodes, string name, string element = "/a")
        {
            return nodes?.FirstOrDefault(x
                            => x.Element("h4")
                                .ParseText()
                                .Matches(name))
                        ?.ExtendFind(element)
                        ?.ParseText();
        }
        private IEnumerable<HtmlNode> GetLinks(HtmlNodeCollection nodes, string name)
        {
            return GetNodes(nodes, name, "a");
        }
        private IEnumerable<HtmlNode> GetNodes(HtmlNodeCollection nodes, string name, string xPath = "/a")
        {
            var elem = nodes?.FirstOrDefault(x => x.Element("h4").ParseText().Matches(name));

            if (elem == null)
            {
                return Array.Empty<HtmlNode>();
            }

            if (xPath.IsValid())
            {
                if (!xPath.StartsWith("/"))
                    xPath = '/' + xPath;
                return elem.ExtendFindAll(xPath);
            }
            return elem.ChildNodes;
        }
    }
}