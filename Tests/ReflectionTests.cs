using System.Collections.Generic;
using Tests.Attributes;
using TMDbLib.Objects.General;
using Next.PCL.Extensions;
using NUnit.Framework;

namespace Tests
{
    class ReflectionTests : TestsBase
    {
        TMDbConfig config;
        public ReflectionTests()
        {
            config = new TMDbConfig();
            config.Images = new ConfigImageTypes()
            {
                LogoSizes = new List<string>() { "32x32" }
            };
        }

        [Case(UNIT_TESTS)]
        public void Get_NestedInner_PropName()
        {
            string name = config.GetPropName(x => x.Images.LogoSizes);
            Assert.AreEqual("Images.LogoSizes", name);
            Log(name);
        }

        [Case(UNIT_TESTS)]
        public void Get_NestedInner_PropValue()
        {
            string name = config.GetPropName(x => x.Images.LogoSizes);
            Assert.AreEqual("Images.LogoSizes", name);
            Log(name);

            var obj = config.GetPropValue2(name);
            Assert.NotNull(obj);
            Log(obj);
        }

        [Case(UNIT_TESTS)]
        public void Get_NestedInner_PropValue2()
        {
            var obj = config.GetPropValue2(x => x.Images.LogoSizes);
            Assert.NotNull(obj);
            Log(obj);
        }
    }
}