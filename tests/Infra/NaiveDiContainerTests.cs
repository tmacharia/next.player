using System;
using Next.PCL.Infra;
using Next.PCL.Online;
using Next.PCL.Services;
using NUnit.Framework;
using Tests.Attributes;

namespace Tests.Infra
{
    [TestFixture]
    class NaiveDiContainerTests : TestsRoot
    {
        private readonly NaiveDIContainer _di;

        public NaiveDiContainerTests()
        {
            _di = new NaiveDIContainer();
        }
        [Order(0)]
        [Case(UNIT_TESTS)]
        public void No_Ctor()
        {
            var val = _di.CreateInstanceOf<SearchQueryFormatter>();

            Assert.IsNotNull(val);
        }

        [Order(0)]
        [Case(UNIT_TESTS)]
        public void Paramless_Ctor()
        {
            var cache = _di.CreateInstanceOf<NaiveMemoryCache>();

            cache.TryAdd("kb", 1024);
            var val = cache.Get<int>("kb");

            Assert.AreEqual(1024, val);
        }

        [Order(0)]
        [Case(UNIT_TESTS)]
        public void Abstract_Class()
        {
            Assert.Throws<Exception>(() => _di.CreateInstanceOf<BaseService>());
        }

        [Order(0)]
        [Case(UNIT_TESTS)]
        public void OneArg_Optional_Ctor()
        {
            var val = _di.CreateInstanceOf<HttpOnlineClient>();

            Assert.IsNotNull(val);
        }

        [Order(1)]
        [Case(UNIT_TESTS)]
        public void AddService_A0A()
        {
            _di.AddSingleton<INaiveCache, NaiveMemoryCache>();
            // silently ignore existing service.
            _di.AddSingleton<INaiveCache, NaiveMemoryCache>();

            _di.AddSingleton<IHttpOnlineClient, HttpOnlineClient>();
        }

        [Order(2)]
        [Case(UNIT_TESTS)]
        public void GetService_A0A()
        {
            var A1 = _di.GetService<INaiveCache>();
            var A2 = _di.GetService<IHttpOnlineClient>();

            Assert.IsNotNull(A1);
            Assert.IsNotNull(A2);

            A1.TryAdd("kb", 1024);
        }

        [Order(2)]
        [Case(UNIT_TESTS)]
        public void AddService_A0B()
        {
            _di.AddSingleton<Imdb>();
        }

        [Order(3)]
        [Case(UNIT_TESTS)]
        public void GetService_A0B()
        {
            var A1 = _di.GetService<Imdb>();

            Assert.IsNotNull(A1);
        }

        [Order(3)]
        [Case(UNIT_TESTS)]
        public void CrossCalls()
        {
            var A1 = _di.GetService<INaiveCache>();

            Assert.IsNotNull(A1);

            var val = A1.Get<int>("kb");

            Assert.AreEqual(1024, val);
        }
    }
}