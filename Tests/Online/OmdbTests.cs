using Next.PCL.Exceptions;
using Next.PCL.Online;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Tests.Online
{
    [Order(4)]
    [TestFixture]
    class OmdbTests : TestsBase
    {
        private Omdb _omdb;

        [OneTimeSetUp]
        public void Setup()
        {
            _omdb = new Omdb(MocksAndSetups.Settings.OmdbApiKey, MocksAndSetups.HttpOnlineClient);
        }

        [TestCase(Category = OMDB_TESTS)]
        public void IfNoApiKey_ThrowEx()
        {
            Assert.Throws<ApiKeyException>(() => new Omdb("", MocksAndSetups.HttpOnlineClient));
        }

        [TestCase(Category = OMDB_TESTS)]
        public void ForInvalidAPIKey_ThrowEx()
        {
            var omdb = new Omdb(MocksAndSetups.Settings.TmdbApiKey, MocksAndSetups.HttpOnlineClient);

            Assert.ThrowsAsync<OnlineException>(() => omdb.FindAsync(SocialNetwork.ImdbId));
        }

        [TestCase(Category = OMDB_TESTS)]
        public void ForInvalidParams_ThrowEx()
        {
            Assert.ThrowsAsync<NextArgumentException>(() => _omdb.FindAsync(""));
        }

        [TestCase(Category = OMDB_TESTS)]
        public async Task Get_By_ImdbID()
        {
            var model = await _omdb.FindAsync(SocialNetwork.ImdbId);

            Assert.AreEqual(SocialNetwork.ImdbId, model.ImdbId);
            Assert.AreEqual(SocialNetwork.Name, model.Name);
        }
    }
}