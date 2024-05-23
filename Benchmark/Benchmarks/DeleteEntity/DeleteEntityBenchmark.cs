using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.DeleteEntity;

[ArtifactsPath(".benchmark_results/" + nameof(DeleteEntityBenchmark<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class DeleteEntityBenchmark<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    private object _entitySet;

    protected override void OnSetup()
    {
        base.OnSetup();
        _entitySet = Context.PrepareSet(EntityCount);
        
        Context.Lock();
        Context.CreateEntities(_entitySet);
        Context.Commit();
    }

    [Benchmark]
    public override void Run()
    {
        Context.Lock();
        Context.DeleteEntities(_entitySet);
        Context.Commit();
    }
}