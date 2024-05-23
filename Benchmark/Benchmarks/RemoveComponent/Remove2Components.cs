using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.RemoveComponent;

[BenchmarkCategory(Categories.PerInvocationSetup)]
[ArtifactsPath(".benchmark_results/" + nameof(Remove2Components<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class Remove2Components<T> : RemoveComponentBase<T> where T : BenchmarkContextBase, new()
{
    private object _entitySet;

    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1, Component2>(0);
        _entitySet = Context.PrepareSet(EntityCount);
        Context.CreateEntities<Component1, Component2>(_entitySet, 0);
        if (Random) _entitySet = Context.Shuffle(_entitySet);
    }

    [Benchmark]
    public override void Run()
    {
        Context.Lock();
        Context.RemoveComponent<Component1, Component2>(_entitySet, 0);
        Context.Commit();
    }
}