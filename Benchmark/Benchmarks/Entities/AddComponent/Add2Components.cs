using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.AddComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Add2Components<T>))]
[MemoryDiagnoser]

#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class Add2Components<T> : IBenchmark<T> where T : IBenchmarkContext
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }

    public T Context { get; set; }
    private Array _entitySet;

    [IterationSetup]
    public void IterationSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context?.Setup();
        _entitySet = Context?.PrepareSet(EntityCount);
        Context?.CreateEntities(_entitySet);
        Context?.Warmup<Component1, Component2>(0);
        Context?.FinishSetup();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        Context?.RemoveComponent<Component1, Component2>(_entitySet, 0);
        Context?.DeleteEntities(_entitySet);
        Context?.Cleanup();
        Context?.Dispose();
        Context = default;
    }

    [Benchmark]
    public void Run()
    {
        Context?.Lock();
        Context?.AddComponent<Component1, Component2>(_entitySet, 0);
        Context?.Commit();
    }
}