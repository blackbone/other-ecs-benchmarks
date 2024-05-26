using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.AddComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Add2Components<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class Add2Components<T> : AddComponentBase<T> where T : BenchmarkContextBase, new()
{
    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1, Component2>(0);
    }

    protected override void OnCleanup()
    {
        base.OnCleanup();
        Context.RemoveComponent<Component1, Component2>(EntitySet, 0);
    }

    [Benchmark]
    public override void Run()
    {
        Context.Lock();
        Context.AddComponent<Component1, Component2>(EntitySet, 0);
        Context.Commit();
    }
}