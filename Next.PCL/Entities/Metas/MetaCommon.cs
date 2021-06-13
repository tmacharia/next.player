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

        public IList<MetaID> IDs { get; set; }
        public IList<Alias> Aliases { get; set; }
        public IList<MetaUrl> Urls { get; set; }
        public IList<MetaImage> Images { get; set; }
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

        public IList<MetaPlot> Plots { get; set; }
        public IList<Genre> Genres { get; set; }
        public IList<MetaVideo> Videos { get; set; }

        public IList<Cast> Casts { get; set; }
        public IList<FilmMaker> FilmMakers { get; set; }
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

        public IList<Company> Companies { get; set; }
        public IList<GeographicLocation> Locations { get; set; }
        public IList<Language> Languages { get; set; }
    }
}