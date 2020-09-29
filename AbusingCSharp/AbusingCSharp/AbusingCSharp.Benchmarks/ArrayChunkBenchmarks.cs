using AbusingCSharp.Library;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbusingCSharp.Benchmarks
{
    /*
    |         Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated | Code Size |
    |--------------- |----------:|----------:|----------:|------:|--------:|------:|------:|------:|----------:|----------:|
    | TestIndexerRef | 0.4254 ns | 0.0433 ns | 0.0464 ns |  1.00 |    0.00 |     - |     - |     - |         - |      33 B |
    |    TestItemRef | 0.2497 ns | 0.0261 ns | 0.0244 ns |  0.59 |    0.10 |     - |     - |     - |         - |      37 B |
    |   TestItemRef2 | 1.4914 ns | 0.0204 ns | 0.0171 ns |  3.50 |    0.39 |     - |     - |     - |         - |      44 B |
    |   TestItemRef3 | 0.0942 ns | 0.0194 ns | 0.0172 ns |  0.22 |    0.04 |     - |     - |     - |         - |      19 B |
    */
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    public class ArrayChunkBenchmarks
    {
        private ArrayChunkOfStructs<int> _arrayChunk;

        [GlobalSetup]
        public void Setup()
        {
            _arrayChunk = new ArrayChunkOfStructs<int>(16);
        }

        [Benchmark(Baseline = true)]
        public ref int TestIndexerRef() => ref _arrayChunk[8];

        [Benchmark]
        public ref int TestItemRef() => ref _arrayChunk.ItemRef(8);

        [Benchmark]
        public ref int TestItemRef2() => ref _arrayChunk.ItemRef2(8);

        [Benchmark]
        public ref int TestItemRef3() => ref _arrayChunk.ItemRef3(8);

    }
}
