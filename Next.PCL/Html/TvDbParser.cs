using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using HtmlAgilityPack;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Extensions;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Tvdb;
using Next.PCL.Static;

namespace Next.PCL.Html
{
    internal class TvDbParser : BaseParser
    {
        private readonly string Language;
        private readonly bool IncludeSpecialSeasons;

        public TvDbParser(string language = "eng", bool includeSpecialSeasons = true)
        {
            Language = language;
            IncludeSpecialSeasons = includeSpecialSeasons;
        }

        internal TvDbShow ParseShow(string html, Uri showUrl)
        {
            var doc = ConvertToHtmlDoc(html);
            var model = new TvDbShow
            {
                Url = showUrl,
                Name = doc.GetElementbyId("series_title").ParseText(),
                Plot = doc.FindAll("//div[@class='change_translation_text']")
                           .FirstWithAttrib("data-language", Language)
                           ?.ParseText(),
            };

            model.Icons = doc.GetArtworksOfType(MetaImageType.Icon);
            model.Banners = doc.GetArtworksOfType(MetaImageType.Banner);
            model.Posters = doc.GetArtworksOfType(MetaImageType.Poster);
            model.Backdrops = doc.GetArtworksOfType(MetaImageType.Backdrop);

            var lists = doc.FindAll("//div[@id='series_basic_info']/ul/li");

            model.Id = GetListItem(lists, TvDbKeys.ID).ParseToInt() ?? 0;
            model.Status = GetListItem(lists, TvDbKeys.Status).ParseToMetaStatus();
            model.AirsOn = GetListItem(lists, TvDbKeys.Airs).ParseToAirShedule();
            model.Network = GetListItem(lists, TvDbKeys.Networks);
            model.Genres = GetListItems(lists, TvDbKeys.Genres).Select(x => x.ParseText()).ToList();
            model.Settings = GetListItems(lists, TvDbKeys.Setting).Select(x => x.ParseText()).ToList();
            model.Locations = GetListItems(lists, TvDbKeys.Location).Select(x => x.ParseText()).ToList();
            model.TimePeriods = GetListItems(lists, TvDbKeys.TimePeriod).Select(x => x.ParseText()).ToList();
            model.Runtime = GetListItem(lists, TvDbKeys.Runtimes).Split('(').First().Split(' ').First().ParseToInt();

            model.OtherSites = GetListItems(lists, TvDbKeys.OtherSites).Select(x =>
            {
                var anc = x.Element("a");
                var metaUrl = new MetaUrl(MetaSource.TVDB);
                metaUrl.Url = anc.GetHref().ParseToUri();
                metaUrl.Domain = metaUrl.Url.ParseToSiteDomain(anc.ParseText());
                return metaUrl;
            }).ToList();

            model.Seasons = doc.FindAll("//div[@role='tabpanel']").FirstContainingClass("tab-official")
                               .ExtendFindAll("/ul/li").WhereHasAttrib("data-number")
                               .Select(x => ParseSimpleSeason(x))
                               .Where(x => x != null).OrderBy(x => x.Number)
                               .ToList();

            return model;
        }

        public TvdbSeason ParseSimpleSeason(HtmlNode node)
        {
            int? num = node.GetAttrib("data-number").ParseToInt();

            if (!num.HasValue)
                return null;

            if (num <= 0 && !IncludeSpecialSeasons)
                return null;

            var linkNode = node.ExtendFind("/h4/a");
            var dates = node.Element("p").ParseText().SplitByAndTrim("-");

            var model = new TvdbSeason
            {
                Number = num,
                Name = linkNode?.ParseText(),
                Url = linkNode?.GetHref()?.ParseToUri(),
                AirDate = dates.FirstOrDefault()?.ParseToDateTime(),
                LastAirDate = dates.Skip(1).FirstOrDefault()?.ParseToDateTime()
            };

            return model;
        }
        public TvdbSeason ParseSeason(string html, Uri seasonUrl)
        {
            var doc = ConvertToHtmlDoc(html);
            var model = new TvdbSeason
            {
                Url = seasonUrl,
                Name = doc.Find("//h1[@class='translated_title']").ParseText(),
                Plot = doc.FindAll("//div[@class='change_translation_text']")
                           .FirstWithAttrib("data-language", Language)?.ParseText(),
            };
            model.Id = doc.GetElementbyId("season_deleted_reason_confirm")
                       ?.GetAttrib("data-id")?.ParseToInt() ?? 0;

            model.Episodes = ParseSeasonEpisodes(null, doc).ToList();
            model.AirDate = model.Episodes.FirstOrDefault()?.AirDate;
            model.LastAirDate = model.Episodes.LastOrDefault()?.AirDate;
            model.Posters = doc.GetArtworksOfType(MetaImageType.Poster);

            return model;
        }
        
        public IEnumerable<TvdbEpisode> ParseSeasonEpisodes(string html, HtmlDocument document = default)
        {
            var doc = document ?? ConvertToHtmlDoc(html);

            var rows = doc.FindAll("//table/tbody/tr");

            int index = 0;
            foreach (var row in rows)
            {
                var tds = row.Elements("td").ToArray();
                var link = tds[1].Element("a");
                var href = link.GetHref();
                index++;

                TvdbEpisode model = new TvdbEpisode
                {
                    Number = index,
                    Name = link.ParseText(),
                    Notation = tds[0].ParseText(),
                    Url = (SiteUrls.TVDB + href).ParseToUri(),
                    Runtime = tds[3].ParseText().ParseToRuntime(),
                    AirDate = tds[2].Element("div").ParseDateTime(),
                    Id = href.SplitByAndTrim("/")
                             .Last()
                             .ParseToInt()
                             .GetValueOrDefault()
                };
                yield return model;
            }
        }
        public TvdbEpisode ParseEpisode(string html, Uri episodeUrl)
        {
            var doc = ConvertToHtmlDoc(html);
            var model = new TvdbEpisode
            {
                Url = episodeUrl,
                Name = doc.Find("//h1[@class='translated_title']").ParseText(),
                Plot = doc.FindAll("//div[@class='change_translation_text']")
                           .FirstWithAttrib("data-language", Language)
                           ?.Element("p")?.ParseText()
            };
            model.Runtime = doc.FindAll("//ul/li/strong").WhereTextEquals("runtime")
                         ?.ParentNode.Element("span").ParseText().ParseToRuntime();
            model.AirDate = doc.FindAll("//ul/li/strong").WhereTextContains("aired")
                         ?.ParentNode.SelectSingleNode("//span/a")?.ParseDateTime();
            model.Id = doc.GetElementbyId("episode_deleted_reason_confirm")
                         ?.GetAttrib("data-id").ParseToInt() ?? 0;

            model.Images = doc.GetArtworksOfType(MetaImageType.Image);

            var rows = doc.FindAll("//table/tbody/tr");
            foreach (var row in rows)
            {
                HtmlNode[] tds = row.Elements("td").ToArray();
                HtmlNode link = tds[0].Element("a");
                string href = link.GetHref();
                Uri url = (SiteUrls.TVDB + href).ParseToUri();
                int id = href.SplitByAndTrim("/").Last().ParseToInt() ?? 0;

                var person = new TvdbPerson
                {
                    Id = id,
                    Url = url,
                    Name = link.ParseText()
                };
                string type = tds[1].ParseText();
                var prf = type.ParseToProfession();
                if(prf != Profession.Other)
                    model.Crews.Add(new TvdbCrew(person) { Profession = prf });
                else
                {
                    string role = tds[2].ParseText();
                    model.Guests.Add(new TvdbCast(person) { Role = role.IsValid() ? role : type });
                }
            }

            return model;
        }

        internal IEnumerable<TvdbPerson> ParseCast(string html, HtmlDocument document = default)
        {
            var doc = document ?? ConvertToHtmlDoc(html);

            var nodes = doc.FindAll("//div[@class='thumbnail']");

            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    var cr = ParseSingleCast(item);
                    if (cr != null)
                        yield return cr;
                }
            }
        }
        internal IEnumerable<TvdbCrew> ParseCrew(string html, HtmlDocument document = default)
        {
            var doc = document ?? ConvertToHtmlDoc(html);

            var writers = doc.FindByTag("h2").WhereTextEquals("Writers")?.GetAdjacent("table")?.ExtendFindAll("tr/td/a");
            var directors = doc.FindByTag("h2").WhereTextEquals("Directors")?.GetAdjacent("table")?.ExtendFindAll("tr/td/a");

            return ParseAllCrew(directors, Profession.Director).Concat(ParseAllCrew(writers, Profession.Writer));
        }
        private IEnumerable<TvdbCrew> ParseAllCrew(HtmlNodeCollection nodes, Profession profession)
        {
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    var cr = ParseSingleCrew(item, profession);
                    if (cr != null)
                        yield return cr;
                }
            }
        }
        private TvdbCrew ParseSingleCrew(HtmlNode node, Profession profession)
        {
            string href = node.GetHref();

            if (!href.IsValid())
                return null;

            return new TvdbCrew(profession)
            {
                Name = node.ParseText(),
                Url = (SiteUrls.TVDB + href).ParseToUri(),
                Id = href.SplitByAndTrim("/").Last().ParseToInt() ?? 0,
            };
        }
        private TvdbPerson ParseSingleCast(HtmlNode node)
        {
            string name = node.Element("h3")?.Element("#text").ParseText();
            string character = node.Element("h3")?.Element("small").ParseText().Substring(2).Trim();
            string imgUrl = node.Element("img").GetAttrib("src");

            if (!imgUrl.IsValid())
                return null;

            var vs = imgUrl.Split('/');

            var cast = new TvdbPerson();
            cast.Name = name;
            cast.Role = character;
            cast.Id = vs.ElementAt(vs.Length - 2).ParseToInt() ?? 0;
            cast.Url = string.Format("{0}/people/{1}", SiteUrls.TVDB, cast.Id).ParseToUri();
            cast.Images.Add(imgUrl.ParseToUri().CreateImage(MetaImageType.Profile));

            return cast;
        }

        internal async Task<List<MetaImage>> GetAndParseImagesAsync(Uri uri, MetaImageType type, CancellationToken token = default)
        {
            string imageType = TvdbExts.CastImageType(type);
            var url = string.Format("{0}/artwork/{1}", uri.OriginalString.TrimEnd('/'), imageType).ParseToUri();
            var doc = await GetHtmlDocumentAsync(url, token);
            return doc.GetArtworksOfType(type);
        }

        private string GetListItem(HtmlNodeCollection nodes, string name)
        {
            var elem = nodes.FirstOrDefault(x => x.Element("strong").ParseText().Matches(name));
            return elem?.Element("span").ParseText();
        }
        private IEnumerable<HtmlNode> GetListItems(HtmlNodeCollection nodes, string name)
        {
            var elem = nodes.FirstOrDefault(x => x.Element("strong").ParseText().Matches(name));
            return elem?.Elements("span");
        }
    }
}