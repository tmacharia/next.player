using System.Collections;
using Next.PCL.Extensions;
using NUnit.Framework;
using Tests.Attributes;
using Tests.TestModels;

namespace Tests
{
    [Order(0)]
    [TestFixture]
    class MathTests : TestsRoot
    {
        [TheoriesFrom(nameof(MedianCases), UNIT_TESTS)]
        public void Calculate_Median(MedianTestModel<int> model)
        {
            int ans = MathExts.Median(model.Values);

            Assert.AreEqual(model.Median, ans);
        }

        internal static IEnumerable MedianCases
        {
            get
            {
                yield return new MedianTestModel<int>(35, 10, 20, 30, 40, 50, 60);
                yield return new MedianTestModel<int>(35, 30, 20, 10, 60, 40, 50);
                yield return new MedianTestModel<int>(35, 60, 30, 20, 10, 40, 50);
            }
        }
    }
}