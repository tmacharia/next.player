using NUnit.Framework;

namespace Tests.Attributes
{
    sealed class ComboCaseAttribute : TestCaseSourceAttribute
    {
        public ComboCaseAttribute(string sourceName)
            : base(sourceName)
        { }
        public ComboCaseAttribute(string sourceName, string category)
            : base(sourceName)
        {
            Category = category;
        }
        public ComboCaseAttribute(string sourceName, params string[] traits)
            : base(sourceName)
        {
            if (traits != null && traits.Length > 0)
                Category = string.Join(',', traits);
        }
    }
    sealed class TheoriesFromAttribute : TestCaseSourceAttribute
    {
        public TheoriesFromAttribute(string sourceName)
            : base(sourceName)
        { }
        public TheoriesFromAttribute(string sourceName, params string[] traits)
            : this(sourceName)
        {
            if (traits != null && traits.Length > 0)
                Category = string.Join(',', traits);
        }
    }
}