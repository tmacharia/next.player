using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common;
using Next.PCL.Enums;
using Next.PCL.Exceptions;
using Next.PCL.Metas;
using Next.PCL.Online.Models;
using TMDbLib.Client;
using TMDbLib.Objects.General;

namespace Next.PCL.Extensions
{
    internal static class TmdbExts
    {
        internal static List<T> GetList<T>(this SearchContainer<T> container)
        {
            if (container != null && container.Results != null)
                return container.Results;
            return null;
        }
        internal static List<T> GetList<T>(this SearchContainerWithId<T> container)
        {
            if (container != null && container.Results != null)
                return container.Results;
            return null;
        }
        
        internal static TCredit AddProfileImages<TCredit>(this TCredit model, string path, TMDbClient client)
            where TCredit : Entities.Person
        {
            model.Images.AddRange(client.ExtractImages(MetaImageType.Profile, path, x => x.Config.Images.ProfileSizes));
            return model;
        }

        internal static List<MetaImage> GetPosters(this PosterImages model, TMDbClient client)
        {
            if (model.Posters.IsNotNullOrEmpty())
                return model.Posters.AsMetaImages(MetaImageType.Poster, client);
            return new List<MetaImage>();
        }
        internal static List<MetaImage> GetPosters<TPoster>(this TPoster poster, TMDbClient client)
            where TPoster : class, IPosterPath
        {
            return client.ExtractImages(poster, MetaImageType.Logo, x => x.PosterPath, x => x.Config.Images.PosterSizes).ToList();
        }
        internal static List<MetaImage> GetStills(this StillImages model, TMDbClient client)
        {
            if (model.Stills.IsNotNullOrEmpty())
                return model.Stills.AsMetaImages(MetaImageType.Still, client);
            return new List<MetaImage>();
        }
        internal static List<MetaVideo> GetVideos(this ResultContainer<Video> model)
        {
            if (model != null && model.Results.IsNotNullOrEmpty())
                return model.Results.Select(x => new MetaVideo()
                {
                    Key = x.Key,
                    Height = (ushort)x.Size,
                    Source = MetaSource.TMDB,
                    Type = x.Type.ParseToMetaVideoType(),
                    Platform = StreamingPlatform.Youtube,
                    Resolution = MetaImageExts.EstimateResolution(x.Size),
                    Url = SocialExts.GetYoubetubeUrl(x.Key),
                }).ToList();
            return new List<MetaVideo>();
        }

        internal static List<MetaImage> GetAllImages(this ImagesWithId model, TMDbClient client)
        {
            var images = new List<MetaImage>();

            if(model != null)
            {
                if (model.Posters.IsNotNullOrEmpty())
                {
                    var posters = model.Posters.AsMetaImages(MetaImageType.Poster, client);
                    if (posters.IsNotNullOrEmpty())
                        images.AddRange(posters);
                }
                if (model.Backdrops.IsNotNullOrEmpty())
                {
                    var backdrops = model.Backdrops.AsMetaImages(MetaImageType.Backdrop, client);
                    if (backdrops.IsNotNullOrEmpty())
                        images.AddRange(backdrops);
                }
            }
            return images;
        }

        internal static List<MetaImage> AsMetaImages(this List<ImageData> images, MetaImageType type, TMDbClient client)
        {
            if (images != null)
                return images.Select(x =>
            new MetaImage(type, MetaSource.TMDB)
            {
                Width = (ushort)x.Width,
                Height = (ushort)x.Height,
                Resolution = MetaImageExts.EstimateResolution(x.Height, x.Width),
                Url = client.GetImageUrl("{RM_TO_SET}", x.FilePath)
            }).ToList();
            return null;
        }
        internal static IEnumerable<MetaImage> ExtractImages<TClass>(this TMDbClient client, TClass obj, MetaImageType type, Expression<Func<TClass,string>> pathSelector, Expression<Func<TMDbClient, List<string>>> sizeSelector)
            where TClass : class
        {
            if (obj == null)
                yield break;

            string path = obj.GetPropValue2(pathSelector);

            var rr = client.ExtractImages(type, path, sizeSelector);
            foreach (var item in rr)
                yield return item;
        }
        internal static IEnumerable<MetaImage> ExtractImages(this TMDbClient client, MetaImageType type, string path, Expression<Func<TMDbClient, List<string>>> sizeSelector)
        {
            if (!client.HasConfig)
                throw new ConfigException("Tmdb config not set.");

            if (!path.IsValid())
                yield break;

            var sizes = client.GetPropValue2(sizeSelector);

            foreach (var sz in sizes)
            {
                var img = new MetaImage(type, MetaSource.TMDB);
                img.Url = client.GetImageUrl(sz, path);
                if (sz.StartsWith("w"))
                {
                    img.Width = ushort.Parse(sz.TrimStart('w'));
                }
                yield return img;
            }
        }
    }
}