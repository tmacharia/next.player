using System.Collections.Generic;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvDbShow : TvdbModel
    {
        public TvDbShow() :base()
        {
            Seasons = new List<TvdbSeason>();
        }
        public string Network { get; set; }
        public AirShedule AirsOn { get; set; }

        public List<TvdbSeason> Seasons { get; set; }
    }
}