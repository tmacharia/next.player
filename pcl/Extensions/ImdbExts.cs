using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using HtmlAgilityPack;
using Next.PCL.Enums;
using Next.PCL.Html;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Imdb;

namespace Next.PCL.Extensions
{
    internal static class ImdbExts
    {
        internal static ImdbImage ParseImageGalleryItem(this HtmlNode node)
        {
            if (node != null)
            {
                string href = node.GetHref();
                string part = href.SplitByAndTrim("?").First();

                var image = new ImdbImage();
                image.Url = (SiteUrls.IMDB + part).ParseToUri();
                image.ImdbId = part.SplitByAndTrim("/").LastOrDefault();

                var imgNode = node.ExtendFind("img");

                if(imgNode != null)
                {
                    string imgUrl = imgNode?.GetAttrib("src");
                    image.TinyImage = new MetaImage(MetaImageType.Thumbnail, MetaSource.IMDB)
                    {
                        Url = imgUrl.ParseToUri()
                    };

                }
                
                return image;
            }
            return null;
        }
        internal static IEnumerable<MetaImage> ParseImageSet(this HtmlNode node)
        {
            if (node != null)
            {
                string srcset = node.GetAttrib("srcset");
                if (srcset.IsValid())
                {
                    var sizes = srcset.SplitByAndTrim(",");
                    foreach (var sz in sizes)
                    {
                        string[] parts = sz.SplitByAndTrim(" ").ToArray();
                        int? num = parts[1].TrimEnd('w').Trim().ParseToInt();
                        int width = num ?? 0;
                        var img = new MetaImage(MetaImageType.Image, MetaSource.IMDB);
                        img.Url = parts[0].ParseToUri();
                        img.Width = (ushort)width;
                        yield return img;
                    }
                }
            }
        }

        internal static ImdbVideo ParseVideoGalleryItem(this HtmlNode node)
        {
            if (node != null)
            {
                string href = node.GetHref();
                string part = href.SplitByAndTrim("?").First();

                var image = new ImdbVideo();
                image.Url = (SiteUrls.IMDB + part).ParseToUri();
                image.ImdbId = part.SplitByAndTrim("/").LastOrDefault();

                var imgNode = node.ExtendFind("img");

                if (imgNode != null)
                {
                    string imgUrl = imgNode?.GetAttrib("src");
                    image.Poster = new MetaImage(MetaImageType.Thumbnail, MetaSource.IMDB)
                    {
                        Url = imgUrl.ParseToUri()
                    };
                }

                return image;
            }
            return null;
        }
    }
}