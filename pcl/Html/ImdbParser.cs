using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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

        internal ImdbModel ParseImdb(string html, HtmlDocument htmlDocument = default)
        {
            var doc = htmlDocument ?? ConvertToHtmlDoc(html);

            var json = doc.Find("//script[@type='application/ld+json']")?.ParseText();
            if (json.IsValid())
            {
                var model = json.DeserializeTo<ImdbModel>();
                model.Url = doc.GetMetaProp("og:url").ParseToUri();
                model.ImdbId = doc.GetMetaProp("imdb:pageConst");
                if (model.Trailer != null && model.Trailer.EmbedUrl != null)
                {
                    model.Trailer.EmbedUrl = (SiteUrls.IMDB + model.Trailer.EmbedUrl.OriginalString).ParseToUri();
                }
                var blocks = doc.GetElementbyId("titleDetails")?.ExtendFindAll("div");
                if (blocks != null && blocks.Any())
                {
                    model.OtherSites = GetLinks(blocks, ImdbKeys.OfficialSites)
                                    .Select(x => x.ParseToMetaUrl(MetaSource.IMDB)).ToList();
                    model.ProductionCompanies = GetLinks(blocks, ImdbKeys.ProductionCo)
                                    .Select(x => x.ParseToCompany(MetaSource.IMDB, CompanyService.Production)).ToList();
                    model.ProductionCountries = GetLinks(blocks, ImdbKeys.Country)
                                    .Select(x => x.ParseText())
                                    .Select(x => x.ToGeoLocale(true)).ToList();
                    if (model.Type == MetaType.Movie)
                    {
                        model.Revenue = new MetaRevenue();
                        model.Revenue.Budget = GetAsNumber(blocks, ImdbKeys.Budget);
                        model.Revenue.CumulativeGross = GetAsNumber(blocks, ImdbKeys.WorldGross);
                    }
                    else if (model.Type == MetaType.TvShow)
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

        internal IEnumerable<ImdbEpisode> ParseSeasonEpisodes(string html, int season, HtmlDocument htmlDocument = default)
        {
            var doc = htmlDocument ?? ConvertToHtmlDoc(html);

            // validate if html is for correct season
            var sn = doc.GetElementbyId("bySeason").ExtendFind("option[@selected]").ParseInt();
            if (!sn.HasValue || sn != season)
                yield break;

            var nodes = doc.DocumentNode.SelectNodes("//div[@class='list detail eplist']/div");
            foreach (var node in nodes)
            {
                var ep = ParseSingleEpisode(node);
                if (ep != null)
                    yield return ep;
            }
        }
        internal ImdbEpisode ParseEpisode(string html, HtmlDocument htmlDocument = default)
        {
            var doc = htmlDocument ?? ConvertToHtmlDoc(html);

            var json = doc.Find("//script[@type='application/ld+json']")?.ParseText();

            if (json.IsValid())
            {
                var model = json.DeserializeTo<ImdbEpisode>();
                model.Url = doc.GetMetaProp("og:url").ParseToUri();
                model.ImdbId = doc.GetMetaProp("imdb:pageConst");
                return model;
            }

            return null;
        }

        internal async Task<IEnumerable<ImdbUserList>> ParseUserListsAsync(Uri imdbUrl, CancellationToken cancellationToken = default)
        {
            var doc = await GetHtmlDocumentAsync(imdbUrl, cancellationToken, false);
            return ParseUserLists(null, doc);
        }
        internal IEnumerable<ImdbSuggestion> ParseSuggestions2(string html, HtmlDocument htmlDocument = default)
        {
            var doc = htmlDocument ?? ConvertToHtmlDoc(html);

            var nodes = doc.FindAll("//div[@class='lister-item mode-detail']");
            if (nodes.IsNotNullOrEmpty())
            {
                foreach (var node in nodes)
                {
                    var rv = ParseSingleImdbSuggestion2(node);
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
        internal IEnumerable<ImdbUserList> ParseUserLists(string html, HtmlDocument htmlDocument = default)
        {
            var doc = htmlDocument ?? ConvertToHtmlDoc(html);

            var nodes = doc.FindAll("//div[contains(@class, 'list-preview')]");
            if (nodes.IsNotNullOrEmpty())
            {
                foreach (var node in nodes)
                {
                    var rv = ParseSingleUserList(node);
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


        private ImdbEpisode ParseSingleEpisode(HtmlNode node)
        {
            var link = node.ExtendFind("div/a[@itemprop='url']");

            var ep= new ImdbEpisode();
            ep.Name = link.GetAttrib("title");
            ep.Plot = node.ExtendFind("div[@itemprop='description']")
                          .ParseText();
            ep.ImdbId = link.ExtendFind("div").GetAttrib("data-const");
            ep.Number = node.ExtendFind("div/meta[@itemprop='episodeNumber']")
                            .GetAttrib("content")
                            .ParseToInt() ?? 0;
            ep.Poster = link.ExtendFind("div/img").GetAttrib("src")
                            .ParseToUri();
            ep.Notation = link.ExtendFind("div/div")
                              .ParseText()
                              .Replace(",", "")
                              .Replace(" ", "")
                              .ToUpper();
            ep.ReleaseDate = node.ExtendFind("div/div[@class='airdate']")
                                 .ParseDateTime();
            ep.Url = (SiteUrls.IMDB + link.GetHref().SplitByAndTrim("?").First())
                           .ParseToUri();

            // parse ratings
            var ipl_rw = node.ExtendFind("div/div[@class='ipl-rating-widget']/div");
            ep.Rating.Score = ipl_rw.ExtendFind("span[@class='ipl-rating-star__rating']").ParseDouble() ?? 0;
            ep.Rating.Votes = ipl_rw.ExtendFind("span[@class='ipl-rating-star__total-votes']")
                                    .ParseText('(', ')')
                                    .ParseToInt() ?? 0;

            return ep;
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
        private ImdbUserList ParseSingleUserList(HtmlNode node)
        {
            try
            {
                var img = node.ExtendFind("div/a/img");
                var link = node.ExtendFind("div/strong/a");
                var href = link?.GetHref()?.SplitByAndTrim("?")?.FirstOrDefault();
                var meta = node.ExtendFind("div[@class='list_meta']")?.FirstChild?.ParseText();

                var ulist = new ImdbUserList();
                ulist.Name = link?.ParseText();
                ulist.ListId = href.Split('/').LastOrDefault();
                ulist.Url = (SiteUrls.IMDB + href).ParseToUri();
                if(img != null)
                {
                    ulist.ImageUrl = img?.GetAttrib("src").ParseToUri();

                    //Console.WriteLine(string.Join(",", img.Attributes.Select(x => x.Name)));
                }


                var rg = Regex.Match(meta, @"\d+");
                if (rg.Success)
                    ulist.TitlesCount = meta.Substring(rg.Index, rg.Length).ParseToInt();

                return ulist;
            }
            catch (Exception)
            {
                return null;
            }
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
        private ImdbSuggestion ParseSingleImdbSuggestion2(HtmlNode node)
        {
            var rec_poster = node.ExtendFind("div/a/img");
            string imdbId = rec_poster.GetAttrib("data-tconst");
            var infos = node.ExtendFindAll("div/p/span");
            var hd3 = node.ExtendFind("div/h3").ParseText();

            //var rec_info = node.ExtendFind("div[@class='rec_details']/div[@class='rec-info']");
            //var rec_title = rec_info.ExtendFind("div/div[@class='rec-title']");
            //var rec_genres = rec_info.ExtendFind("div/div[@class='rec-cert-genre']");
            //var rec_rating = rec_info.ExtendFind("div/div[@class='rec-rating']");

            var suggestion = new ImdbSuggestion();
            suggestion.ImdbId = imdbId;
            suggestion.Name = rec_poster.GetAttrib("alt");
            suggestion.Poster = rec_poster.GetAttrib("src").ParseToUri();
            suggestion.Url = string.Format("{0}/title/{1}", SiteUrls.IMDB, imdbId).ParseToUri();
            suggestion.Runtime = infos.FirstContainingClass("runtime").ParseText().ParseToRuntime();
            suggestion.Genres = infos.FirstContainingClass("genre").ParseText().SplitByAndTrim(",").ToList();
            suggestion.Score = node.ExtendFind("div/div/div/span[@class='ipl-rating-star__rating']").ParseDouble();

            var rg = Regex.Match(hd3, @"\d{4}");
            if (rg.Success)
            {
                int? year = hd3.Substring(rg.Index, rg.Length).ParseToInt();
                suggestion.ReleaseDate = new DateTime(year.Value, 1, 1);
            }
            //suggestion.Plot = rec_rating.ExtendFind("div[@class='rec-outline']/p").ParseText();
            //suggestion.Score = rec_rating.ExtendFind("div/span[@class='rating-rating']/span").ParseDouble();



            return suggestion;
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