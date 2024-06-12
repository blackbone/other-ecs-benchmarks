using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.RemoveComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Remove2Components<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class Remove2Components<T> : IBenchmark<T> where T : class, IBenchmarkContext
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    public T Context { get; set; }
    private Array _entitySet;

    [IterationSetup]
    public void Setup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context?.Setup();

        Context?.Warmup<Component1, Component2>(0);
        _entitySet = Context?.PrepareSet(EntityCount);
        Context?.CreateEntities<Component1, Component2>(_entitySet, 0);
        Context?.FinishSetup();
    }

    [IterationCleanup]
    public void Cleanup()
    {
        if (!Context.DeletesEntityOnLastComponentDeletion)
            Context?.DeleteEntities(_entitySet);

        Context?.Cleanup();
        Context?.Dispose();
        Context = default;
    }

    [Benchmark]
    public void Run()
    {
        Context?.Lock();
        Context?.RemoveComponent<Component1, Component2>(_entitySet, 0);
        Context?.Commit();
    }
}