using Benchmark._Context;
using Benchmark.Utils;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

public abstract class AddComponentBase<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    protected int[] EntityIds;

    [Params(true, false)] public bool Random { get; set; }

    protected override void OnSetup()
    {
        EntityIds = new int[EntityCount];
        Context.CreateEntities(EntityIds);
        if (Random) EntityIds.Shuffle();
    }

    protected sealed override void OnCleanup()
    {
    }
}