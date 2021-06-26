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
}