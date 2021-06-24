using System.Linq;
using System.Threading.Tasks;
using Common;
using Next.PCL.Enums;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    [Order(7)]
    [TestFixture]
    class ImdbTests : TestsBase
    {
        private Imdb _imdb;

        [OneTimeSetUp]
        public void Setup()
        {
            _imdb = new Imdb(MocksAndSetups.HttpOnlineClient);
        }

        [Order(0)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_ImdbMov_SocialNetwork()
        {
            var model = await _imdb.GetImdbAsync(SocialNetwork.ImdbID);

            Assert.NotNull(model);
            Assert.True(model.ReleaseDate.HasValue);
            Assert.AreEqual(120, model.Runtime);
            Assert.AreEqual(2010, model.ReleaseDate.Value.Year);
            Assert.AreEqual(MetaType.Movie, model.Type);
            Assert.NotNull(model.Trailer);
            Assert.NotNull(model.Rating);
            Assert.That(model.Genres.Any());
            Assert.That(model.Cast.Any());
            Log(model);
            Log(model.Rating);
            Log(model.Revenue);
            Log(model.Cast);
        }
        [Order(1)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_ImdbTV_MorningShow()
        {
            var model = await _imdb.GetImdbAsync(TheMorningShow.ImdbID);

            Assert.NotNull(model);
            Assert.That(model.Genres.Any());
            Assert.True(model.ReleaseDate.HasValue);
            Assert.AreEqual(60, model.Runtime);
            Assert.AreEqual(2019, model.ReleaseDate.Value.Year);
            Assert.AreEqual(MetaType.TvShow, model.Type);
            Assert.NotNull(model.Trailer);
            Assert.NotNull(model.Rating);
            Log(model);
            Log(model.Rating);
            Log(model.Revenue);
        }
        [Order(2)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_Reviews()
        {
            var list = await _imdb.GetReviewsAsync(GOT.ImdbID);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Score.HasValue));
            Assert.That(list.All(x => x.Review.IsValid()));
            Assert.That(list.All(x => x.Timestamp.HasValue));
            Assert.That(list.All(x => x.Reviewer.IsValid()));
            Log(list);
        }
        [Order(3)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_FilmingLocations()
        {
            var list = await _imdb.GetLocationsAsync(GOT.ImdbID);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.All(x => x.IsCountry));
            Log(list);
        }
    }
}