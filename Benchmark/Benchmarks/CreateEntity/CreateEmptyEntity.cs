using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEmptyEntity<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class CreateEmptyEntity<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    private object _entitySet;

    protected override void OnSetup()
    {
        base.OnSetup();
        _entitySet = Context.PrepareSet(EntityCount);
    }

    [Benchmark]
    public void _()
    {
        Context.Lock();
        Context.CreateEntities(_entitySet);
        Context.Commit();
    }
}