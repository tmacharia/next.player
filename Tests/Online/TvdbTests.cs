using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Next.PCL.Enums;
using Next.PCL.Online;
using Next.PCL.Extensions;
using NUnit.Framework;
using Tests.Attributes;
using Tests.TestModels;
using System.Collections;

namespace Tests.Online
{
    [Order(6)]
    [TestFixture]
    class TvdbTests : TestsRoot
    {
        private Tvdb _tvdb;

        [OneTimeSetUp]
        public void Setup()
        {
            _tvdb = new Tvdb(MocksAndSetups.HttpOnlineClient, MocksAndSetups.AutoMapper, MocksAndSetups.NaiveCache);
        }

        #region Tv Shows
        [ComboCase(nameof(TvShows), TVDB_TESTS, SHOW_TESTS)]
        public async Task GetShow_ByUrl(TvdbTestModel model)
        {
            var tv = await _tvdb.GetShowAsync(model.Url);

            Assert.NotNull(tv);
            Assert.AreEqual(model.TvDbId, tv.Id);
            Assert.AreEqual(model.Name, tv.Name);
            Assert.AreEqual(model.ImdbId, tv.ImdbId);
            //Assert.AreEqual(model.Status, tv.Status);

            if (model.DayOfWeek.HasValue)
            {
                Assert.NotNull(tv.AirsOn);
                Assert.AreEqual(model.DayOfWeek, tv.AirsOn.DayOfWeek);
            }
            if (model.Year.HasValue)
            {
                Assert.That(tv.ReleaseDate.HasValue);
                Assert.AreEqual(model.Year, tv.ReleaseDate.Value.Year);
            }
            if (model.Genre.IsValid())
            {
                Assert.That(tv.Genres.Any());
                Assert.That(tv.Genres.Contains(model.Genre));
            }
            if (model.Network.IsValid())
            {
                Assert.That(tv.Networks.Any());
                Assert.That(tv.Networks.AnyEquals(model.Network));
            }
           
            Assert.That(tv.Posters.Any());
            Assert.That(tv.Backdrops.Any());
            Assert.That(tv.OtherSites.Any());
            Assert.That(tv.OtherSites.All(x => x.Url != null));
            Assert.That(tv.OtherSites.All(x => x.Source == MetaSource.TVDB));
            Assert.That(tv.OtherSites.Any(x => x.Domain == OtherSiteDomain.IMDB));

            if (model.SeasonsCount.HasValue)
            {
                Assert.That(tv.Seasons.Any());
                Assert.AreEqual(model.SeasonsCount, tv.Seasons.Count);

                Log(tv.Seasons);
            }
        }

        [ComboCase(nameof(TvShows), TVDB_TESTS, SHOW_TESTS)]
        public async Task GetShow_BySlug(TvdbTestModel model)
        {
            var tv = await _tvdb.GetShowAsync(model.Name);

            Assert.NotNull(tv);
            Assert.AreEqual(model.TvDbId, tv.Id);
            Assert.AreEqual(model.Name, tv.Name);
            Assert.AreEqual(model.ImdbId, tv.ImdbId);
            //Assert.AreEqual(model.Status, tv.Status);

            if (model.DayOfWeek.HasValue)
            {
                Assert.NotNull(tv.AirsOn);
                Assert.AreEqual(model.DayOfWeek, tv.AirsOn.DayOfWeek);
            }
            if (model.Year.HasValue)
            {
                Assert.That(tv.ReleaseDate.HasValue);
                Assert.AreEqual(model.Year, tv.ReleaseDate.Value.Year);
            }
            if (model.Genre.IsValid())
            {
                Assert.That(tv.Genres.Any());
                Assert.That(tv.Genres.Contains(model.Genre));
            }
            if (model.Network.IsValid())
            {
                Assert.That(tv.Networks.Any());
                Assert.That(tv.Networks.AnyEquals(model.Network));
            }

            Assert.That(tv.Posters.Any());
            Assert.That(tv.Backdrops.Any());
            Assert.That(tv.OtherSites.Any());
            Assert.That(tv.OtherSites.All(x => x.Url != null));
            Assert.That(tv.OtherSites.All(x => x.Source == MetaSource.TVDB));
            Assert.That(tv.OtherSites.Any(x => x.Domain == OtherSiteDomain.IMDB));

            if (model.SeasonsCount.HasValue)
            {
                Assert.That(tv.Seasons.Any());
                Assert.AreEqual(model.SeasonsCount, tv.Seasons.Count);

                Log(tv.Seasons);
            }
        }
        #endregion

        #region Movies
        [Case(TVDB_TESTS, MOVIE_TESTS)]
        public async Task Get_Movie_BySlug_01()
        {
            var mov = await _tvdb.GetMovieAsync("iron-man");

            Assert.NotNull(mov);
            Assert.AreEqual("Iron Man", mov.Name);
            Assert.AreEqual(108, mov.Id);
            Assert.AreEqual(MetaStatus.Released, mov.Status);
            Assert.IsTrue(mov.ReleaseDate.HasValue);
            Assert.AreEqual(30, mov.ReleaseDate.Value.Day);
            Assert.AreEqual(4, mov.ReleaseDate.Value.Month);
            Assert.AreEqual(2008, mov.ReleaseDate.Value.Year);

            Assert.That(mov.Genres.Any());
            Assert.That(mov.Genres.Contains("Science Fiction"));

            Assert.That(mov.Cast.Any());
            Assert.That(mov.Studios.Any());
            Assert.That(mov.Studios.AnyMatches("Paramount"));
            Assert.That(mov.ProductionCompanies.Any());
            Assert.That(mov.ProductionCompanies.AnyMatches("Marvel"));

            Assert.That(mov.Posters.Any());
            Assert.That(mov.Backdrops.Any());

            Assert.That(mov.Trailers.Any());
        }
        [Case(TVDB_TESTS, MOVIE_TESTS)]
        public async Task Get_Movie_BySlug_02()
        {
            var mov = await _tvdb.GetMovieAsync("moneyball");

            Assert.NotNull(mov);
            Assert.AreEqual(3053, mov.Id);
            Assert.AreEqual("Moneyball", mov.Name);
            Assert.AreEqual("tt1210166", mov.ImdbId);
            Assert.AreEqual(MetaStatus.Released, mov.Status);
            Assert.IsTrue(mov.Runtime.HasValue);
            Assert.AreEqual(134, mov.Runtime.Value);
            Assert.IsTrue(mov.ReleaseDate.HasValue);
            Assert.AreEqual(22, mov.ReleaseDate.Value.Day);
            Assert.AreEqual(9, mov.ReleaseDate.Value.Month);
            Assert.AreEqual(2011, mov.ReleaseDate.Value.Year);

            Assert.That(mov.Genres.Any());
            Assert.That(mov.Genres.Contains("Drama"));

            Assert.That(mov.Cast.Any());
            Assert.That(mov.Studios.Any());
            Assert.That(mov.Studios.AnyMatches("Sony"));
            Assert.That(mov.ProductionCompanies.Any());
            Assert.That(mov.ProductionCompanies.AnyMatches("Columbia"));

            Assert.That(mov.Posters.Any());
            Assert.That(mov.Backdrops.Any());

            Assert.That(mov.Trailers.Any());
        }
        #endregion

        #region Companies
        [Case(TVDB_TESTS, COMPANY_TESTS)]
        public async Task Get_Company_BySlug()
        {
            var model = await _tvdb.GetCompanyAsync("netflix");

            Assert.NotNull(model);
            Assert.AreEqual(535, model.Id);
            Assert.AreEqual("Netflix", model.Name);

            Assert.That(model.Images.Any());
        }
        [Case(TVDB_TESTS, COMPANY_TESTS, SHOW_TESTS)]
        public async Task Get_Movies_ByCompany()
        {
            var model = await _tvdb.GetMoviesByCompanyAsync("netflix");
            var list = model.ToList();

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Url != null));
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.All(x => x.Posters.Any()));
            Assert.That(list.All(x => x.Posters.All(p => p.Url != null)));
        }
        [Case(TVDB_TESTS, COMPANY_TESTS, SHOW_TESTS)]
        public async Task Get_Shows_ByCompany()
        {
            var model = await _tvdb.GetShowsByCompanyAsync("netflix");
            var list = model.ToList();

            Assert.NotNull(list);
            Assert.That(list.All(x => x != null));
            Assert.That(list.All(x => x.Url != null));
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.All(x => x.Posters.Any()));
            Assert.That(list.All(x => x.Posters.All(p => p.Url != null)));
        }
        #endregion

        #region Seasons
        [Case(TVDB_TESTS, SEASON_TESTS)]
        public async Task Get_Season_ByUrl()
        {
            var uri = new Uri("https://www.thetvdb.com/series/true-detective/seasons/official/1");
            var sn = await _tvdb.GetSeasonAsync(uri);

            Assert.NotNull(sn);
            Assert.AreEqual(522572, sn.Id);
            Assert.AreEqual(uri, sn.Url);
            Assert.That(sn.Plot.IsValid());
            Assert.GreaterOrEqual(sn.Posters.Count, 1);
            Assert.AreEqual(12, sn.AirDate.Value.Day);
            Assert.AreEqual(1, sn.AirDate.Value.Month);
            Assert.AreEqual(2014, sn.AirDate.Value.Year);
            Assert.That(sn.LastAirDate.HasValue);
        }
        [Case(TVDB_TESTS, SEASON_TESTS)]
        public async Task Get_Season_ByNumber()
        {
            var sn = await _tvdb.GetSeasonAsync("true-detective", 3);

            Assert.NotNull(sn);
            Assert.AreEqual(782266, sn.Id);
            Assert.GreaterOrEqual(sn.Posters.Count, 1);
            Assert.AreEqual(13, sn.AirDate.Value.Day);
            Assert.AreEqual(1, sn.AirDate.Value.Month);
            Assert.AreEqual(2019, sn.AirDate.Value.Year);
            Assert.That(sn.LastAirDate.HasValue);
        }
        #endregion

        #region Episodes
        [Case(TVDB_TESTS, EPISODE_TESTS, CAST_TESTS)]
        public async Task Get_Episode_WithCastAndCrew()
        {
            var ep = await _tvdb.GetEpisodeAsync("true-detective", 4592328);

            Assert.NotNull(ep);
            Assert.AreEqual(4592328, ep.Id);
            Assert.NotNull(ep.Crews);
            Assert.NotNull(ep.Guests);
            Assert.GreaterOrEqual(ep.Crews.Count, 1);
            Assert.GreaterOrEqual(ep.Guests.Count, 1);

            Assert.That(ep.Crews.All(x => x.Id > 0));
            Assert.That(ep.Crews.All(x => x.Name.IsValid()));
            Assert.That(ep.Guests.All(x => x.Id > 0));
            Assert.That(ep.Guests.All(x => x.Name.IsValid()));
        }
        #endregion

        #region Cast & Crew
        [Case(TVDB_TESTS, CAST_TESTS, SHOW_TESTS)]
        public async Task Get_Show_Cast()
        {
            var list = await _tvdb.GetCastAsync(SHOW_URL);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Id > 0));
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.All(x => x.Images.Count > 0));

            Log(list);
        }
        [Case(TVDB_TESTS, CAST_TESTS, SHOW_TESTS)]
        public async Task Get_Show_Crew()
        {
            var list = await _tvdb.GetCrewAsync(SHOW_URL);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Id > 0));
            Assert.That(list.All(x => x.Name.IsValid()));

            Log(list);
        }
        [Case(TVDB_TESTS, CAST_TESTS, SHOW_TESTS)]
        public async Task Get_Show_Both_CastAndCrew()
        {
            var list = await _tvdb.GetCastAndCrewAsync(SHOW_URL);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Id > 0));
            Assert.That(list.All(x => x.Role.IsValid()));
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.Any(x => x.Images.Count > 0));

            Log(list);
        }

        [Case(TVDB_TESTS, CAST_TESTS, MOVIE_TESTS)]
        public async Task Get_Movie_Cast()
        {
            var list = await _tvdb.GetCastAsync(MOVIE_URL);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Id > 0));
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.All(x => x.Role.IsValid()));
            Assert.That(list.AnyEquals("Robert Downey Jr."));
            Log(list);
        }
        [Case(TVDB_TESTS, CAST_TESTS, MOVIE_TESTS)]
        public async Task Get_Movie_Crew()
        {
            var list = await _tvdb.GetCrewAsync(MOVIE_URL);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Id > 0));
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.Any(x => x.Profession == Profession.Writer));
            Assert.That(list.Any(x => x.Profession == Profession.Director));
            Assert.That(list.Any(x => x.Profession == Profession.Producer));
            Log(list);
        }
        #endregion

        #region Artworks
        [Case(TVDB_TESTS)]
        public async Task Get_All_Artworks()
        {
            var imgs = await _tvdb.GetArtworksAsync(SHOW_URL);

            Assert.NotNull(imgs);
            Assert.That(imgs.Count > 0);
            Assert.That(imgs.Any(x => x.Type == MetaImageType.Icon));
            Assert.That(imgs.Any(x => x.Type == MetaImageType.Banner));
            Assert.That(imgs.Any(x => x.Type == MetaImageType.Poster));
            Assert.That(imgs.Any(x => x.Type == MetaImageType.Backdrop));
        }
        #endregion

        public static IEnumerable TvShows
        {
            get
            {
                yield return new TvdbTestModel("tt2356777",2014, "True Detective")
                {
                    TvDbId = 270633,
                    Status = MetaStatus.Ended,
                    DayOfWeek = DayOfWeek.Sunday,
                    Genre = "Crime",
                    Network = "HBO",
                    SeasonsCount = 4,
                    Url = new("https://thetvdb.com/series/true-detective")
                };
                yield return new TvdbTestModel("tt0455275", 2005, "Prison Break")
                {
                    TvDbId = 360115,
                    Status = MetaStatus.Ended,
                    DayOfWeek = DayOfWeek.Tuesday,
                    Genre = "Crime",
                    Network = "FOX",
                    SeasonsCount = 6,
                    Url = new("https://www.thetvdb.com/series/prison-break")
                };
                yield return new TvdbTestModel("tt8289930", 2019, "Formula 1: Drive to Survive")
                {
                    TvDbId = 359913,
                    Status = MetaStatus.Airing,
                    DayOfWeek = DayOfWeek.Friday,
                    Genre = "Sport",
                    Network = "Netflix",
                    SeasonsCount = 4,
                    Url = new("https://www.thetvdb.com/series/formula-1-drive-to-survive")
                };
            }
        }

        private static readonly Uri MOVIE_URL = new("https://thetvdb.com/movies/iron-man");
        private static readonly Uri SHOW_URL = new("https://thetvdb.com/series/true-detective");
    }
}