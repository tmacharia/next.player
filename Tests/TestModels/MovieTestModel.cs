using Next.PCL.Enums;
using System;

namespace Tests.TestModels
{
    struct MovieTestModel
    {
        public MovieTestModel(string imdb,int runtime, int year, string name)
        {
            ImdbID = imdb;
            Runtime = runtime;
            Year = year;
            Name = name;
        }
        public string ImdbID { get; set; }
        public int Year { get; set; }
        public int Runtime { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, ImdbID);
        }
    }
    class TvdbTestModel
    {
        public int Id { get; set; }
        public string ImdbID { get; set; }
        public Uri Url { get; set; }
        public int? Year { get; set; }
        public int Runtime { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Network { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public MetaStatus Status { get; set; }
        public int? SeasonsCount { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}",
                Year.HasValue ? Year.ToString() : "??",
                Name,
                Url);
        }
    }
}