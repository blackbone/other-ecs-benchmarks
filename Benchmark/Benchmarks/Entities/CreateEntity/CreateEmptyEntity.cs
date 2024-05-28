using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEmptyEntity<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class CreateEmptyEntity<T> : EntitiesBenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    private Array _entitySet;

    protected override void OnSetup()
    {
        base.OnSetup();
        _entitySet = Context.PrepareSet(EntityCount);
    }

    protected override void OnCleanup()
    {
        base.OnCleanup();
        Context.DeleteEntities(_entitySet);
    }

    [Benchmark]
    public override void Run()
    {
        Context.Lock();
        Context.CreateEntities(_entitySet);
        Context.Commit();
    }
}