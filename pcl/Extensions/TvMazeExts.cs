using System;
using System.Collections.Generic;
using Next.PCL.Enums;
using Next.PCL.Metas;
using Next.PCL.Online.Models;

namespace Next.PCL.Extensions
{
    internal static class TvMazeExts
    {
        internal static IEnumerable<MetaImage> ParseToMetaImages(this TvMazeImage tvMazeImage)
        {
            if(tvMazeImage != null && tvMazeImage.Sizes != null)
            {
                if (tvMazeImage.Sizes.Original != null)
                    yield return tvMazeImage.Sizes.Original.CreateImage(tvMazeImage.ImgType);

                if (tvMazeImage.Sizes.Medium != null)
                    yield return tvMazeImage.Sizes.Medium.CreateImage(tvMazeImage.ImgType);
            }
        }
        private static MetaImage CreateImage(this TvMazeImageUrl tvMazeImageUrl, MetaImageType type)
        {
            var meta = new MetaImage(type, MetaSource.TVMAZE, tvMazeImageUrl.Url);
            meta.Width = tvMazeImageUrl.Width;
            meta.Height = tvMazeImageUrl.Height;
            meta.Resolution = meta.DetermineResolution();
            return meta;
        }
    }
}