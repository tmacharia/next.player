using System;
using System.Collections.Generic;
using System.Linq;
using Next.PCL.Entities;
using Next.PCL.Enums;

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

            Icons = new List<Uri>();
            Banners = new List<Uri>();
            Posters = new List<Uri>();
            Backdrops = new List<Uri>();
        }
        public int Id { get; set; }
        public AirShedule AirsOn { get; set; }
        public MetaStatus Status { get; set; }
        public string Network { get; set; }
        public override Uri Poster => Posters?.FirstOrDefault();

        public List<string> Settings { get; set; }
        public List<string> Locations { get; set; }
        public List<string> TimePeriods { get; set; }

        public List<Uri> Icons { get; set; }
        public List<Uri> Banners { get; set; }
        public List<Uri> Posters { get; set; }
        public List<Uri> Backdrops { get; set; }
    }
}