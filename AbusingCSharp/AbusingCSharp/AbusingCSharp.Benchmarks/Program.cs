using BenchmarkDotNet.Running;
using System;

namespace AbusingCSharp.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ArrayChunkBenchmarks>();
        }
    }
}
