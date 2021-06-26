using System;

namespace Next.PCL.Exceptions
{
    public class ExpectationFailedException : NextPCLException
    {
        public string Expected { get; }
        public string Actual { get; }

        public ExpectationFailedException(string expected, string actual)
            : base(string.Format("Expected '{0}' but got '{1}'.", expected, actual))
        {
            Expected = expected;
            Actual = actual;
        }
        public ExpectationFailedException(Type expected, Type actual)
            : base(string.Format("Expected item of type '{0}' but got '{1}'.", expected.Name, actual.Name))
        {
            Expected = expected.Name;
            Actual = actual.Name;
        }
    }
}