using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Next.PCL.Enums;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Tvdb;

namespace Next.PCL.Extensions
{
    internal static class TvdbExts
    {
        internal static List<MetaImage> GetPosters(this TvdbSeason sn)
        {
            var metas = new List<MetaImage>();
            foreach (var item in sn.Posters)
            {
                var meta = new MetaImage(MetaImageType.Poster, MetaSource.TVDB)
                {
                    Url = item,
                    Width = 680,
                    Height = 1000,
                    Resolution = Resolution.HD
                };
                metas.Add(meta);

                meta.Width = 340;
                meta.Height = 500;
                meta.Resolution = Resolution.WVGA;

                string url = item.OriginalString;
                meta.Url = string.Format("{0}_t{1}", url
                                  .Substring(0, url.LastIndexOf('.')), url
                                  .Substring(url.LastIndexOf('.')))
                                  .ParseToUri();
                metas.Add(meta);
            }
            return metas;
        }
        internal static List<Uri> GetArtworksOfType(this HtmlDocument doc, string imageType)
        {
            return doc.FindAll($"//a[@rel='artwork_{imageType}']")
                      ?.GetAllAttribs("href")
                      .Select(x => x.ParseToUri())
                      .Where(x => x != null)
                      .ToList() 
                      ?? new List<Uri>();
        }
    }
}