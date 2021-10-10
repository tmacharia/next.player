using System;
using System.Collections.Generic;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Metas;
using Next.PCL.Online.Models;

namespace Next.PCL.Extensions
{
    internal static class TvMazeExts
    {
        internal static IEnumerable<MetaImageNx> ParseToMetaImages(this TvMazeImage tvMazeImage)
        {
            if(tvMazeImage != null && tvMazeImage.Sizes != null)
            {
                if (tvMazeImage.Sizes.Original != null)
                    yield return tvMazeImage.Sizes.Original.CreateImage(tvMazeImage.ImgType);

                if (tvMazeImage.Sizes.Medium != null)
                    yield return tvMazeImage.Sizes.Medium.CreateImage(tvMazeImage.ImgType);
            }
        }
        internal static Cast CreateCast(this TvMazeCast model)
        {
            if (model == null || model.Person == null)
                return null;

            var cast = new Cast(model.Person.CreatePerson(), "");
            if(model.Character != null)
            {
                cast.Role = model.Character.Name;
                cast.Urls.Add(model.Character.Url.ParseToMetaUrl(MetaSource.TVMAZE));
                cast.Images.AddRange(model.Character.Image.CreateImage(MetaImageType.Profile));
            }
            return cast;
        }
        internal static FilmMaker CreateCrew(this TvMazeCrew model)
        {
            if (model == null || model.Person == null)
                return null;

            return new FilmMaker(model.Person.CreatePerson(), model.Role);
        }
        private static Person CreatePerson(this TvMazePerson model)
        {
            Person crew = new Person();
            crew.Id = model.Id;
            crew.Name = model.Name;
            crew.Gender = model.Gender;
            crew.Birthday = model.Birthday;

            crew.Urls.Add(model.Url.ParseToMetaUrl(MetaSource.TVMAZE));
            crew.Images.AddRange(model.Image.CreateImage(MetaImageType.Profile));

            return crew;
        }
        private static MetaImageNx CreateImage(this TvMazeImageUrl tvMazeImageUrl, MetaImageType type)
        {
            var meta = new MetaImageNx(type, MetaSource.TVMAZE, tvMazeImageUrl.Url);
            meta.Width = tvMazeImageUrl.Width;
            meta.Height = tvMazeImageUrl.Height;
            meta.Resolution = meta.DetermineResolution();
            return meta;
        }
        private static IEnumerable<MetaImageNx> CreateImage(this TvMazeTinyImage tinyImage, MetaImageType type)
        {
            if(tinyImage != null)
            {
                var meta = new MetaImageNx(type, MetaSource.TVMAZE);
                if(tinyImage.Original != null)
                {
                    meta.Url = tinyImage.Original;
                    yield return meta;
                }
                if (tinyImage.Medium != null)
                {
                    meta.Url = tinyImage.Medium;
                    yield return meta;
                }
            }
        }
    }
}