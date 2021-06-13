using System;
using System.Collections.Generic;
using Next.PCL.Enums;
using Next.PCL.Metas;

namespace Next.PCL.Entities
{
    /// <summary>
    /// Class A of related media items. Close to all.
    /// </summary>
    public abstract class MetaCommonA : NamedEntity, IEditableEntity, IMetaIDs, IAliases, IUrls, IMetaImages, IMetaCommonA
    {
        protected MetaCommonA()
        {
            IDs = new List<MetaID>();
            Aliases = new List<Alias>();
            Urls = new List<MetaUrl>();
            Images = new List<MetaImage>();
        }

        public int MainPlotId { get; set; }
        public MetaStatus Status { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime? LastModified { get; set; }

        public List<MetaID> IDs { get; set; }
        public List<Alias> Aliases { get; set; }
        public List<MetaUrl> Urls { get; set; }
        public List<MetaImage> Images { get; set; }
    }
    /// <summary>
    /// Class B of related media items. Targets; movies,
    /// tv shows, tv episodes, and documentaries. 
    /// <b>NEGATES</b> tv seasons
    /// </summary>
    public abstract class MetaCommonB : MetaCommonA, IMetaPlots, IMetaVideos, IMetaCommonB
    {
        protected MetaCommonB() :base()
        {
            Plots = new List<MetaPlot>();
            Videos = new List<MetaVideo>();

            Casts = new List<Cast>();
            Genres = new List<Genre>();
            FilmMakers = new List<FilmMaker>();
        }
        public TimeSpan Runtime { get; set; }
        public BaseRating ImdbRating { get; set; }

        public List<MetaPlot> Plots { get; set; }
        public List<Genre> Genres { get; set; }
        public List<MetaVideo> Videos { get; set; }

        public List<Cast> Casts { get; set; }
        public List<FilmMaker> FilmMakers { get; set; }
    }
    /// <summary>
    /// Class C of related media items. Targets; movies,
    /// tv shows, and documentaries. 
    /// <b>NEGATES</b> tv seasons and episodes.
    /// </summary>
    public abstract class MetaCommonC : MetaCommonB, IMetaCommonC
    {
        protected MetaCommonC() : base()
        {
            Companies = new List<Company>();
            Locations = new List<GeographicLocation>();
            Languages = new List<Language>();
        }
        public MetaRatings MetaRatings { get; set; }

        public List<Company> Companies { get; set; }
        public List<GeographicLocation> Locations { get; set; }
        public List<Language> Languages { get; set; }
    }
}