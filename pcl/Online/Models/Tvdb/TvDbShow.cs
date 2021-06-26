using System.Collections.Generic;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvDbShow : TvdbModel
    {
        public TvDbShow() :base()
        {
            Networks = new List<Company>();
            Seasons = new List<TvdbSeason>();
        }
        public AirShedule AirsOn { get; set; }

        public List<Company> Networks { get; set; }
        public List<TvdbSeason> Seasons { get; set; }
    }
}