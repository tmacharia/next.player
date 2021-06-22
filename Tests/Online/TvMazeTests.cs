using System.Threading.Tasks;
using Next.PCL.Exceptions;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    [TestFixture]
    class TvMazeTests : TestsBase
    {
        private TvMaze _maze;

        [OneTimeSetUp]
        public void Setup()
        {
            _maze = new TvMaze(MocksAndSetups.HttpOnlineClient);
        }

        [TestCase(Category = TVMAZE_TESTS)]
        public void Lookup_EmptyParams_ThrowEx()
        {
            Assert.ThrowsAsync<NextArgumentException>(() => _maze.LookupAsync());
        }

        [TestCase(Category = TVMAZE_TESTS)]
        public async Task Lookup_ByImdbId_01()
        {
            var show = await _maze.LookupAsync(GOT.ImdbID);

            Log(show);
            Assert.NotNull(show);
            Assert.AreEqual(GOT.MazeID, show.Id);
        }
        [TestCase(Category = TVMAZE_TESTS)]
        public async Task Lookup_ByImdbId_02()
        {
            var show = await _maze.LookupAsync(TheMorningShow.ImdbID);

            Log(show);
            Assert.NotNull(show);
            Assert.AreEqual(TheMorningShow.MazeID, show.Id);
        }

        [TestCase(Category = TVMAZE_TESTS)]
        public async Task Lookup_ByTvdbId()
        {
            var show = await _maze.LookupAsync(null,GOT.TvDbID);

            Assert.NotNull(show);
            Assert.AreEqual(GOT.MazeID, show.Id);
            Assert.AreEqual(GOT.ImdbID, show.ImdbId);
        }
    }
}