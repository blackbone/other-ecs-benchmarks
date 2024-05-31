using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.DeleteEntity;

[ArtifactsPath(".benchmark_results/" + nameof(DeleteEntityBenchmark<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class DeleteEntityBenchmark<T> : IBenchmark<T> where T : IBenchmarkContext
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    public T Context { get; set; }
    private Array _entitySet;

    [IterationSetup]
    public void Setup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();
        _entitySet = Context.PrepareSet(EntityCount);
        Context.Lock();
        Context.CreateEntities(_entitySet);
        Context.Commit();
        Context.FinishSetup();
    }

    [IterationCleanup]
    public void Cleanup()
    {
        Context?.DeleteEntities(_entitySet);
        Context?.Cleanup();
        Context?.Dispose();
        Context = default;
    }

    [Benchmark]
    public void Run()
    {
        Context?.Lock();
        Context?.DeleteEntities(_entitySet);
        Context?.Commit();

        // TIP: this needed to prevent deleting of deleted entities because some framework crashes on it
        _entitySet = Context.PrepareSet(0);
    }
}