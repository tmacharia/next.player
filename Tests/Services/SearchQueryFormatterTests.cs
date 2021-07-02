using Next.PCL.Services;
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

            Log(ans);
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
            //"A.Lot.Like.Love.2005.720p.BrRip.x264.BOKUTOX.YIFY",
            //"A.Star.Is.Born.2018.720p.BluRay.x264-[YTS.AM]",
            //"Ali.Wong.Baby.Cobra.2016.WEBRip.x264-ION10",
            //"Ali.Wong.Hard.Knock.Wife.2018.1080p.WEBRip.x264-RARBG",
            //"All.The.Money.In.The.World.2017.720p.BluRay.x264-[YTS.AM]",
            //"All.The.Way.2016.720p.BluRay.x264-[YTS.AG]",
            //"An.Education.2009.720p.BluRay.x264-[YTS.AM]",
            //"Arbitrage.2012.7200p.BrRip.x264.YIFY",
            //"Ashens.And.The.Polybius.Heist.2020.720p.BluRay.x264.AAC-[YTS.MX]",
            //"At.Eternity's.Gate.2018.720p.BluRay.x264-[YTS.AM]",
            //"Becoming.Warren.Buffet.2017.1080p.WEBRip.x264-RARBG",
            //"Betting.On.Zero.2016.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Beverly.Hills.Wedding.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Billie.Eilish.The.Worlds.A.Little.Blurry.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Blue.Marble.Sky.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Bo.Burnham.Inside.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Boogie.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Capital.In.The.Twenty-First.Century.2019.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Cherry.2021.720p.BluRay.x264.AAC-[YTS.MX]",
            //"Closer.2004.720p.BluRay.x264.YIFY",
            //"Confirmation.2016.1080p.BluRay.H264.AAC-RARBG",
            //"Created.Equal.Clarence.Thomas.In.His.Own.Words.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Destination.Wedding.2018.720p.BluRay.x264-[YTS.AM]",
            //"Downloaded.2013.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Emma..2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Equity.2016.720p.BluRay.x264-[YTS.AG]",
            //"Every.Breath.You.Take.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Fatherhood.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Fit.For.A.Prince.2021.1080p.WEBRip.x264.AAC5.1-[YTS.MX]",
            //"Flash.Of.Genius.2008.720p.WEBRip.x264-[YTS.LT]",
            //"Forever.My.Girl.2018.720p.BluRay.x264-[YTS.AM]",
            //"Friends.The.Reunion.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Genius.2016.720p.BluRay.x264-[YTS.AG]",
            //"Inside.Job.2010.720p.BluRay.x264.YIFY",
            //"Interstellar.2014.720p.BluRay.x264.YIFY",
            //"Just.The.Way.You.Are.2015.720p.WEBRip.x264-[YTS.AM]",
            //"Lincoln.2012.720p.Bluray.x264.YIFY",
            //"Lion.2016.720p.BluRay.x264-[YTS.AG]",
            //"Locked.Down.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Louis.Theroux.A.Different.Brain.2016.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Louis.Theroux.Selling.Sex.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Louis.Theroux.The.Night.In.Question.2019.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Love,.Rosie.2014.720p.BluRay.x264.YIFY",
            //"Mary.Shelley.2017.720p.BluRay.x264-[YTS.AM]",
            //"Me.Before.You.2016.720p.BluRay.x264-[YTS.AG]",
            //"MessiCirque.2019.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Minari.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"MLK.FBI.2020.720p.BluRay.x264.AAC-[YTS.MX]",
            //"Money.for.Nothing.Inside.the.Federal.Reserve.2013.1080p.WEBRip.x264-RARBG",
            //"Moneyball.2011.720p.BrRip.x264.YIFY",
            //"My.Salinger.Year.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Notting.Hill.1999.720p.BrRip.x264.BOKUTOX.YIFY",
            //"Operation.Varsity.Blues.The.College.Admissions.Scandal.2021.720p.BluRay.x264.AAC-[YTS.MX]",
            //"Oslo.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Our.Brand.Is.Crisis.2015.720p.BluRay.x264-[YTS.AG]",
            //"People.You.May.Know.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Peoples.Republic.Of.Desire.2018.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Pizza.A.Love.Story.2019.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Pornocratie.Les.Nouvelles.Multinationales.Du.Sexe.2017.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Rebel.In.The.Rye.2017.720p.BluRay.x264-[YTS.AG]",
            //"Road.Less.Traveled.2017.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Saint.Judy.2018.720p.WEBRip.x264-[YTS.AM]",
            //"Snowden.2016.720p.BluRay.x264-[YTS.AG]",
            //"Soul.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Steve.Jobs.2015.720p.BluRay.x264-[YTS.AG]",
            //"Submission.2017.720p.BluRay.x264-[YTS.AM]",
            //"Sully.2016.720p.BluRay.x264-[YTS.AG]",
            //"The.63rd.Annual.Grammy.Awards.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"The.Banker.2020.720p.BluRay.x264.AAC-[YTS.MX]",
            //"The.Big.Short.2015.720p.BluRay.x264-[YTS.AG]",
            //"The.Big.Sick.2017.720p.BluRay.x264-[YTS.AG]",
            //"The.Bookshop.2017.720p.BluRay.x264-[YTS.AM]",
            //"The.Color.Of.Money.1986.720p.BluRay.x264-[YTS.AG]",
            //"The.Company.Men.2010.720p.BluRay.x264-[YTS.AM]",
            //"The.Dissident.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"THE.END.Inside.The.Last.Days.Of.The.Obama.White.House.2017.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"The.Farthest.2017.720p.BluRay.x264-[YTS.AM]",
            //"The.Fight.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"The.Guernsey.Literary.And.Potato.Peel.Pie.Society.2018.720p.BluRay.x264-[YTS.AM]",
            //"The.Hacker.Wars.2014.720p.WEBRip.x264-[YTS.LT]",
            //"The.Help.2011.720p.BrRip.x264.YIFY",
            //"The.Hummingbird.Project.2018.720p.BluRay.x264-[YTS.LT]",
            //"The.Imitation.Game.2014.720p.BluRay.x264.YIFY",
            //"The.Invisible.Man.2020.720p.BluRay.x264.AAC-[YTS.MX]",
            //"The.New.Corporation.The.Unfortunately.Necessary.Sequel.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"The.Pirate.Bay.Away.From.Keyboard.2013.720p.BRrip.x264.GAZ.YIFY",
            //"The Rise Of Jordan Peterson 2019 x264 720p",
            //"The.Sabbatical.2015.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"The.Sleepless.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"The.Trip.To.Greece.2020.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"The.Trip.to.Italy.2014.720p.BluRay.x264.YIFY",
            //"The.Trip.To.Spain.2017.720p.BluRay.x264-[YTS.AM]",
            //"The.Wizard.Of.Lies.2017.720p.BluRay.x264-[YTS.AG]",
            //"The.World.Before.Your.Feet.2018.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"The.Year.Earth.Changed.2021.1080p.WEBRip.x264.AAC5.1-[YTS.MX]",
            //"They'll.Love.Me.When.I'm.Dead.2018.720p.WEBRip.x264-[YTS.AM]",
            //"Too.Big.To.Fail.2011.720p.BluRay.x264-[YTS.AM]",
            //"Wall.Street.Money.Never.Sleeps.2010.720p.BrRip.x264.BOKUTOX.YIFY",
            //"The.Vault.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"What.Lies.Upstream.2017.720p.WEBRip.x264-[YTS.AM]",
            //"Tom.Clancys.Without.Remorse.2021.720p.WEBRip.x264.AAC-[YTS.MX]",
            //"Wrath.Of.Man.2021.720p.WEBRip.x264.AAC-[YTS.MX]",

        };
    }
}