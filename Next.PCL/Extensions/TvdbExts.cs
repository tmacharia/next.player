using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using HtmlAgilityPack;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Tvdb;
using Next.PCL.Static;

namespace Next.PCL.Extensions
{
    internal static class TvdbExts
    {
        internal static AirShedule ParseToAirShedule(this string s)
        {
            if (s.IsValid())
            {
                bool a = Enum.TryParse(s.Split(',').First(), out DayOfWeek wk);
                bool b = DateTime.TryParse(s.Split(' ').Last(), out DateTime tm);

                if (a && b)
                {
                    return new AirShedule()
                    {
                        DayOfWeek = wk,
                        Time = tm.TimeOfDay
                    };
                }
            }
            return null;
        }

        internal static List<MetaImage> GetAsPosters(this List<Uri> uris)
        {
            var metas = new List<MetaImage>();
            foreach (var item in uris)
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
        internal static List<MetaImage> GetAsBackdrops(this List<Uri> uris)
        {
            var metas = new List<MetaImage>();
            foreach (var item in uris)
            {
                var meta = new MetaImage(MetaImageType.Backdrop, MetaSource.TVDB)
                {
                    Url = item,
                    Width = 1920,
                    Height = 1080,
                    Resolution = Resolution.FullHD
                };
                metas.Add(meta);

                meta.Width = 640;
                meta.Height = 360;
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
        internal static List<MetaImage> GetArtworksOfType(this HtmlDocument doc, MetaImageType type)
        {
            string imageType = CastImageType(type);

            var images = new List<MetaImage>();
            var anchors = doc.FindAll($"//a[@rel='artwork_{imageType}']");
            foreach (var item in anchors)
            {
                var link_url = item.GetHref().ParseToUri();
                var img_url = item.Element("img")?.GetAttrib("src")?.ParseToUri();

                if (img_url != null)
                    images.Add(img_url.CreateImage(type));

                if (link_url != null)
                    images.Add(link_url.CreateImage(type));
            }
            return images;
        }
        private static MetaImage CreateImage(this Uri uri, MetaImageType type)
        {
            var meta = new MetaImage(type, MetaSource.TVDB)
            {
                Url = uri,
                Resolution = Resolution.HD
            };
            var (w, h) = meta.Type.GetDimensions();
            meta.Width = w;
            meta.Height = h;
            return meta;
        }
        private static (ushort w, ushort h) GetDimensions(this MetaImageType type, bool tinyOrOriginal = false)
        {
            ushort w=0; ushort h=0;

            switch (type)
            {
                case MetaImageType.Icon:
                    break;
                case MetaImageType.Poster:
                    break;
                case MetaImageType.Backdrop:
                    break;
            }
            return (w, h);
        }
        internal static string CastImageType(MetaImageType metaImageType)
        {
            switch (metaImageType)
            {
                case MetaImageType.Icon: return TvDbKeys.Icons;
                case MetaImageType.Poster: return TvDbKeys.Posters;
                case MetaImageType.Banner: return TvDbKeys.Banners;
                case MetaImageType.Backdrop: return TvDbKeys.Backdrops;
                default: return TvDbKeys.Images;
            }
        }
    }
}