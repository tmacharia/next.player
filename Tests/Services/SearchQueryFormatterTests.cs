﻿using Next.PCL.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Tests.Attributes;

namespace Tests.Services
{
    [Order(0)]
    [TestFixture]
    class SearchQueryFormatterTests : TestsRoot
    {
        private ISearchQueryFormatter _searchQueryFormatter;

        [OneTimeSetUp]
        public void Setup()
        {
            _searchQueryFormatter = MocksAndSetups.SearchQueryFormatter;
        }

        [TestCase("", Category = UNIT_TESTS)]
        [TestCase(" ", Category = UNIT_TESTS)]
        [TestCase("abc", Category = UNIT_TESTS)]
        [TestCase(null, Category = UNIT_TESTS)]
        public void OnInvalidQuery_ThrowEx(string q)
        {
            Assert.Throws<ArgumentException>(() => _searchQueryFormatter.CleanAndFormat(q));
        }

        [TheoriesFrom(nameof(YtsMovieFileNames), UNIT_TESTS)]
        public void YTS_FileName(string q)
        {
            var ans = _searchQueryFormatter.CleanAndFormat(q);

            Assert.NotNull(ans);
            Assert.IsTrue(ans.Year.HasValue);
        }

        public static List<string> YtsMovieFileNames = new()
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
            "Closer.2004.720p.BluRay.x264.YIFY",

        };
    }
}