using Next.PCL.Enums;
using System;

namespace Tests.TestModels
{
    class MovieTestModel : BaseMetaTestModel
    {
        public MovieTestModel(string imdb, int year, string name)
            :base(imdb,year,name)
        { }
        public MovieTestModel(string imdb,int runtime, int year, string name)
            :this(imdb,year,name)
        {
            Runtime = runtime;
        }
        public MovieTestModel(string imdb, int maze, int tvdb, int tmdb, int yts, string name)
            : base(imdb, maze, tvdb, tmdb, yts, name)
        { }
        public virtual int? Runtime { get; set; }
    }
    class TvShowTestModel : BaseMetaTestModel
    {
        public TvShowTestModel(string imdb, int year, string name)
            : base(imdb, year, name)
        { }
        public TvShowTestModel(string imdb, int maze, int tvdb, int tmdb, int yts, string name)
            : base(imdb, maze, tvdb, tmdb, yts, name)
        { }
        public int? SeasonsCount { get; set; }
    }
    class TvdbTestModel : TvShowTestModel
    {
        public TvdbTestModel(string imdb, int year, string name)
            : base(imdb, year, name)
        { }
        public Uri Url { get; set; }
        public int Runtime { get; set; }
        public string Genre { get; set; }
        public string Network { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public MetaStatus Status { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}",
                Year.HasValue ? Year.ToString() : "??",
                Name,
                Url);
        }
    }
}