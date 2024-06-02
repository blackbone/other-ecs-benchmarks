using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.AddComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Add1ComponentRandomOrder<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class Add1ComponentRandomOrder<T> : IBenchmark<T> where T : IBenchmarkContext
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }

    public T Context { get; set; }
    private Array _entitySet;

    [IterationSetup]
    public void Setup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context?.Setup();
        _entitySet = Context?.PrepareSet(EntityCount);
        Context?.CreateEntities(_entitySet);
        _entitySet = Context?.Shuffle(_entitySet);
        Context?.Warmup<Component1>(0);
        Context?.FinishSetup();
    }

    [IterationCleanup]
    public void Cleanup()
    {
        Context?.RemoveComponent<Component1>(_entitySet, 0);
        Context?.DeleteEntities(_entitySet);
        Context?.Cleanup();
        Context?.Dispose();
        Context = default;
    }

    [Benchmark]
    public void Run()
    {
        Context?.Lock();
        Context?.AddComponent<Component1>(_entitySet, 0);
        Context?.Commit();
    }
}