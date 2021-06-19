using System.Linq;
using System.Threading.Tasks;
using Common;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    class ImdbTests : TestsBase
    {
        private readonly Imdb _imdb;

        public ImdbTests()
        {
            _imdb = new Imdb();
        }

        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_Imdb()
        {
            var model = await _imdb.GetImdbAsync(SocialNetwork.ImdbID);

            Assert.NotNull(model);
            Assert.That(model.Genres.Any());
            Assert.True(model.ReleaseDate.HasValue);
            Assert.AreEqual(2010, model.ReleaseDate.Value.Year);
            Log(model);
        }

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