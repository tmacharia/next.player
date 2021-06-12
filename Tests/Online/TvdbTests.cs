using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Next.PCL.Enums;
using Next.PCL.Online;
using NUnit.Framework;

namespace Tests.Online
{
    internal class TvdbTests : TestsBase
    {
        private Tvdb _tvdb;

        private const string SHOW_SLUG = "liseys-story";
        private static readonly Uri SHOW_URL = new("https://thetvdb.com/series/true-detective");
        private static readonly Uri SHOW_EP_URL = new("https://thetvdb.com/series/liseys-story/episodes/8221744");

        [OneTimeSetUp]
        public void Setup_Tests()
        {
            _tvdb = new Tvdb();
        }
        

        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Show_BySlug()
        {
            var tv = await _tvdb.GetShowAsync("true-detective");

            Assert.NotNull(tv);
            Assert.AreEqual("True Detective", tv.Name);
            Assert.AreEqual(270633, tv.Id);
            Assert.AreEqual("HBO", tv.Network);
            Assert.AreEqual(MetaStatus.Airing, tv.Status);

            Assert.NotNull(tv.AirsOn);
            Assert.AreEqual(DayOfWeek.Sunday, tv.AirsOn.DayOfWeek);
            Assert.That(tv.Runtime.HasValue);
            Assert.NotZero(tv.Runtime.Value);

            Assert.NotNull(tv.Genres);
            Assert.That(tv.Genres.Contains("Crime"));

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

        #region Seasons
        [TestCase(Category = TVDB_TESTS)]
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
        [TestCase(Category = TVDB_TESTS)]
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
        [TestCase(Category = TVDB_TESTS)]
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
        [TestCase(Category = TVDB_TESTS)]
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
        [TestCase(Category = TVDB_TESTS)]
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
        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Crew()
        {
            var list = await _tvdb.GetCrewAsync(SHOW_URL);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Id > 0));
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.All(x => x.Images.Count <= 0));
        }
        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Cast()
        {
            var list = await _tvdb.GetCastAsync(SHOW_URL);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Id > 0));
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.All(x => x.Images.Count > 0));
            Log(list.Count());
        }
        [TestCase(Category = TVDB_TESTS)]
        public async Task Get_Both_CastAndCrew()
        {
            var list = await _tvdb.GetCastAndCrewAsync(SHOW_URL);

            Assert.NotNull(list);
            Assert.That(list.Any());
            Assert.That(list.All(x => x.Id > 0));
            Assert.That(list.All(x => x.Role.IsValid()));
            Assert.That(list.All(x => x.Name.IsValid()));
            Assert.That(list.Any(x => x.Images.Count > 0));
            list.ForEach(x => Log(x));
        }
        #endregion

        #region Artworks
        [TestCase(Category = TVDB_TESTS)]
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