using System.Threading.Tasks;
using Next.PCL.Exceptions;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    class TvMazeTests : TestsBase
    {
        private readonly TvMaze _maze;

        public TvMazeTests()
        {
            _maze = new TvMaze();
        }

        [TestCase(Category = TVMAZE_TESTS)]
        public void Lookup_EmptyParams_ThrowEx()
        {
            Assert.ThrowsAsync<NextArgumentException>(() => _maze.LookupAsync());
        }

        [TestCase(Category = TVMAZE_TESTS)]
        public async Task Lookup_ByImdbId()
        {
            var show = await _maze.LookupAsync(GOT.ImdbID);

            Log(show);
            Assert.NotNull(show);
            Assert.AreEqual(GOT.MazeID, show.Id);
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