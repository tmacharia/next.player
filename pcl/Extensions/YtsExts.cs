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
        internal static void ResolveMetaImages(this YtsMovie model)
        {
            var list = new List<MetaImageNx>();
            if(model != null)
            {
                model.AddImageTo(x => x.Poster, MetaImageType.Poster, Resolution.WVGA, ref list);
                model.AddImageTo(x => x.Poster_SM, MetaImageType.Poster, Resolution.WVGA, ref list);
                model.AddImageTo(x => x.Poster_LG, MetaImageType.Poster, Resolution.HD, ref list);

                model.AddImageTo(x => x.BackgroundImage, MetaImageType.Backdrop, Resolution.WVGA, ref list);
                model.AddImageTo(x => x.BackgroundImageOriginal, MetaImageType.Backdrop, Resolution.HD, ref list);

                model.AddImageTo(x => x.LargeScreenshotImage1, MetaImageType.Screenshot, Resolution.HD, ref list);
                model.AddImageTo(x => x.LargeScreenshotImage2, MetaImageType.Screenshot, Resolution.HD, ref list);
                model.AddImageTo(x => x.LargeScreenshotImage3, MetaImageType.Screenshot, Resolution.HD, ref list);
            }
            if (list.Count > 0)
                model.Images.AddRange(list);
        }
        private static void AddImageTo(this YtsMovie model, Expression<Func<YtsMovie,Uri>> keySelector, MetaImageType type, Resolution resolution, ref List<MetaImageNx> images)
        {
            var url = model.GetPropValue2(keySelector);
            if(url != null)
            {
                images.Add(new MetaImageNx(type, MetaSource.YTS_MX)
                {
                    Url = url,
                    Resolution = resolution
                });
            }
        }
    }
}