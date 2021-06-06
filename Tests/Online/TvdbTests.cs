using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Next.PCL.Enums;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    class TvdbTests : TestsBase
    {
        private readonly Tvdb _tvdb;

        private const string SHOW_SLUG = "liseys-story";
        private static readonly Uri SHOW_EP_URL = new("https://thetvdb.com/series/liseys-story/episodes/8221744");

        public TvdbTests()
        {
            _tvdb = new Tvdb();
        }

        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Episode_ByUrl()
        {
            var ep = await _tvdb.GetEpisodeAsync(SHOW_EP_URL);

            Assert.NotNull(ep);
            Assert.AreEqual(8221744, ep.Id);
            Assert.AreEqual(SHOW_EP_URL, ep.Url);
            Assert.NotNull(ep.Poster);
            Assert.True(ep.Plot.IsValid());
            Assert.AreEqual("Bool Hunt", ep.Name);
            Assert.AreEqual(50, ep.Runtime);
            Assert.AreEqual(4, ep.AirDate.Value.Day);
            Assert.AreEqual(6, ep.AirDate.Value.Month);
            Assert.AreEqual(2021, ep.AirDate.Value.Year);
        }

        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Episode_ByNumber()
        {
            var ep = await _tvdb.GetEpisodeAsync(SHOW_SLUG, 1, 1);

            Assert.NotNull(ep);
            Assert.AreEqual(8221744, ep.Id);
            Assert.AreEqual(SHOW_EP_URL, ep.Url);
            Assert.AreEqual("Bool Hunt", ep.Name);
            Assert.AreEqual("S01E01", ep.Notation);
            Assert.AreEqual(50, ep.Runtime);
            Assert.AreEqual(4, ep.AirDate.Value.Day);
            Assert.AreEqual(6, ep.AirDate.Value.Month);
            Assert.AreEqual(2021, ep.AirDate.Value.Year);
        }

        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Season_ByUrl()
        {
            var uri = new Uri("https://www.thetvdb.com/series/true-detective/seasons/official/1");
            var sn = await _tvdb.GetSeasonAsync(uri);

            Assert.NotNull(sn);
            Assert.AreEqual(522572, sn.Id);
            Assert.AreEqual(uri, sn.Url);
            Assert.True(sn.Plot.IsValid());
            Assert.GreaterOrEqual(sn.Posters.Count, 1);
            Assert.AreEqual(12, sn.AirDate.Value.Day);
            Assert.AreEqual(1, sn.AirDate.Value.Month);
            Assert.AreEqual(2014, sn.AirDate.Value.Year);
        }
        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Season_ByNumber()
        {
            var sn = await _tvdb.GetSeasonAsync("true-detective", 3);

            Assert.NotNull(sn);
            Assert.AreEqual(782266, sn.Id);
            Assert.GreaterOrEqual(sn.Posters.Count, 1);
            Assert.AreEqual(13, sn.AirDate.Value.Day);
            Assert.AreEqual(1, sn.AirDate.Value.Month);
            Assert.AreEqual(2019, sn.AirDate.Value.Year);
        }

        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Episode_WithCastAndCrew()
        {
            var ep = await _tvdb.GetEpisodeAsync("true-detective", 4592328);

            Assert.NotNull(ep);
            Assert.NotNull(ep.Crews);
            Assert.NotNull(ep.Guests);
            Assert.GreaterOrEqual(ep.Crews.Count, 1);
            Assert.GreaterOrEqual(ep.Guests.Count, 1);

            Assert.True(ep.Crews.All(x => x.Id > 0));
            Assert.True(ep.Crews.All(x => x.Name.IsValid()));
            Assert.True(ep.Guests.All(x => x.Id > 0));
            Assert.True(ep.Guests.All(x => x.Name.IsValid()));
        }

        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Show_BySlug()
        {
            var tv = await _tvdb.GetShowAsync("true-detective");

            Assert.NotNull(tv);
            Assert.AreEqual("True Detective", tv.Name);
            Assert.AreEqual(270633, tv.Id);
            Assert.AreEqual("HBO", tv.Network);
            Assert.AreEqual(MetaStatus.Airing, tv.Status);

            Assert.NotNull(tv.AirsOn);
            Assert.AreEqual(DayOfWeek.Sunday, tv.AirsOn.DayOfWeek);
            Assert.True(tv.Runtime.HasValue);
            Assert.AreEqual(59, tv.Runtime);

            Assert.NotNull(tv.Genres);
            Assert.True(tv.Genres.Contains("Crime"));
        }
    }
}