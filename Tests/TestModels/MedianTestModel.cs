using System;

namespace Tests.TestModels
{
    struct MedianTestModel<T> where T : IComparable<T>
    {
        public MedianTestModel(T ans, params T[] vals)
        {
            Median = ans;
            Values = vals;
        }
        public T Median { get; set; }
        public T[] Values { get; set; }

        public override string ToString()
        {
            return string.Format("Ans: {0} | Params: {1}", Median, string.Join(',', Values));
        }
    }
}