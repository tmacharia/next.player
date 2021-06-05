using System;
using System.Threading.Tasks;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    class TvdbTests : TestsBase
    {
        private readonly Tvdb _tvdb;
        public TvdbTests()
        {
            _tvdb = new Tvdb();
        }

        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Episode()
        {
            var uri = new Uri("https://thetvdb.com/series/liseys-story/episodes/8221744");

            var ep = await _tvdb.GetEpisodeAsync(uri);

            Assert.NotNull(ep);
            Assert.AreEqual(8221744, ep.Id);
            Assert.AreEqual(uri, ep.Url);
            Assert.NotNull(ep.Poster);
            Assert.AreEqual("Bool Hunt", ep.Name);
            Assert.AreEqual(50, ep.Runtime);
            Assert.AreEqual(4, ep.AirDate.Value.Day);
            Assert.AreEqual(6, ep.AirDate.Value.Month);
            Assert.AreEqual(2021, ep.AirDate.Value.Year);
        }
    }
}