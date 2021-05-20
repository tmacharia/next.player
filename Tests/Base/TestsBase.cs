using System;
using System.Collections.Generic;
using Tests.TestModels;

namespace Tests
{
    class TestsBase : TestVariables
    {
        internal (string ImdbID, int TvDbID, int MazeID) GOT => ("tt0944947", 121361, 82);

        internal static List<string> Genres => new()
        {
            "Comedy,Drama",
        };
        internal static List<DateTimeFormat> ReleaseDates => new()
        {
            new DateTimeFormat("07 Feb 2020","dd MMM yyyy"), // from OMDB
            new DateTimeFormat("2011-04-17", "yyyy-MM-dd") // from TVMaze
        };
        internal static List<Tuple<string, int>> MetaScores => new()
        {
            new Tuple<string, int>("8", 8),
            new Tuple<string, int>("5.4", 5),
            new Tuple<string, int>("5.5", 6),
            new Tuple<string, int>("NA", 0),
            new Tuple<string, int>("N/A", 0),
        };

        internal virtual void Log(object o) => Console.WriteLine(o);
    }
}