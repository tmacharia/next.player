using System;
using System.Collections.Generic;
using Next.PCL.Enums;
using Next.PCL.Metas;

namespace Next.PCL.Entities
{
    /// <summary>
    /// Class A of related media items. Close to all.
    /// </summary>
    public interface IMetaCommonA
    {
        MetaStatus Status { get; set; }
        DateTime ReleaseDate { get; set; }

        List<MetaID> IDs { get; set; }
        List<Alias> Aliases { get; set; }
        List<MetaUrl> Urls { get; set; }
        List<MetaImage> Images { get; set; }
    }
    /// <summary>
    /// Class B of related media items. Targets; movies,
    /// tv shows, tv episodes, and documentaries. 
    /// <b>NEGATES</b> tv seasons
    /// </summary>
    public interface IMetaCommonB : IMetaCommonA
    {
        TimeSpan Runtime { get; set; }

        List<Cast> Casts { get; set; }
        List<Genre> Genres { get; set; }
        List<MetaPlot> Plots { get; set; }
        List<MetaVideo> Videos { get; set; }
        List<FilmMaker> FilmMakers { get; set; }
    }
    /// <summary>
    /// Class C of related media items. Targets; movies,
    /// tv shows, and documentaries. 
    /// <b>NEGATES</b> tv seasons and episodes.
    /// </summary>
    public interface IMetaCommonC : IMetaCommonB
    {
        List<Company> Companies { get; set; }
        List<GeographicLocation> Locations { get; set; }
        List<Language> Languages { get; set; }
    }
}