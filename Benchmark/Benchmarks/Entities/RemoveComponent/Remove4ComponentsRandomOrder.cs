using System;
using Benchmark.Context;
using Benchmark.Utils;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.RemoveComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Remove4ComponentsRandomOrder<T, TE>))]
[MemoryDiagnoser]

#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class Remove4ComponentsRandomOrder<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    public T Context { get; set; }
    private TE[] _entitySet;
    

    [GlobalSetup]
    public void GlobalSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();
        Context.Warmup<Component1, Component2, Component3, Component4>(0);
        _entitySet =  Context.PrepareSet(EntityCount);
        Context.FinishSetup();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        Context.CreateEntities<Component1, Component2, Component3, Component4>(_entitySet, 0, default(Component1), default(Component2), default(Component3), default(Component4));
        _entitySet.Shuffle();
    }

    [Benchmark]
    public void Run()
    {
        Context.RemoveComponent<Component1, Component2, Component3, Component4>(_entitySet, 0);
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        if (!Context.DeletesEntityOnLastComponentDeletion)
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
