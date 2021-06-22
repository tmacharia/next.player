using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Common;
using Next.PCL.AutoMap;
using Tests.TestModels;

namespace Tests
{
    class TestsBase : TestsRoot
    {
        internal static MetaTestModel GOT => new("tt0944947", 82, 121361, 1399, 0, "Game of Thrones");
        internal static MetaTestModel SocialNetwork => new("tt1285016", 82, 2240, 37799, 3726, "The Social Network");
        internal static MetaTestModel TheMorningShow => new("tt7203552", 41524, 361563, 0, 0, "The Morning Show");

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