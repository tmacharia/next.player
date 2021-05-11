using System;
using System.Collections.Generic;

namespace Next.PCL.Entities
{
    public class Series : MetaCommonC
    {
        public Series()
        {
            Seasons = new List<Season>();
        }
        public TVAA AgeRating { get; set; }
        public AirShedule AirsOn { get; set; }

        public List<Season> Seasons { get; set; }
    }
    public class AirShedule : EditableEntity
    {
        public TimeSpan Time { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}