using System;
using System.Linq;
using Common;
using Next.PCL.Online.Models;
using NUnit.Framework;
using Tests.Attributes;
using Tests.TestModels;

namespace Tests.Converters
{
    [Order(2)]
    [TestFixture]
    class JsonConverterTests : TestsBase
    {
        [TestCase(Category = UNIT_TESTS)]
        public void OmdbRuntime()
        {
            string json = "{\"Runtime\":\"85 mins\"}";
            var model = json.DeserializeTo<OmdbModel>();
            Assert.NotNull(model);
            Assert.True(model.Runtime.HasValue);
            Assert.AreEqual(85, model.Runtime.Value);
        }
        [TestCase(Category = UNIT_TESTS)]
        public void StringToURI()
        {
            string url = "https://google.com";
            string json = "{\"Poster\":\"" + url + "\"}";
            var model = json.DeserializeTo<OmdbModel>();
            Assert.NotNull(model);
            Assert.NotNull(model.Poster);
            Assert.AreEqual(url, model.Poster.OriginalString);
        }
        [TheoriesFrom(nameof(ReleaseDates), UNIT_TESTS)]
        public void StringToDateTime(DateTimeFormat dtf)
        {
            string json = "{\"ReleaseDate\":\"" + dtf.DateTime + "\"}";
            var model = json.DeserializeTo<BaseOnlineModel>();
            Assert.NotNull(model);
            Assert.True(model.ReleaseDate.HasValue);
            Assert.AreEqual(dtf.DateTime, model.ReleaseDate.Value.ToString(dtf.Format));
        }
        [TheoriesFrom(nameof(Genres), UNIT_TESTS)]
        public void StringToList(string s)
        {
            string json = "{\"Genre\":\"" + s + "\"}";
            var model = json.DeserializeTo<OmdbModel>();
            Assert.NotNull(model);
            Assert.That(model.Genres.Any());
        }
        [TheoriesFrom(nameof(MetaScores), UNIT_TESTS)]
        public void StringToInt(Tuple<string, int> tuple)
        {
            string json = "{\"Metascore\":\"" + tuple.Item1 + "\"}";
            var model = json.DeserializeTo<OmdbModel>();
            Assert.NotNull(model);
            Assert.AreEqual(tuple.Item2, model.Metascore);
        }
    }
}