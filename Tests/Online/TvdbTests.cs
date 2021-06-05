using System;
using System.Threading.Tasks;
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
            Assert.IsNotEmpty(ep.Plot);
            Assert.AreEqual("Bool Hunt", ep.Name);
            Assert.AreEqual(50, ep.Runtime);
            Assert.AreEqual(4, ep.AirDate.Value.Day);
            Assert.AreEqual(6, ep.AirDate.Value.Month);
            Assert.AreEqual(2021, ep.AirDate.Value.Year);
        }

        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Episode_FromSeaonsList()
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
    }
}