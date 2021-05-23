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

        internal static List<MetaImage> GetPosters(this Movie mov, TMDbClient client)
        {
            if(mov.Images == null)
                return client.ExtractImages(mov, MetaImageType.Poster, x => x.PosterPath, x => x.Config.Images.PosterSizes);

            return mov.Images.Posters.AsMetaImages(MetaImageType.Poster, client);
        }
        internal static List<MetaImage> GetBackdrops(this Movie mov, TMDbClient client)
        {
            if (mov.Images == null)
                return client.ExtractImages(mov, MetaImageType.Backdrop, x => x.BackdropPath, x => x.Config.Images.BackdropSizes);

            return mov.Images.Backdrops.AsMetaImages(MetaImageType.Backdrop, client);
        }

        internal static List<MetaImage> AsMetaImages(this List<ImageData> images, MetaImageType type, TMDbClient client)
        {
            if (images != null)
                return images.Select(x =>
            new MetaImage(type, MetaSource.TMDB)
            {
                Width = (ushort)x.Width,
                Height = (ushort)x.Height,
                Resolution = EstimateResolution(x.Width, x.Height),
                Url = client.GetImageUrl("{RM_TO_SET}", x.FilePath)
            }).ToList();
            return null;
        }
        internal static List<MetaImage> GetImages<TProfile>(this TProfile profile, TMDbClient client)
            where TProfile : class, ITmdbProfile
        {
            return client.ExtractImages(profile, MetaImageType.Profile, x => x.ProfilePath, x => x.Config.Images.ProfileSizes);
        }
        internal static List<MetaImage> GetLogos(this Company company, TMDbClient client)
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

        internal static Resolution EstimateResolution(int w, int h)
        {
            return Resolution.HD;
        }
    }
}