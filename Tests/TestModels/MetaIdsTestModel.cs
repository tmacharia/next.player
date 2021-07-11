namespace Tests.TestModels
{
    class MetaIdsTestModel
    {
        public MetaIdsTestModel(string imdb)
        {
            ImdbId = imdb;
        }
        public MetaIdsTestModel(string imdb, int maze, int tvdb, int tmdb, int yts)
            :this(imdb)
        {
            TvMazeId = maze;
            TmDbId = tmdb;
            TvDbId = tvdb;
            YtsId = yts;
        }
        public int TvMazeId { get; set; }
        public int TmDbId { get; set; }
        public int TvDbId { get; set; }
        public int YtsId { get; set; }
        public string ImdbId { get; set; }
    }
    class BaseMetaTestModel : MetaIdsTestModel
    {
        public BaseMetaTestModel(string imdb)
            : base(imdb)
        { }
        public BaseMetaTestModel(string imdb, int year, string name)
            : this(imdb)
        {
            Year = year;
            Name = name;
        }
        public BaseMetaTestModel(string imdb, int maze, int tvdb, int tmdb, int yts, string name)
            : base(imdb, maze, tvdb, tmdb, yts)
        {
            Name = name;
        }
        public virtual int? Year { get; set; }
        public virtual string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Name, Year, ImdbId);
        }
    }
}