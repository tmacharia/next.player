using System.Threading.Tasks;
using Next.PCL.Exceptions;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    class TmdbTests : TestsBase
    {
        private readonly Tmdb _tmdb;

        public TmdbTests()
        {
            _tmdb = new Tmdb(Settings.TmdbApiKey);
        }

        [TestCase(Category = TMDB_TESTS)]
        public void No_ApiKey_ThrowEx()
        {
            Assert.Throws<ApiKeyException>(() => new Tmdb(""));
        }

        [TestCase(Category = TMDB_TESTS)]
        public async Task Get_ById()
        {
            var show = await _tmdb.GetShowAsync(GOT.TmDbID);

            Assert.NotNull(show);
            Assert.NotNull(show.ExternalIds);
            Assert.AreEqual(GOT.TmDbID, show.Id);
            Assert.AreEqual(GOT.ImdbID, show.ExternalIds.ImdbId);
        }

        [TestCase(Category = TMDB_TESTS)]
        public async Task Search_ByQuery()
        {
            var shows = await _tmdb.SearchShowAsync(GOT.Name);

            Assert.NotNull(shows);
            Assert.Greater(shows.Count, 0);
            Assert.AreEqual(GOT.Name, shows[0].Name);
            Assert.AreEqual(GOT.TmDbID, shows[0].Id);
        }
    }
}