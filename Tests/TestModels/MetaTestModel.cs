namespace Tests.TestModels
{
    struct MetaTestModel
    {
        public MetaTestModel(string imdb, int maze, int tvdb, int tmdb, string name)
        {
            MazeID = maze;
            TmDbID = tmdb;
            TvDbID = tvdb;
            Name = name;
            ImdbID = imdb;
        }
        public int MazeID { get; set; }
        public int TmDbID { get; set; }
        public int TvDbID { get; set; }
        public string Name { get; set; }
        public string ImdbID { get; set; }
    }
}