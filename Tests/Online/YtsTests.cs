using System.Linq;
using System.Threading.Tasks;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    class YtsTests : TestsBase
    {
        private readonly Yts _yts;

        public YtsTests()
        {
            _yts = new Yts();
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


        [TestCase(Category = YTS_TESTS)]
        public async Task Get_Suggestions_ByID()
        {
            var list = await _yts.GetSuggestionsAsync(SocialNetwork.YtsID);

            Assert.That(list.Any());
            Log(list);
        }
    }
}