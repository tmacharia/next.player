﻿using System.Linq;
using HtmlAgilityPack;
using Next.PCL.Enums;
using Next.PCL.Html;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Imdb;

namespace Next.PCL.Extensions
{
    internal static class ImdbExts
    {
        internal static ImdbImage ParseToMediaUrl(this HtmlNode node)
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
    }
}