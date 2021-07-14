using System.Linq;
using System.Threading.Tasks;
using Next.PCL.Exceptions;
using Next.PCL.Online;
using NUnit.Framework;
using Tests.Attributes;
using Tests.TestModels;

namespace Tests.Online
{
    [Order(3)]
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

        [ComboCase(nameof(TvShows), TVMAZE_TESTS, SHOW_TESTS)]
        public async Task GetShow_ById(TvShowTestModel model)
        {
            var show = await _maze.GetShowByIdAsync(model.TvMazeId);

            Assert.NotNull(show);

            Assert.AreEqual(model.TvMazeId, show.Id);
            Assert.AreEqual(model.ImdbId, show.ImdbId);
            Assert.AreEqual(model.Name, show.Name);

            Assert.NotNull(show.ReleaseDate);

            Log(show);
        }

        [ComboCase(nameof(TvShows), TVMAZE_TESTS, SHOW_TESTS)]
        public async Task Lookup_ByImdbId(TvShowTestModel model)
        {
            var show = await _maze.LookupAsync(model.ImdbId);

            Assert.NotNull(show);

            Assert.AreEqual(model.TvMazeId, show.Id);
            Assert.AreEqual(model.ImdbId, show.ImdbId);
            Assert.AreEqual(model.Name, show.Name);

            Log(show);
        }
        
        [ComboCase(nameof(TvShows), TVMAZE_TESTS, SHOW_TESTS)]
        public async Task Lookup_ByTvdbId(TvShowTestModel model)
        {
            var show = await _maze.LookupAsync(tvdbId: model.TvDbId);

            Assert.NotNull(show);

            Assert.AreEqual(model.TvMazeId, show.Id);
            Assert.AreEqual(model.ImdbId, show.ImdbId);
            Assert.AreEqual(model.Name, show.Name);

            Log(show);
        }

        [Case(TVMAZE_TESTS, SEASON_TESTS)]
        public async Task GetSeasons_ByShowId()
        {
            var list = await _maze.GetSeasonsAsync(Veep.TvMazeId);

            Assert.NotNull(list);

            Assert.That(list.Any());
            Assert.AreEqual(Veep.SeasonsCount, list.Count);

            Log(list);
        }
        
        [Case(TVMAZE_TESTS, EPISODE_TESTS)]
        public async Task GetEpisode()
        {
            var ep = await _maze.GetEpisodeAsync(Veep.TvMazeId, 1, 1);

            Assert.NotNull(ep);
            Assert.AreEqual(1, ep.Number);
            Assert.AreEqual(1, ep.Season);
            Assert.AreEqual("Fundraiser", ep.Name);

            Assert.NotNull(ep.AirDate);
            Assert.AreEqual(22, ep.AirDate.Value.Day);
            Assert.AreEqual(04, ep.AirDate.Value.Month);
            Assert.AreEqual(2012, ep.AirDate.Value.Year);

            Log(ep);
        }

        [ComboCase(nameof(TvShows), TVMAZE_TESTS, SHOW_TESTS)]
        public async Task GetShowImages(TvShowTestModel model)
        {
            var list = await _maze.GetShowImagesAsync(showId: model.TvMazeId);

            Assert.NotNull(list);
            Assert.That(list.Any());

            Log(list);
        }
    }
}