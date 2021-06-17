using System.Linq;
using System.Threading.Tasks;
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
        public async Task Get_Reviews()
        {
            var list = await _imdb.GetReviewsAsync(GOT.ImdbID);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Log(list);
        }
    }
}