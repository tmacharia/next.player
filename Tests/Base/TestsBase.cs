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
    class TestsBase : TestVariables
    {
        internal static IMapper AutoMapper => GetMapper();
        internal static TestSettingsModel Settings => ReadPrivateSettingsFile();

        internal static MetaTestModel GOT => new("tt0944947", 82, 121361, 1399, 0, "Game of Thrones");
        internal static MetaTestModel SocialNetwork => new("tt1285016", 82, 2240, 37799, 3726, "The Social Network");

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

        internal static IEnumerable MedianCases
        {
            get
            {
                yield return new MedianTestModel<int>(35, 10, 20, 30, 40, 50, 60);
                yield return new MedianTestModel<int>(35, 30, 20, 10, 60, 40, 50);
                yield return new MedianTestModel<int>(35, 60, 30, 20, 10, 40, 50);
                //yield return new MedianTestModel<int>(57, 59, 58, 60, 57, 55, 11, 67, 30, 87, 64, 12, 76, 56, 54);
            }
        }

        private static IMapper GetMapper()
        {
            var config = AutomapperConfig.Configure();
            return new Mapper(config);
        }
        private static TestSettingsModel ReadPrivateSettingsFile()
        {
            string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "settings.json"));
            return json.DeserializeTo<TestSettingsModel>();
        }

        internal virtual void Log<T>(IEnumerable<T> ts) => ts.ForEach(x => Log(x));
        internal virtual void Log(object o) => Console.WriteLine(o);
        internal virtual void Log(string format, params object[] args) => Console.WriteLine(format, args);
    }
}