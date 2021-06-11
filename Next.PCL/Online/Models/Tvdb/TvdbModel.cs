﻿using System;
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
            Locations = new List<string>();
            TimePeriods = new List<string>();

            Icons = new List<MetaImage>();
            Banners = new List<MetaImage>();
            Posters = new List<MetaImage>();
            Backdrops = new List<MetaImage>();

            OtherSites = new List<MetaUrl>();
        }
        public int Id { get; set; }
        public AirShedule AirsOn { get; set; }
        public MetaStatus Status { get; set; }
        public string Network { get; set; }
        public override Uri Poster => Posters?.FirstOrDefault()?.Url;

        public List<string> Settings { get; set; }
        public List<string> Locations { get; set; }
        public List<string> TimePeriods { get; set; }

        public List<MetaImage> Icons { get; set; }
        public List<MetaImage> Banners { get; set; }
        public List<MetaImage> Posters { get; set; }
        public List<MetaImage> Backdrops { get; set; }

        public List<MetaUrl> OtherSites { get; set; }
    }
}