using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntityWithFourComponents<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class CreateEntityWithFourComponents<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Benchmark]
    public void CreateEntitiesWithFourComponent()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1, Component2, Component3, Component4>(0);
        Context.Commit();
    }
    
    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1, Component2, Component3, Component4>(0);
    }
}