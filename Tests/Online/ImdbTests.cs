using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Next.PCL.Enums;
using Next.PCL.Online;
using NUnit.Framework;
using Tests.Attributes;
using Tests.TestModels;

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
            _imdb = new Imdb(MocksAndSetups.HttpOnlineClient, MocksAndSetups.NaiveCache);
        }

        [Order(0)]
        [TestCase(Category = IMDB_TESTS)]
        public void WrongPageNo_OnGetSuggestions_ThrowEx()
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _imdb.GetSuggestionsAsync(SocialNetwork.ImdbId, 0));
        }
        [Order(1)]
        [TheoriesFrom(nameof(Movies), IMDB_TESTS)]
        public async Task Get_ImdbMov(MovieTestModel mov)
        {
            var model = await _imdb.GetImdbAsync(mov.ImdbId);

            Assert.NotNull(model);
            Assert.True(model.ReleaseDate.HasValue);
            Assert.AreEqual(mov.Name, model.Name);
            Assert.AreEqual(mov.Runtime, model.Runtime);
            Assert.AreEqual(mov.Year, model.ReleaseDate.Value.Year);
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

        [Order(2)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_ImdbTV_MorningShow()
        {
            var model = await _imdb.GetImdbAsync(TheMorningShow.ImdbId);

            Assert.NotNull(model);
            Assert.That(model.Genres.Any());
            Assert.True(model.ReleaseDate.HasValue);
            //Assert.AreEqual(60, model.Runtime);
            Assert.AreEqual(2019, model.ReleaseDate.Value.Year);
            Assert.AreEqual(MetaType.TvShow, model.Type);
            Assert.NotNull(model.Trailer);
            Assert.NotNull(model.Rating);
            Log(model);
            Log(model.Rating);
            Log(model.Revenue);
        }

        [Order(1)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_Reviews()
        {
            var list = await _imdb.GetReviewsAsync(GOT.ImdbId);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Score.HasValue));
            Assert.That(list.All(x => x.Content.IsValid()));
            Assert.That(list.All(x => x.Timestamp.HasValue));
            Assert.That(list.All(x => x.Author.IsValid()));
            Log(list);
        }

        [Order(1)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_FilmingLocations()
        {
            var list = await _imdb.GetLocationsAsync(GOT.ImdbId);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.All(x => x.IsCountry));
            Log(list);
        }

        [Order(1)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_UserLists()
        {
            var list = await _imdb.GetUserListsWithAsync(SocialNetwork.ImdbId);

            Assert.NotNull(list);
            Assert.That(list.Any());

            Log(list);
        }

        [Order(3)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_Suggestions_Page1()
        {
            var list = await _imdb.GetSuggestionsAsync(SocialNetwork.ImdbId, 1);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.ImdbId.IsValid()));
            Assert.False(list.Any(x => x.ImdbId.EqualsOIC(SocialNetwork.ImdbId)),"Should query id be part of suggestions?");

            Log(list);
        }

        [Order(4)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_Suggestions_Page2()
        {
            var list = await _imdb.GetSuggestionsAsync(SocialNetwork.ImdbId, 2);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.ImdbId.IsValid()));
            Assert.False(list.Any(x => x.ImdbId.EqualsOIC(SocialNetwork.ImdbId)), "Should query id be part of suggestions?");

            Log(list);
        }

        [Order(5)]
        [TestCase(Category = IMDB_TESTS)]
        public async Task Get_Suggestions_Page3_1()
        {
            var list = await _imdb.GetSuggestionsAsync(SocialNetwork.ImdbId, 1);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.ImdbId.IsValid()));
            Assert.False(list.Any(x => x.ImdbId.EqualsOIC(SocialNetwork.ImdbId)), "Should query id be part of suggestions?");

            Log(list);
        }
        [Order(4)]
        [Case(IMDB_TESTS, SEASON_TESTS, EPISODE_TESTS)]
        public async Task ForInvalidSeason_Return_EmptyEpisodes()
        {
            var list = await _imdb.GetEpisodesAsync(Veep.ImdbId, 8);

            Assert.NotNull(list);
            Assert.False(list.Any());
        }
        [Order(4)]
        [Case(IMDB_TESTS, SEASON_TESTS, EPISODE_TESTS)]
        public async Task Get_Episodes_BySeason()
        {
            var list = await _imdb.GetEpisodesAsync(Veep.ImdbId, 1);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.ImdbId.IsValid()));

            Log(list);
        }

        [Order(5)]
        [Case(IMDB_TESTS, EPISODE_TESTS)]
        public async Task Get_Episode_ById()
        {
            var imdb = await _imdb.GetEpisodeAsync("tt2103089");

            Assert.NotNull(imdb);
            Assert.AreEqual("tt2103089", imdb.ImdbId);

            Log(imdb);
        }

        [Order(5)]
        [Case(IMDB_TESTS)]
        public async Task Get_Images_ByTitleId()
        {
            var list = await _imdb.PreFetchImagesAsync(Veep.ImdbId);

            Assert.NotNull(list);
            Assert.That(list.Any());

            Log(list);
        }
    }
}