using System.Threading.Tasks;
using Next.PCL.Exceptions;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    class TmdbTests : TestsBase
    {
        private readonly Tmdb _tmdb;

        public TmdbTests()
        {
            _tmdb = new Tmdb(Settings.TmdbApiKey, AutoMapper);
        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            await _tmdb.ConfigureAsync();
        }

        [TestCase(Category = TMDB_TESTS)]
        public void No_ApiKey_ThrowEx()
        {
            Assert.Throws<ApiKeyException>(() => new Tmdb("", null));
        }

        [TestCase(Category = TMDB_TESTS)]
        public async Task Get_TvShow_ById()
        {
            var show = await _tmdb.GetShowAsync(GOT.TmDbID);

            Assert.NotNull(show);
            Assert.NotNull(show.Posters);
            Assert.NotNull(show.ExternalIds);
            Assert.AreEqual(GOT.TmDbID, show.Id);
            Assert.AreEqual(GOT.ImdbID, show.ExternalIds.ImdbId);
            //Assert.Greater(0, show.Posters.Count);
        }

        [TestCase(Category = TMDB_TESTS)]
        public async Task Search_TvShow_ByQuery()
        {
            var shows = await _tmdb.SearchShowAsync(GOT.Name);

            Assert.NotNull(shows);
            Assert.Greater(shows.Count, 0);
            Assert.AreEqual(GOT.Name, shows[0].Name);
            Assert.AreEqual(GOT.TmDbID, shows[0].Id);
        }

        [TestCase(Category = TMDB_TESTS)]
        public async Task Get_Movie_ById()
        {
            var mov = await _tmdb.GetMovieAsync(SocialNetwork.TmDbID);

            Assert.NotNull(mov);
            Assert.NotNull(mov.ExternalIds);
            Assert.AreEqual(SocialNetwork.TmDbID, mov.Id);
            Assert.AreEqual(SocialNetwork.ImdbID, mov.ExternalIds.ImdbId);
        }
        [TestCase(Category = TMDB_TESTS)]
        public async Task Search_Movie_ByQuery()
        {
            var movies = await _tmdb.SearchMovieAsync(SocialNetwork.Name);

            Assert.NotNull(movies);
            Assert.Greater(movies.Count, 0);
            Assert.AreEqual(SocialNetwork.Name, movies[0].Name);
            Assert.AreEqual(SocialNetwork.TmDbID, movies[0].Id);
        }
    }
}