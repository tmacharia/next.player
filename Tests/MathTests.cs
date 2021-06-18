using Next.PCL.Extensions;
using NUnit.Framework;
using Tests.Attributes;
using Tests.TestModels;

namespace Tests
{
    class MathTests : TestsBase
    {
        [TheoriesFrom(nameof(MedianCases), UNIT_TESTS)]
        public void Calculate_Median(MedianTestModel<int> model)
        {
            int ans = MathExts.Median(model.Values);

            Assert.AreEqual(model.Median, ans);
        }
    }
}