using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[ArtifactsPath(".benchmark_results/" + nameof(Add2Components<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
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

    [Benchmark]
    public void UseCache()
    {
        Context.Lock();
        Context.AddComponent<Component1, Component2>(EntityIds, 0);
        Context.Commit();
    }

    [Benchmark]
    public void NoCache()
    {
        Context.Lock();
        Context.AddComponent<Component1, Component2>(EntityIds);
        Context.Commit();
    }
}