using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Next.PCL.Enums;
using Next.PCL.Online;
using Next.PCL.Extensions;
using NUnit.Framework;
using Tests.Attributes;

namespace Tests.Online
{
    [TestFixture]
    class TvdbTests : TestsRoot
    {
        private Tvdb _tvdb;

        private const string SHOW_SLUG = "liseys-story";
        private static readonly Uri MOVIE_URL = new("https://thetvdb.com/movies/iron-man");
        private static readonly Uri SHOW_URL = new("https://thetvdb.com/series/true-detective");
        private static readonly Uri SHOW_EP_URL = new("https://thetvdb.com/series/liseys-story/episodes/8221744");

        [OneTimeSetUp]
        public void Setup()
        {
            _tvdb = new Tvdb(MocksAndSetups.HttpOnlineClient, MocksAndSetups.AutoMapper);
        }


        #region Tv Shows
        [Case(TVDB_TESTS, SHOW_TESTS)]
        public async Task Get_Show_BySlug_01()
        {
            var tv = await _tvdb.GetShowAsync("true-detective");

            Assert.NotNull(tv);
            Assert.AreEqual("True Detective", tv.Name);
            Assert.AreEqual(270633, tv.Id);

            Assert.NotNull(tv.AirsOn);
            Assert.AreEqual(DayOfWeek.Sunday, tv.AirsOn.DayOfWeek);
            Assert.That(tv.Runtime.HasValue);
            Assert.NotZero(tv.Runtime.Value);

            Assert.That(tv.Genres.Any());
            Assert.That(tv.Genres.Contains("Crime"));
            Assert.That(tv.Networks.Any());
            Assert.That(tv.Networks.Contains("HBO"));

            Assert.That(tv.Posters.Any());
            Assert.That(tv.Backdrops.Any());

            Assert.That(tv.OtherSites.Any());
            Assert.That(tv.OtherSites.All(x => x.Url != null));
            Assert.That(tv.OtherSites.All(x => x.Source == MetaSource.TVDB));
            Assert.That(tv.OtherSites.Any(x => x.Domain == OtherSiteDomain.IMDB));

            Assert.That(tv.Seasons.Any());
            Assert.AreEqual(4, tv.Seasons.Count);
            Assert.That(tv.Seasons.All(x => x.AirDate.HasValue && x.LastAirDate.HasValue));

            tv.Seasons.ForEach(x => Log("{0}, {1}", x.Number, x.Name));
            Log("\n==========\n");
            tv.OtherSites.ForEach(x => Log(x));
        }
        [Case(TVDB_TESTS, SHOW_TESTS)]
        public async Task Get_Show_BySlug_02()
        {
            var tv = await _tvdb.GetShowAsync("blindspotting-2021");

            Assert.NotNull(tv);
            Assert.AreEqual("Blindspotting", tv.Name);
            Assert.AreEqual(392720, tv.Id);
            Assert.AreEqual(MetaStatus.Airing, tv.Status);

            Assert.NotNull(tv.AirsOn);
            Assert.AreEqual(DayOfWeek.Sunday, tv.AirsOn.DayOfWeek);
            Assert.That(tv.Runtime.HasValue);
            Assert.NotZero(tv.Runtime.Value);

            Assert.That(tv.Genres.Any());
            Assert.That(tv.Genres.Contains("Drama"));
            Assert.That(tv.Networks.Any());
            Assert.That(tv.Networks.Contains("Starz"));

            Assert.That(tv.Posters.Any());
            Assert.That(tv.Backdrops.Any());

            Assert.That(tv.OtherSites.Any());
            Assert.That(tv.Trailers.Any());
            Assert.That(tv.Trailers.All(x => x.Url != null));

            Assert.That(tv.Seasons.Any());
            Assert.AreEqual(1, tv.Seasons.Count);
            Assert.That(tv.Seasons.Where(x => x.Number > 0).All(x => x.AirDate.HasValue && x.LastAirDate.HasValue));

            tv.Seasons.ForEach(x => Log("{0}, {1}", x.Number, x.Name));
            Log("\n==========\n");
            tv.Trailers.ForEach(x => Log(x.Url));
        }
        [Case(TVDB_TESTS, SHOW_TESTS)]
        public async Task Get_Show_BySlug_03()
        {
            var tv = await _tvdb.GetShowAsync("formula-1-drive-to-survive");

            Assert.NotNull(tv);
            Assert.AreEqual("Formula 1: Drive to Survive", tv.Name);
            Assert.AreEqual(359913, tv.Id);
            Assert.AreEqual(MetaStatus.Airing, tv.Status);

            Assert.NotNull(tv.AirsOn);
            Assert.AreEqual(DayOfWeek.Friday, tv.AirsOn.DayOfWeek);
            Assert.That(tv.Runtime.HasValue);
            Assert.NotZero(tv.Runtime.Value);

            Assert.That(tv.Genres.Any());
            Assert.That(tv.Genres.Contains("Sport"));
            Assert.That(tv.Networks.Any());
            Assert.That(tv.Networks.Contains("Netflix"));

            Assert.That(tv.Posters.Any());
            Assert.That(tv.Backdrops.Any());

            Assert.That(tv.OtherSites.Any());
            Assert.That(tv.Trailers.Any());
            Assert.That(tv.Trailers.All(x => x.Url != null));

            Assert.That(tv.Seasons.Any());
            Assert.AreEqual(4, tv.Seasons.Count);

            tv.Seasons.ForEach(x => Log("{0}, {1}", x.Number, x.Name));
            Log("\n==========\n");
            tv.Trailers.ForEach(x => Log(x.Url));
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
            Assert.AreEqual(MOVIE_URL, mov.Url);
            Assert.AreEqual(MetaStatus.Released, mov.Status);
            Assert.IsTrue(mov.ReleaseDate.HasValue);
            Assert.AreEqual(30, mov.ReleaseDate.Value.Day);
            Assert.AreEqual(4, mov.ReleaseDate.Value.Month);
            Assert.AreEqual(2008, mov.ReleaseDate.Value.Year);

            Assert.That(mov.Genres.Any());
            Assert.That(mov.Genres.Contains("Science Fiction"));

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
            Assert.That(list.All(x => x != null));
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
            Assert.That(list.Any());
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
        [Case(TVDB_TESTS, EPISODE_TESTS)]
        public async Task Get_Episode_ByUrl()
        {
            var ep = await _tvdb.GetEpisodeAsync(SHOW_EP_URL);

            Assert.NotNull(ep);
            Assert.AreEqual(8221744, ep.Id);
            Assert.AreEqual(SHOW_EP_URL, ep.Url);
            Assert.That(ep.Plot.IsValid());
            Assert.AreEqual("Bool Hunt", ep.Name);
            Assert.AreEqual(50, ep.Runtime);
            Assert.AreEqual(4, ep.AirDate.Value.Day);
            Assert.AreEqual(6, ep.AirDate.Value.Month);
            Assert.AreEqual(2021, ep.AirDate.Value.Year);
            Assert.That(ep.Images.Any());
        }
        [Case(TVDB_TESTS, EPISODE_TESTS)]
        public async Task Get_Episode_ByNumber()
        {
            var ep = await _tvdb.GetEpisodeAsync(SHOW_SLUG, 1, 1);

            Assert.NotNull(ep);
            Assert.AreEqual(8221744, ep.Id);
            Assert.AreEqual(SHOW_EP_URL, ep.Url);
            Assert.AreEqual("Bool Hunt", ep.Name);
            Assert.AreEqual("S01E01", ep.Notation);
            Assert.AreEqual(50, ep.Runtime);
            Assert.AreEqual(4, ep.AirDate.Value.Day);
            Assert.AreEqual(6, ep.AirDate.Value.Month);
            Assert.AreEqual(2021, ep.AirDate.Value.Year);
        }
        [Case(TVDB_TESTS, CAST_TESTS)]
        public async Task Get_Episode_WithCastAndCrew()
        {
            var ep = await _tvdb.GetEpisodeAsync("true-detective", 4592328);

            Assert.NotNull(ep);
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
            Assert.That(list.Contains("Robert Downey Jr."));
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
            Assert.That(list.All(x => x.Role.IsValid()));
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
    }
}