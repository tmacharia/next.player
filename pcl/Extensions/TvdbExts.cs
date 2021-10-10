using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Common;
using HtmlAgilityPack;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Exceptions;
using Next.PCL.Html;
using Next.PCL.Metas;
using Next.PCL.Static;

namespace Next.PCL.Extensions
{
    internal static class TvdbExts
    {
        internal const string IMAGE_EXTENSION = ".png";

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
        
        internal static MetaVideo ParseToMetaVideo(this HtmlNode node)
        {
            if (node != null)
            {
                if (!node.Name.EqualsOIC("a"))
                    throw new ExpectationFailedException("a", node.Name);

                var uri = node.GetHref().ParseToUri();

                return new MetaVideo(MetaSource.TVDB)
                {
                    Url = uri,
                    Type = MetaVideoType.Trailer,
                    Platform = StreamingPlatform.Youtube,
                    Key = SocialExts.GetYoubetubeKey(uri),
                };
            }
            return null;
        }
        internal static Company ParseToTvDbCompany(this HtmlNode node, CompanyService companyType = CompanyService.Network)
        {
            return node.ParseToCompany(MetaSource.TVDB, companyType);
        }
        internal static IEnumerable<MetaImageNx> ParseToImagesAs(this HtmlNode node, MetaImageType type)
        {
            if (node != null)
            {
                if (!node.Name.EqualsOIC("img"))
                    throw new ExpectationFailedException("img", node.Name);

                string img = node?.GetAttrib("src");
                
                string ext = '.' + img.Split('.').Last();
                string part = img.Substring(0, img.LastIndexOf(ext));
                Console.WriteLine(part);
                string sm, lg;

                if (part.EndsWith("_t"))
                {
                    sm = img;
                    lg = string.Format("{0}{1}", part.Substring(0, part.LastIndexOf("_t")), ext);
                }
                else
                {
                    lg = img;
                    sm = string.Format("{0}_t{1}", part, ext);
                }

                yield return sm.ParseToUri().CreateImage(type);
                yield return lg.ParseToUri().CreateImage(type, 1);
            }
            yield return null;
        }


        internal static List<MetaImageNx> GetArtworksOfType(this HtmlDocument doc, MetaImageType type)
        {
            var images = new List<MetaImageNx>();
            if(doc != null)
            {
                string imageType = CastImageType(type);

                var anchors = doc.FindAll($"//a[@rel='artwork_{imageType}']");
                if (anchors == null)
                    return images;
                foreach (var item in anchors)
                {
                    var link_url = item.GetHref().ParseToUri();
                    var img_url = item.Element("img")?.GetAttrib("src")?.ParseToUri();

                    if (img_url != null)
                        images.Add(img_url.CreateImage(type));

                    if (link_url != null)
                        images.Add(link_url.CreateImage(type, 1));
                }
            }
            return images;
        }
        internal static MetaImageNx CreateImage(this Uri uri, MetaImageType type, uint imageType = 0)
        {
            var meta = new MetaImageNx(type, MetaSource.TVDB, uri);
            var size = meta.GetDefaultDimensions(imageType);
            meta.Width = (ushort)size.Width;
            meta.Height = (ushort)size.Height;
            meta.Resolution = meta.DetermineResolution();
            return meta;
        }
        private static Size GetDefaultDimensions(this MetaImageNx metaImage, uint imageType = 0)
        {
            if (imageType == 0)
            {
                switch (metaImage.Type)
                {
                    case MetaImageType.Icon: return new Size(512, 512);
                    case MetaImageType.Logo: return new Size(256, 256);
                    case MetaImageType.Poster: return new Size(340, 500);
                    case MetaImageType.Banner: return new Size(758, 140);
                    case MetaImageType.Backdrop: return new Size(640, 360);
                    case MetaImageType.Profile: return new Size(300, 450);
                    case MetaImageType.Image: return new Size(640, 360);
                    case MetaImageType.ClearArt: return new Size(500, 281);
                    case MetaImageType.ClearLogo: return new Size(400, 155);
                }
            }
            else
            {
                switch (metaImage.Type)
                {
                    case MetaImageType.Logo: return new Size(512, 512);
                    case MetaImageType.Icon: return new Size(1024, 1024);
                    case MetaImageType.Poster: return new Size(680, 1000);
                    case MetaImageType.Banner: return new Size(758, 140);
                    case MetaImageType.Backdrop: return new Size(1920, 1080);
                    case MetaImageType.Image: return new Size(640, 360);
                    case MetaImageType.ClearArt: return new Size(1000, 562);
                    case MetaImageType.ClearLogo: return new Size(800, 310);
                }
            }
            return new Size(0, 0);
        }
        internal static string CastImageType(MetaImageType metaImageType)
        {
            switch (metaImageType)
            {
                case MetaImageType.Icon: return TvDbKeys.Icons;
                case MetaImageType.Poster: return TvDbKeys.Posters;
                case MetaImageType.Banner: return TvDbKeys.Banners;
                case MetaImageType.Backdrop: return TvDbKeys.Backdrops;
                case MetaImageType.ClearArt: return TvDbKeys.ClearArt;
                case MetaImageType.ClearLogo: return TvDbKeys.ClearLogo;
                default: return TvDbKeys.Images;
            }
        }
    }
}