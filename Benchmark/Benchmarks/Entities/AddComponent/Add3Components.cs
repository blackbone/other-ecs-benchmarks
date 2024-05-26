using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.AddComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Add3Components<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class Add3Components<T> : AddComponentBase<T> where T : BenchmarkContextBase, new()
{
    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1, Component2, Component3>(0);
    }

    protected override void OnCleanup()
    {
        base.OnCleanup();
        Context.RemoveComponent<Component1, Component2, Component3>(EntitySet, 0);
    }

    [Benchmark]
    public override void Run()
    {
        Context.Lock();
        Context.AddComponent<Component1, Component2, Component3>(EntitySet, 0);
        Context.Commit();
    }
}