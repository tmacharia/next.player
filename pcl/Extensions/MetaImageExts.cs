using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Common;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Metas;

namespace Next.PCL.Extensions
{
    public static class MetaImageExts
    {
        private static List<int> _resolutionValues = new List<int>();

        public static Resolution DetermineResolution(this MetaImageNx img)
        {
            return EstimateResolution(img.Height, img.Width);
        }
        internal static Resolution EstimateResolution(int h, int w = 0)
        {
            return h > 0 && h <= 480 ? Resolution.SD
                : h <= 720 ? Resolution.HD
                : h <= 1080 ? Resolution.FullHD
                : h <= 2160 || h > 2160 ? Resolution.UltraHD : Resolution.Unknown;
        }
        
        public static int AddMetaImages(this List<MetaImageNx> images, IEnumerable<MetaImageNx> metas)
        {
            int n = 0;
            if (metas.IsNotNullOrEmpty())
            {
                metas.ForEach(x =>
                {
                    bool b = images.AddMetaImage(x);
                    n += b ? 1 : 0;
                });
            }
            return n;
        }
        public static bool AddMetaImage(this List<MetaImageNx> images, MetaImageNx img)
        {
            if(img != null)
            {
                if (!images.Any(x => x.Equals(img)))
                {
                    images.Add(img);
                    return true;
                }
            }
            return false;
        }

        public static Uri GetImageUrl<TModel>(this TModel model, MetaImageType type = MetaImageType.Poster, Resolution resolution = Resolution.HD, bool ifNullPickNext = true)
            where TModel : class, IMetaImages
        {
            if(model != null)
            {
                var img = model.Images.GetImageBasedOn(type, resolution, ifNullPickNext);
                if (img != null)
                    return img.Url;
            }
            return null;
        }
        internal static Uri GetImageUrlBasedOn(this IEnumerable<MetaImageNx> images, MetaImageType type = MetaImageType.Poster, Resolution resolution = Resolution.HD, bool ifNullPickNext = true)
        {
            var img = images.GetImageBasedOn(type, resolution, ifNullPickNext);
            if (img != null)
                return img.Url;
            return null;
        }
        internal static MetaImageNx GetImageBasedOn(this IEnumerable<MetaImageNx> images, MetaImageType type = MetaImageType.Poster, Resolution resolution = Resolution.HD, bool ifNullPickNext = true)
        {
            images = images.Where(x => x.Type == type);

            if (!images.Any())
                return null;

            int k = 0;
            var query = images.Where(x => x.Resolution == resolution);
            int qt = query.Count();
            if (qt > 1)
            {
                k = Constants.random.Next(0, qt);
                return query.ElementAtOrDefault(k);
            }
            else if (qt == 1)
                return query.First();

            query = GetNextResolutions(images, resolution).ToArray();

            if (ifNullPickNext)
            {
                // To-do
            }

            k = Constants.random.Next(0, query.Count());
            return query.ElementAtOrDefault(k);
        }
        private static IEnumerable<MetaImageNx> GetNextResolutions(this IEnumerable<MetaImageNx> images, Resolution current)
        {
            var resolution = GetNext(current);
            if (resolution.HasValue)
            {
                var query = images.Where(x => x.Resolution == resolution).ToArray();
                if (query.Any())
                {
                    if (resolution.Value > Resolution.WVGA)
                        return query.OrderBy(x => x.Height).ToArray();
                    else
                        return query.OrderByDescending(x => x.Height).ToArray();
                }
                else
                {
                    return images.Where(x => x.Resolution > resolution).GetNextResolutions(resolution.Value);
                }
            }
            return Array.Empty<MetaImageNx>();
        }

        internal static Resolution? GetNext(Resolution resolution)
        {
            if (_resolutionValues.Count <= 0)
                SetResolutionValues();

            foreach (var val in _resolutionValues)
            {
                if (val > (int)resolution)
                {
                    return (Resolution)val;
                }
            }

            return null;
        }
        private static void SetResolutionValues()
        {
            _resolutionValues.Clear();

            IEnumerator iter = Enum.GetValues(typeof(Resolution)).GetEnumerator();

            while (iter.MoveNext())
            {
                _resolutionValues.Add((int)iter.Current);
            }

            _resolutionValues = _resolutionValues.OrderBy(x => x).ToList();
        }
    }
}