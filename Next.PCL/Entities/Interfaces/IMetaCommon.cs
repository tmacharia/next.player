using System;
using System.Collections.Generic;
using Next.PCL.Enums;

namespace Next.PCL.Entities
{
    /// <summary>
    /// Class A of related media items. Close to all.
    /// </summary>
    public interface IMetaCommonA
    {
        MetaStatus Status { get; set; }
        DateTime ReleaseDate { get; set; }

        IList<MetaID> IDs { get; set; }
        IList<Alias> Aliases { get; set; }
        IList<MetaUrl> Urls { get; set; }
        IList<MetaImage> Images { get; set; }
    }
    /// <summary>
    /// Class B of related media items. Targets; movies,
    /// tv shows, tv episodes, and documentaries. 
    /// <b>NEGATES</b> tv seasons
    /// </summary>
    public interface IMetaCommonB : IMetaCommonA
    {
        TimeSpan Runtime { get; set; }

        IList<Cast> Casts { get; set; }
        IList<Genre> Genres { get; set; }
        IList<MetaPlot> Plots { get; set; }
        IList<MetaVideo> Videos { get; set; }
        IList<FilmMaker> FilmMakers { get; set; }
    }
    /// <summary>
    /// Class C of related media items. Targets; movies,
    /// tv shows, and documentaries. 
    /// <b>NEGATES</b> tv seasons and episodes.
    /// </summary>
    public interface IMetaCommonC : IMetaCommonB
    {
        IList<Company> Companies { get; set; }
        IList<Country> Countries { get; set; }
        IList<Language> Languages { get; set; }
    }
}