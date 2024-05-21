using Benchmark._Context;
using Benchmark.Utils;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[BenchmarkCategory(Categories.StructuralChanges)]
[ArtifactsPath(".benchmark_results/" + nameof(Remove1Component<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class Remove1Component<T> : RemoveComponentBase<T> where T : BenchmarkContextBase, new()
{
    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1>(0);
        Context.CreateEntities<Component1>(EntityIds);
        if (Random) EntityIds.Shuffle();
    }

    [Benchmark]
    public void UseCache()
    {
        Context.Lock();
        Context.RemoveComponent<Component1>(EntityIds, 0);
        Context.Commit();
    }

    [Benchmark]
    public void NoCache()
    {
        Context.Lock();
        Context.RemoveComponent<Component1>(EntityIds);
        Context.Commit();
    }
}