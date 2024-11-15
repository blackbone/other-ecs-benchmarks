using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.DeleteEntity;

[ArtifactsPath(".benchmark_results/" + nameof(DeleteEntity<T>))]
[MemoryDiagnoser]

#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class DeleteEntity<T> : IBenchmark<T> where T : IBenchmarkContext
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    public T Context { get; set; }
    private Array _entitySet;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();
        _entitySet = Context.PrepareSet(EntityCount);
        Context.FinishSetup();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        Context.CreateEntities(_entitySet);
    }

    [Benchmark]
    public void Run()
    {
        Context.DeleteEntities(_entitySet);

        _entitySet = Context.PrepareSet(0);
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