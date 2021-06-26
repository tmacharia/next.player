using BenchmarkDotNet.Attributes;
using Next.PCL.Services;
using System;
using System.Collections.Generic;

namespace Next.Console_
{
    [RankColumn]
    [MemoryDiagnoser]
    public class SearchQueryFormatterBenchmarks
    {
        private const int OPS = 10;
        private Random _random = new Random();
        private ISearchQueryFormatter _searchQueryFormatter = new SearchQueryFormatter();


        [Benchmark(OperationsPerInvoke = OPS)]
        public void QueryFormat_OneTitle()
        {
            int k = _random.Next(0, YtsMovieFileNames.Count);
            _searchQueryFormatter.CleanAndFormat(YtsMovieFileNames[k]);
        }

        [Benchmark]
        public void QueryFormat_MultipleTitles()
        {
            for (int i = 0; i < OPS; i++)
            {
                int k = _random.Next(0, YtsMovieFileNames.Count);
                _searchQueryFormatter.CleanAndFormat(YtsMovieFileNames[k]);
            }
        }

        public static List<string> YtsMovieFileNames = new List<string>()
        {
            "Human.Capital.2019.720p.WEBRip.x264.AAC-[YTS.MX]",
            "The.Father.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            "Hot.Money.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            "The.Bit.Player.2018.720p.BluRay.x264.AAC-[YTS.MX]",
            "Sensation.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            "Dutch.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            "76.Days.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            "A.Billion.Lives.2016.720p.WEBRip.x264.AAC-[YTS.MX]",
            "A.Game.Of.Honor.2011.720p.WEBRip.x264.AAC-[YTS.MX]",
            "A.Lot.Like.Love.2005.720p.BrRip.x264.BOKUTOX.YIFY",
            "A.Star.Is.Born.2018.720p.BluRay.x264-[YTS.AM]",
            "Ali.Wong.Baby.Cobra.2016.WEBRip.x264-ION10",
            "Ali.Wong.Hard.Knock.Wife.2018.1080p.WEBRip.x264-RARBG",
            "All.The.Money.In.The.World.2017.720p.BluRay.x264-[YTS.AM]",
            "All.The.Way.2016.720p.BluRay.x264-[YTS.AG]",
            "An.Education.2009.720p.BluRay.x264-[YTS.AM]",
            "Arbitrage.2012.7200p.BrRip.x264.YIFY",
            "Ashens.And.The.Polybius.Heist.2020.720p.BluRay.x264.AAC-[YTS.MX]",
            "At.Eternity's.Gate.2018.720p.BluRay.x264-[YTS.AM]",
            "Becoming.Warren.Buffet.2017.1080p.WEBRip.x264-RARBG",
            "Betting.On.Zero.2016.720p.WEBRip.x264.AAC-[YTS.MX]",
            "Beverly.Hills.Wedding.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            "Billie.Eilish.The.Worlds.A.Little.Blurry.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            "Blue.Marble.Sky.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            "Bo.Burnham.Inside.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            "Boogie.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            "Capital.In.The.Twenty-First.Century.2019.720p.WEBRip.x264.AAC-[YTS.MX]",
            "Cherry.2021.720p.BluRay.x264.AAC-[YTS.MX]",
            "Closer.2004.720p.BluRay.x264.YIFY"
        };
    }
}