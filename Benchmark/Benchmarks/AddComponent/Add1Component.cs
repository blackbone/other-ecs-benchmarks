using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.AddComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Add1Component<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class Add1Component<T> : AddComponentBase<T> where T : BenchmarkContextBase, new()
{
    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1>(0);
    }

    protected override void OnCleanup()
    {
        base.OnCleanup();
        Context.RemoveComponent<Component1>(EntitySet, 0);
    }

    [Benchmark]
    public void UseCache()
    {
        Context.Lock();
        Context.AddComponent<Component1>(EntitySet, 0);
        Context.Commit();
    }

    [Benchmark]
    public void NoCache()
    {
        Context.Lock();
        Context.AddComponent<Component1>(EntitySet);
        Context.Commit();
    }
}