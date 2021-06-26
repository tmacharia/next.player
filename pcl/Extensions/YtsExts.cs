using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Next.PCL.Enums;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Yts;

namespace Next.PCL.Extensions
{
    internal static class YtsExts
    {
        internal static List<MetaImage> GetMetaImages(this YtsMovie model)
        {
            List<MetaImage> images = new List<MetaImage>();
            if(model != null)
            {
                model.GetImageAs(x => x.Poster, MetaImageType.Poster, Resolution.WVGA, ref images);
                model.GetImageAs(x => x.Poster_SM, MetaImageType.Poster, Resolution.WVGA, ref images);
                model.GetImageAs(x => x.Poster_LG, MetaImageType.Poster, Resolution.HD, ref images);

                model.GetImageAs(x => x.BackgroundImage, MetaImageType.Backdrop, Resolution.WVGA, ref images);
                model.GetImageAs(x => x.BackgroundImageOriginal, MetaImageType.Backdrop, Resolution.HD, ref images);

                model.GetImageAs(x => x.LargeScreenshotImage1, MetaImageType.Screenshot, Resolution.HD, ref images);
                model.GetImageAs(x => x.LargeScreenshotImage2, MetaImageType.Screenshot, Resolution.HD, ref images);
                model.GetImageAs(x => x.LargeScreenshotImage3, MetaImageType.Screenshot, Resolution.HD, ref images);
            }
            return images;
        }
        private static void GetImageAs(this YtsMovie model, Expression<Func<YtsMovie,Uri>> keySelector, MetaImageType type, Resolution resolution, ref List<MetaImage> images)
        {
            var url = model.GetPropValue2(keySelector);
            if(url != null)
            {
                images.Add(new MetaImage(type, MetaSource.YTS_MX)
                {
                    Url = url,
                    Resolution = resolution
                });
            }
        }
    }
}