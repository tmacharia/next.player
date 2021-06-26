using BenchmarkDotNet.Running;
using System;

namespace Next.Console_
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Next.Player.Console!");
            BenchmarkRunner.Run<SearchQueryFormatterBenchmarks>();
        }
    }
}