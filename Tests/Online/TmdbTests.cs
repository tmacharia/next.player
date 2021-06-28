﻿using System.Linq;
using System.Threading.Tasks;
using Next.PCL.Enums;
using Next.PCL.Exceptions;
using Next.PCL.Online;
using Next.PCL.Extensions;
using NUnit.Framework;
using Tests.Attributes;

namespace Tests.Online
{
    [Order(5)]
    [TestFixture]
    class TmdbTests : TestsBase
    {
        private Tmdb _tmdb;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _tmdb = new Tmdb(MocksAndSetups.Settings.TmdbApiKey, MocksAndSetups.AutoMapper);
            await _tmdb.ConfigureAsync();
        }

        [Case(TMDB_TESTS)]
        public void No_ApiKey_ThrowEx()
        {
            Assert.Throws<ApiKeyException>(() => new Tmdb("", null));
        }

        [Case(TMDB_TESTS, SHOW_TESTS)]
        public async Task Get_TvShow_ById()
        {
            var show = await _tmdb.GetShowAsync(GOT.TmDbID);

            Assert.NotNull(show);
            Assert.NotNull(show.Posters);
            Assert.NotNull(show.ExternalIds);
            Assert.AreEqual(GOT.TmDbID, show.Id);
            Assert.AreEqual(GOT.ImdbID, show.ExternalIds.ImdbId);

            Log(show);
        }

        [Case(TMDB_TESTS, SHOW_TESTS)]
        public async Task Search_TvShow_ByQuery()
        {
            var shows = await _tmdb.SearchShowAsync(GOT.Name);

            Assert.NotNull(shows);
            Assert.Greater(shows.Count, 0);
            Assert.AreEqual(GOT.Name, shows[0].Name);
            Assert.AreEqual(GOT.TmDbID, shows[0].Id);
            Log(shows);
        }

        [Case(TMDB_TESTS, MOVIE_TESTS)]
        public async Task Get_Movie_ById()
        {
            var mov = await _tmdb.GetMovieAsync(SocialNetwork.TmDbID);

            Assert.NotNull(mov);
            Assert.NotNull(mov.ExternalIds);
            Assert.AreEqual(SocialNetwork.TmDbID, mov.Id);
            Assert.AreEqual(SocialNetwork.ImdbID, mov.ExternalIds.ImdbId);
            Log(mov);
        }
        [Case(TMDB_TESTS, MOVIE_TESTS)]
        public async Task Search_Movie_ByQuery()
        {
            var movies = await _tmdb.SearchMovieAsync(SocialNetwork.Name);

            Assert.NotNull(movies);
            Assert.Greater(movies.Count, 0);
            Assert.AreEqual(SocialNetwork.Name, movies[0].Name);
            Assert.AreEqual(SocialNetwork.TmDbID, movies[0].Id);
            Log(movies);
        }

        [Case(TMDB_TESTS)]
        public async Task Get_Reviews()
        {
            var list = await _tmdb.GetReviewsAsync(GOT.TmDbID, MetaType.TvShow);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Log(list);
        }
        [Case(TMDB_TESTS, MOVIE_TESTS)]
        public async Task Get_Movie_Reviews()
        {
            var list = await _tmdb.GetReviewsAsync(SocialNetwork.TmDbID, MetaType.Movie);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Log(list);
        }

        [Case(TMDB_TESTS, MOVIE_TESTS)]
        public async Task Get_Movie_Suggestions()
        {
            var list = await _tmdb.SimilarMoviesAsync(SocialNetwork.TmDbID);

            Assert.That(list.Any());
            Log(list);
        }

        [Case(TMDB_TESTS, SHOW_TESTS)]
        public async Task Get_Show_Suggestions()
        {
            var list = await _tmdb.SimilarShowsAsync(GOT.TmDbID);

            Assert.That(list.Any());
            Log(list);
        }

        [Case(TMDB_TESTS)]
        public async Task Get_Movie_Credits()
        {
            var credits = await _tmdb.GetCreditsAsync(SocialNetwork.TmDbID, MetaType.Movie);

            Assert.NotNull(credits);
            // Jesse Eisenberg
            Assert.That(credits.Cast.AnyMatches("Jesse Eisenberg"));
            Assert.That(credits.Cast.AnyMatches("Justin Timberlake"));
            Assert.That(credits.Crew.AnyMatches("Aaron Sorkin"));
            Log(credits);
            Log(credits.Crew);
        }
    }
}