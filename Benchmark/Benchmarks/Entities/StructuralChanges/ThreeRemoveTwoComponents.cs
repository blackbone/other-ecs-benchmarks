using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.StructuralChanges;

[ArtifactsPath(".benchmark_results/" + nameof(ThreeRemoveTwoComponents<T>))]
[MemoryDiagnoser]

#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class ThreeRemoveTwoComponents<T> : IBenchmark<T> where T : IBenchmarkContext
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
        Context.Warmup<Component1, Component2, Component3>(0);
        Context.Warmup<Component2, Component3>(1);
        Context.FinishSetup();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        Context.CreateEntities(_entitySet, 0, default(Component1), default(Component2), default(Component3));
    }

    [Benchmark]
    public void Run()
    {
        Context.RemoveComponent<Component2, Component3>(_entitySet, 1);
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