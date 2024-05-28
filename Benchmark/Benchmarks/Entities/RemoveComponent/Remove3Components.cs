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
public class Remove3Components<T> : RemoveComponentBase<T> where T : BenchmarkContextBase, new()
{
    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1, Component2, Component3>(0);
        EntitySet = Context.PrepareSet(EntityCount);
        Context.CreateEntities<Component1, Component2, Component3>(EntitySet, 0);
        if (Random) EntitySet = Context.Shuffle(EntitySet);
    }

    [Benchmark]
    public override void Run()
    {
        Context.Lock();
        Context.RemoveComponent<Component1, Component2, Component3>(EntitySet, 0);
        Context.Commit();
    }
}