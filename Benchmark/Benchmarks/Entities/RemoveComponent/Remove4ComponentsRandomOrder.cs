using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.RemoveComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Remove4ComponentsRandomOrder<T>))]
[MemoryDiagnoser]
[BenchmarkCategory(Categories.PerInvocationSetup)]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class Remove4ComponentsRandomOrder<T> : IBenchmark<T> where T : IBenchmarkContext
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    public T Context { get; set; }
    private Array _entitySet;

    [IterationSetup]
    public void IterationSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context?.Setup();

        Context?.Warmup<Component1, Component2, Component3, Component4>(0);
        _entitySet = Context?.PrepareSet(EntityCount);
        Context?.CreateEntities<Component1, Component2, Component3, Component4>(_entitySet, 0);
        _entitySet = Context?.Shuffle(_entitySet);
        Context?.FinishSetup();
    }

    [IterationCleanup]
    public void IterationCleanup()
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
        Context?.RemoveComponent<Component1, Component2, Component3, Component4>(_entitySet, 0);
        Context?.Commit();
    }
}