using System;
using System.Collections.Generic;
using System.Linq;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Metas;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvdbModel : BaseOnlineModel
    {
        public TvdbModel() :base()
        {
            Source = MetaSource.TVDB;

            Settings = new List<string>();
            TimePeriods = new List<string>();

            Icons = new List<MetaImageNx>();
            Banners = new List<MetaImageNx>();
            Posters = new List<MetaImageNx>();
            Backdrops = new List<MetaImageNx>();
            ClearArts = new List<MetaImageNx>();
            ClearLogos = new List<MetaImageNx>();

            Cast = new List<Cast>();

            OtherSites = new List<MetaUrl>();
            Trailers = new List<MetaVideo>();
            Locations = new List<GeographicLocation>();
            ProductionCountries = new List<GeographicLocation>();
        }
        public int Id { get; set; }
        public MetaStatus Status { get; set; }
        public override Uri Poster => Posters?.FirstOrDefault()?.Url;

        public List<string> Settings { get; set; }
        public List<string> TimePeriods { get; set; }

        public List<MetaImageNx> Icons { get; set; }
        public List<MetaImageNx> Banners { get; set; }
        public List<MetaImageNx> Posters { get; set; }
        public List<MetaImageNx> Backdrops { get; set; }
        public List<MetaImageNx> ClearArts { get; set; }
        public List<MetaImageNx> ClearLogos { get; set; }

        public List<Cast> Cast { get; set; }

        public List<MetaUrl> OtherSites { get; set; }
        public List<MetaVideo> Trailers { get; set; }
        public List<GeographicLocation> Locations { get; set; }
        public List<GeographicLocation> ProductionCountries { get; set; }
    }
}