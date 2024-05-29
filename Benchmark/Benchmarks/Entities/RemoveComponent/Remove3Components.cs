using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.RemoveComponent;

[BenchmarkCategory(Categories.PerInvocationSetup)]
[ArtifactsPath(".benchmark_results/" + nameof(Remove3Components<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class Remove3Components<T> : IBenchmark<T> where T : struct, IBenchmarkContext
{
    [Params(true, false)] public bool Random { get; set; }
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    public T Context { get; set; }
    private Array _entitySet;

    [IterationSetup]
    public void Setup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();

        Context.Warmup<Component1, Component2, Component3>(0);
        _entitySet = Context.PrepareSet(EntityCount);
        Context.CreateEntities<Component1, Component2, Component3>(_entitySet, 0);
        if (Random) _entitySet = Context.Shuffle(_entitySet);
        Context.FinishSetup();
    }

    [IterationCleanup]
    public void Cleanup()
    {
        if (!Context.DeletesEntityOnLastComponentDeletion)
            Context.DeleteEntities(_entitySet);

        Context.Cleanup();
        Context.Dispose();
        Context = default;
    }

    [Benchmark]
    public void Run()
    {
        Context.Lock();
        Context.RemoveComponent<Component1, Component2, Component3>(_entitySet, 0);
        Context.Commit();
    }
}