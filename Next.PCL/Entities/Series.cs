using System;

namespace Next.PCL.Entities
{
    public class Series : MetaCommonC
    {
        public AirShedule AirsOn { get; set; }
    }
    public class AirShedule : EditableEntity
    {
        public TimeSpan Time { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}