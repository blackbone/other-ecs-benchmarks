using System;
using Benchmark._Context;
using Benchmark.Utils;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.RemoveComponent;

[BenchmarkCategory(Categories.PerInvocationSetup)]
[ArtifactsPath(".benchmark_results/" + nameof(Remove1Component<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class Remove1Component<T> : RemoveComponentBase<T> where T : BenchmarkContextBase, new()
{
    private object _entitySet;
    
    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1>(0);
        _entitySet = Context.PrepareSet(EntityCount);
        Context.CreateEntities<Component1>(_entitySet, 0);
        if (Random) _entitySet = Context.Shuffle(_entitySet);
    }

    [Benchmark]
    public void _()
    {
        Context.Lock();
        Context.RemoveComponent<Component1>(_entitySet, 0);
        Context.Commit();
    }
}