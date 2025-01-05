using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEmptyEntity<T, TE>))]
[MemoryDiagnoser]

#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class CreateEmptyEntity<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    public T Context { get; set; }
    private TE[] _entitySet;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();
        Context.FinishSetup();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _entitySet =  Context.PrepareSet(EntityCount);
    }

    [Benchmark]
    public void Run()
    {
        Context.CreateEntities(_entitySet);
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        Context.DeleteEntities(_entitySet);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        Context.Cleanup();
        Context.Dispose();
        Context = default;
    }
}
