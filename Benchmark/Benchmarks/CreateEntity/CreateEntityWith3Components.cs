using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntityWith3Components<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class CreateEntityWith3Components<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    private object _entitySet;

    protected override void OnSetup()
    {
        base.OnSetup();
        _entitySet = Context.PrepareSet(EntityCount);
        Context.Warmup<Component1, Component2, Component3>(0);
    }

    [Benchmark]
    public void _()
    {
        Context.Lock();
        Context.CreateEntities<Component1, Component2, Component3>(_entitySet, 0);
        Context.Commit();
    }
}