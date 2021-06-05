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
        public TvdbEpisode ParseEpisode(string html, Uri episodeUrl)
        {
            HtmlDocument doc = ConvertToHtmlDoc(html);
            TvdbEpisode ep = new TvdbEpisode();
            ep.Url=episodeUrl;
            ep.Name = doc.Find("//h1[@class='translated_title']").ParseText();
            ep.Plot = doc.FindAll("//div[@class='change_translation_text']")
                           .FirstContainingAttrib("data-language", "eng")
                           ?.Element("p")
                           ?.ParseText();

            var run = doc.FindAll("//ul/li/strong").Where(x => x.TextEquals("runtime")).FirstOrDefault();
            var air = doc.FindAll("//ul/li/strong").Where(x => x.TextContains("aired")).FirstOrDefault();
            var idt = doc.GetElementbyId("episode_deleted_reason_confirm").GetAttrib("data-id").ParseToInt();

            if (idt.HasValue)
                ep.Id = idt.Value;

            if (run != null)
                ep.Runtime = run.ParentNode.Element("span").ParseText().ParseToRuntime();
            if (air != null)
                ep.AirDate = run.ParentNode.SelectSingleNode("//span/a").ParseDateTime();

            var imgs = GetImages(doc, "image");
            if (imgs.IsNotNullOrEmpty())
                ep.Poster = imgs[0];
            
            return ep;
        }

        private static List<Uri> GetImages(HtmlDocument doc, string suffix)
        {
            var list = new List<Uri>();

            var urls = doc.FindAll($"//a[@rel='artwork_{suffix}']")
                           .GetAllAttribs("href")
                           .ToList();

            foreach (var item in urls)
            {
                list.Add(new Uri(item));
            }

            return list;
        }
    }
}