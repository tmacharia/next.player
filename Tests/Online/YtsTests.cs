using System.Linq;
using System.Threading.Tasks;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    [Order(4)]
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
            var model = await _yts.GetMovieByIdAsync(SocialNetwork.YtsId);

            Assert.AreEqual(SocialNetwork.ImdbId, model.ImdbId);
            Assert.AreEqual(SocialNetwork.Name, model.Name);
        }

        [TestCase(Category = YTS_TESTS)]
        public async Task Search_By_ImdbID()
        {
            var list = await _yts.SearchAsync(SocialNetwork.ImdbId);
            var model = list.FirstOrDefault();

            Assert.AreEqual(SocialNetwork.ImdbId, model.ImdbId);
            Assert.AreEqual(SocialNetwork.Name, model.Name);
            Log(model.Id);
        }

        [TestCase(Category = YTS_TESTS)]
        public async Task Search_By_Name()
        {
            var list = await _yts.SearchAsync(SocialNetwork.Name);
            var model = list.FirstOrDefault();

            Assert.AreEqual(SocialNetwork.ImdbId, model.ImdbId);
            Assert.AreEqual(SocialNetwork.Name, model.Name);
            Log(model.Id);
        }

        [Test]
        [TestCase(Category = YTS_TESTS)]
        public async Task Get_Suggestions_ByID()
        {
            var list = await _yts.GetSuggestionsAsync(SocialNetwork.YtsId);

            Assert.That(list.Any());
            Log(list);
        }
    }
}