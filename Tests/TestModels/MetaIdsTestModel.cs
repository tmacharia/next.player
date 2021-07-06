namespace Tests.TestModels
{
    struct MetaIdsTestModel
    {
        public MetaIdsTestModel(string imdb, int maze, int tvdb, int tmdb, int yts, string name)
        {
            MazeID = maze;
            TmDbID = tmdb;
            TvDbID = tvdb;
            Name = name;
            YtsID = yts;
            ImdbID = imdb;
        }
        public int MazeID { get; set; }
        public int TmDbID { get; set; }
        public int TvDbID { get; set; }
        public int YtsID { get; set; }
        public string Name { get; set; }
        public string ImdbID { get; set; }
    }
}