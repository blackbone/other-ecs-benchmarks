using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.AddComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Add1Component<T>))]
[MemoryDiagnoser]

#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class Add1Component<T> : IBenchmark<T> where T : IBenchmarkContext
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }

    public T Context { get; set; }
    private Array _entitySet;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context?.Setup();
        _entitySet = Context?.PrepareSet(EntityCount);
        Context?.Warmup<Component1>(0);
        Context?.FinishSetup();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        Context?.Lock();
        Context?.CreateEntities(_entitySet);
        Context?.Commit();
    }

    [Benchmark]
    public void Run()
    {
        Context?.Lock();
        Context?.AddComponent<Component1>(_entitySet, 0);
        Context?.Commit();
    }
    
    [IterationCleanup]
    public void IterationCleanup()
    {
        Context?.Lock();
        Context?.DeleteEntities(_entitySet);
        Context?.Commit();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        Context?.Cleanup();
        Context?.Dispose();
        Context = default;
    }
}