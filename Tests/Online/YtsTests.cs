using System.Linq;
using System.Threading.Tasks;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    [TestFixture]
    class YtsTests : TestsBase
    {
        private Yts _yts;

        [OneTimeSetUp]
        public void Setup()
        {
            _yts = new Yts(MocksAndSetups.HttpOnlineClient);
        }
        [TestCase(Category = YTS_TESTS)]
        public async Task Get_By_ID()
        {
            var model = await _yts.GetMovieByIdAsync(SocialNetwork.YtsID);

            Assert.AreEqual(SocialNetwork.ImdbID, model.ImdbId);
            Assert.AreEqual(SocialNetwork.Name, model.Name);
        }

        [TestCase(Category = YTS_TESTS)]
        public async Task Search_By_ImdbID()
        {
            var list = await _yts.SearchAsync(SocialNetwork.ImdbID);
            var model = list.FirstOrDefault();

            Assert.AreEqual(SocialNetwork.ImdbID, model.ImdbId);
            Assert.AreEqual(SocialNetwork.Name, model.Name);
            Log(model.Id);
        }

        [TestCase(Category = YTS_TESTS)]
        public async Task Search_By_Name()
        {
            var list = await _yts.SearchAsync(SocialNetwork.Name);
            var model = list.FirstOrDefault();

            Assert.AreEqual(SocialNetwork.ImdbID, model.ImdbId);
            Assert.AreEqual(SocialNetwork.Name, model.Name);
            Log(model.Id);
        }

        [Test]
        [TestCase(Category = YTS_TESTS)]
        public async Task Get_Suggestions_ByID()
        {
            var list = await _yts.GetSuggestionsAsync(SocialNetwork.YtsID);

            Assert.That(list.Any());
            Log(list);
        }
    }
}