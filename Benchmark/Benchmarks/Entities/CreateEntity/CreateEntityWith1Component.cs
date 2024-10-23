using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntityWith1Component<T>))]
[MemoryDiagnoser]

#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class CreateEntityWith1Component<T> : IBenchmark<T> where T : IBenchmarkContext
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    public T Context { get; set; }
    private Array _entitySet;

    [GlobalSetup]
    public void GlobalSetup() {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context?.Setup();
        Context?.Warmup<Component1>(0);
        Context?.FinishSetup();

        _entitySet = Context?.PrepareSet(EntityCount);
    }

    [Benchmark]
    public void Run()
    {
        Context?.Lock();
        Context?.CreateEntities<Component1>(_entitySet, 0);
        Context?.Commit();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        Context?.DeleteEntities(_entitySet);
    }

    [GlobalCleanup]
    public void GlobalCleanup() {
        Context?.Cleanup();
        Context?.Dispose();
        Context = default;
    }
}