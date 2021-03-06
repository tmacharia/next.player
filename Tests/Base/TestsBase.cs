using System;
using System.Collections;
using System.Collections.Generic;
using Tests.TestModels;

namespace Tests
{
    class TestsBase : TestsRoot
    {
        internal static TvShowTestModel GOT => new("tt0944947", 82, 121361, 1399, 0, "Game of Thrones");
        internal static BaseMetaTestModel SocialNetwork => new("tt1285016", 82, 2240, 37799, 3726, "The Social Network");
        internal static TvShowTestModel TheMorningShow => new("tt7203552", 41524, 361563, 0, 0, "The Morning Show");
        internal static TvShowTestModel TedLasso => new("tt10986410", 44458, 383203, 97546, 0, "Ted Lasso");
        internal static TvShowTestModel Veep => new("tt1759761", 142, 237831, 0, 0, "Veep")
        {
            Year = 2012,
            SeasonsCount = 7
        };

        public static IEnumerable TvShows
        {
            get
            {
                yield return GOT;
                yield return Veep;
                yield return TedLasso;
                yield return TheMorningShow;
            }
        }
        internal static List<string> Genres => new()
        {
            "Comedy,Drama",
            "Drama"
        };
        internal static List<DateTimeFormat> ReleaseDates => new()
        {
            new DateTimeFormat("07 Feb 2020","dd MMM yyyy"), // from OMDB
            new DateTimeFormat("2011-04-17", "yyyy-MM-dd") // from TVMaze
        };
        internal static List<MovieTestModel> Movies => new()
        {
            new MovieTestModel(SocialNetwork.ImdbId,120,2010,SocialNetwork.Name),
            new MovieTestModel("tt1210166", 133, 2011, "Moneyball"),
            new MovieTestModel("tt1596363", 130, 2015, "The Big Short")
        };
        internal static List<Tuple<string, int>> MetaScores => new()
        {
            new Tuple<string, int>("8", 8),
            new Tuple<string, int>("5.4", 5),
            new Tuple<string, int>("5.5", 6),
            new Tuple<string, int>("NA", 0),
            new Tuple<string, int>("N/A", 0),
        };
    }
}