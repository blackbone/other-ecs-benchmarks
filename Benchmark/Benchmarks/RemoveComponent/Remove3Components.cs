using Benchmark._Context;
using Benchmark.Utils;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.RemoveComponent;

[BenchmarkCategory(Categories.PerInvocationSetup)]
[ArtifactsPath(".benchmark_results/" + nameof(Remove3Components<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class Remove3Components<T> : RemoveComponentBase<T> where T : BenchmarkContextBase, new()
{
    private object _entitySet;

    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1, Component2, Component3>(0);
        _entitySet = Context.PrepareSet(EntityCount);
        Context.CreateEntities<Component1, Component2, Component3>(_entitySet);
        if (Random) _entitySet = Context.Shuffle(_entitySet);
    }

    [Benchmark]
    public void UseCache()
    {
        Context.Lock();
        Context.RemoveComponent<Component1, Component2, Component3>(_entitySet, 0);
        Context.Commit();
    }

    [Benchmark]
    public void NoCache()
    {
        Context.Lock();
        Context.RemoveComponent<Component1, Component2, Component3>(_entitySet);
        Context.Commit();
    }
}