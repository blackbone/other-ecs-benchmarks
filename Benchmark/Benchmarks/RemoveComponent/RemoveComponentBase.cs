using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

public abstract class RemoveComponentBase<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    protected int[] EntityIds;

    [Params(true, false)] public bool Random { get; set; }

    protected override void OnSetup()
    {
        EntityIds = new int[EntityCount];
    }
}