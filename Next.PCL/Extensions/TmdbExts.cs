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
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;

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
        internal static TmdbCast ToTmdbCast(this TMDbLib.Objects.TvShows.Cast cast)
        {
            if (cast != null)
                return null;
            return null;
        }
        internal static List<TmdbCrew> ToList2(this List<Crew> crews)
        {
            if (crews != null)
                return crews.Select(x => x.ToTmdbCrew()).ToList();
            return null;
        }
        internal static TmdbCrew ToTmdbCrew(this Crew crew)
        {
            if (crew != null)
                return null;
            return null;
        }

        internal static List<MetaImage> GetAllImages(this Movie model, TMDbClient client)
        {
            var images = new List<MetaImage>();

            var posters = model.GetPosters(client);
            if (posters.IsNotNullOrEmpty())
                images.AddRange(posters);

            var backdrops = model.GetBackdrops(client);
            if (backdrops.IsNotNullOrEmpty())
                images.AddRange(backdrops);

            return images;
        }
        internal static List<MetaImage> GetPosters(this Movie model, TMDbClient client)
        {
            if (model.Images == null)
                return client.ExtractImages(model, MetaImageType.Poster, x => x.PosterPath, x => x.Config.Images.PosterSizes);
            return model.Images.Posters.AsMetaImages(MetaImageType.Poster, client);
        }
        internal static List<MetaImage> GetBackdrops(this Movie model, TMDbClient client)
        {
            if (model.Images == null)
                return client.ExtractImages(model, MetaImageType.Backdrop, x => x.BackdropPath, x => x.Config.Images.BackdropSizes);

            return model.Images.Backdrops.AsMetaImages(MetaImageType.Backdrop, client);
        }

        internal static List<MetaImage> GetAllImages(this TvShow model, TMDbClient client)
        {
            var images = new List<MetaImage>();

            var posters = model.GetPosters(client);
            if (posters.IsNotNullOrEmpty())
                images.AddRange(posters);

            var backdrops = model.GetBackdrops(client);
            if (backdrops.IsNotNullOrEmpty())
                images.AddRange(backdrops);

            return images;
        }
        internal static List<MetaImage> GetPosters(this TvShow model, TMDbClient client)
        {
            if (model.Images == null)
                return client.ExtractImages(model, MetaImageType.Poster, x => x.PosterPath, x => x.Config.Images.PosterSizes);

            return model.Images.Posters.AsMetaImages(MetaImageType.Poster, client);
        }
        internal static List<MetaImage> GetBackdrops(this TvShow model, TMDbClient client)
        {
            if (model.Images == null)
                return client.ExtractImages(model, MetaImageType.Backdrop, x => x.BackdropPath, x => x.Config.Images.BackdropSizes);

            return model.Images.Backdrops.AsMetaImages(MetaImageType.Backdrop, client);
        }

        internal static List<MetaImage> GetPosters(this PosterImages model, TMDbClient client)
        {
            if (model.Posters.IsNotNullOrEmpty())
                return model.Posters.AsMetaImages(MetaImageType.Poster, client);
            return new List<MetaImage>();
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
                    Type = StringExts.ParseToMetaVideoType(x.Type),
                    Platform = StreamingPlatform.Youtube,
                    Resolution = EstimateResolution(x.Size),
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
                Resolution = EstimateResolution(x.Height, x.Width),
                Url = client.GetImageUrl("{RM_TO_SET}", x.FilePath)
            }).ToList();
            return null;
        }
        internal static List<MetaImage> GetImages<TProfile>(this TProfile profile, TMDbClient client)
            where TProfile : class, ITmdbProfile
        {
            return client.ExtractImages(profile, MetaImageType.Profile, x => x.ProfilePath, x => x.Config.Images.ProfileSizes);
        }
        internal static List<MetaImage> GetLogos(this TmdbCompany company, TMDbClient client)
        {
            return client.ExtractImages(company, MetaImageType.Logo, x => x.LogoPath, x => x.Config.Images.LogoSizes);
        }
        internal static List<MetaImage> GetLogos(this ProductionCompany company, TMDbClient client)
        {
            return client.ExtractImages(company, MetaImageType.Logo, x => x.LogoPath, x => x.Config.Images.LogoSizes);
        }

        internal static List<MetaImage> ExtractImages<TClass>(this TMDbClient client, TClass obj, MetaImageType type, Expression<Func<TClass,string>> pathSelector, Expression<Func<TMDbClient, List<string>>> sizeSelector)
            where TClass : class
        {
            if (obj == null)
                return null;

            if (!client.HasConfig)
                throw new ConfigException("Tmdb config not set.");

            string path = obj.GetPropValue(pathSelector);

            if (!path.IsValid())
                return new List<MetaImage>();

            var sizes = client.GetPropValue(sizeSelector);
            return sizes.Select(x => new
            {
                w = ushort.Parse(x.TrimStart('w')),
                url = client.GetImageUrl(x, path)
            }).Select(x => new MetaImage(type, MetaSource.TMDB)
            {
                Url = x.url,
                Width = x.w,
            }).ToList();
        }

        internal static Resolution EstimateResolution(int h, int w = 0)
        {
            return Resolution.HD;
        }
    }
}