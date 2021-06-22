using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace Tests
{
    class TestsRoot : TestVariables
    {
        internal virtual void Log<T>(IEnumerable<T> ts) => ts.ForEach(x => Log(x));
        internal virtual void Log(object o)
        {
            Debug.WriteLine(o);
            Console.WriteLine(o);
        }
        internal virtual void Log(string format, params object[] args)
        {
            Debug.WriteLine(format, args);
            Console.WriteLine(format, args);
        }
    }
}