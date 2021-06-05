using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Next.PCL.Extensions;
using Next.PCL.Online.Models.Tvdb;

namespace Next.PCL.Html
{
    internal class TvDbParser : BaseParser
    {
        private readonly string Language;

        public TvDbParser(string language = "eng")
        {
            Language = language;
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

                TvdbEpisode ep = new TvdbEpisode
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
                yield return ep;
            }
        }
        public TvdbEpisode ParseEpisode(string html, Uri episodeUrl)
        {
            var doc = ConvertToHtmlDoc(html);
            var ep = new TvdbEpisode
            {
                Url = episodeUrl,
                Name = doc.Find("//h1[@class='translated_title']").ParseText(),
                Plot = doc.FindAll("//div[@class='change_translation_text']")
                           .FirstContainingAttrib("data-language", Language)
                           ?.Element("p")
                           ?.ParseText(),
                Poster = doc.GetArtworksOfType("image")
                           .FirstOrDefault()
            };
            ep.Runtime = doc.FindAll("//ul/li/strong").Where(x => x.TextEquals("runtime")).FirstOrDefault()
                         ?.ParentNode.Element("span")
                         ?.ParseText().ParseToRuntime();
            ep.AirDate = doc.FindAll("//ul/li/strong").Where(x => x.TextContains("aired")).FirstOrDefault()
                         ?.ParentNode.SelectSingleNode("//span/a")
                         ?.ParseDateTime();
            ep.Id = doc.GetElementbyId("episode_deleted_reason_confirm")
                         ?.GetAttrib("data-id")
                         ?.ParseToInt() ?? 0;

            return ep;
        }
        public TvdbSeason ParseSeason(string html, Uri seasonUrl)
        {
            var doc = ConvertToHtmlDoc(html);
            var sn = new TvdbSeason
            {
                Url = seasonUrl,
                Name = doc.Find("//h1[@class='translated_title']").ParseText(),
                Plot = doc.FindAll("//div[@class='change_translation_text']")
                           .FirstContainingAttrib("data-language", "eng")
                           ?.ParseText(),
                Posters = doc.GetArtworksOfType("posters"),
                Episodes = ParseSeasonEpisodes(null, doc).ToList()
            };
            sn.Id = doc.GetElementbyId("season_deleted_reason_confirm")
                       ?.GetAttrib("data-id")
                       ?.ParseToInt() ?? 0;
            sn.AirDate = sn.Episodes.FirstOrDefault()?.AirDate;

            return sn;
        }
    }
}