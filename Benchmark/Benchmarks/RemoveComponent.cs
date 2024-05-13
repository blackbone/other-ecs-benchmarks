using Benchmark._Context;
using Benchmark.Utils;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[BenchmarkCategory(Categories.StructuralChanges)]
[ArtifactsPath(".benchmark_results/" + nameof(RemoveComponent<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class RemoveComponent<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    private int[] entityIds;
    
    [Params(false, true)] public bool Shuffled { get; set; }

    protected override void OnSetup()
    {
        entityIds = new int[EntityCount];
        for (var i = 0; i < EntityCount; i++)
            entityIds[i] = Context.CreateEntity<Component1, Component2, Component3, Component4>();
        
        if (Shuffled) entityIds.Shuffle();
    }

    [Benchmark]
    public void RemoveOneComponent()
    {
        Context.Warmup<Component1>(0);
        Context.RemoveComponent<Component1>(entityIds, 0);
        Context.Commit();
    }
    
    [Benchmark]
    public void RemoveTwoComponent()
    {
        Context.Warmup<Component1, Component2>(0);
        Context.RemoveComponent<Component1, Component2>(entityIds, 0);
        Context.Commit();
    }
    
    [Benchmark]
    public void RemoveThreeComponent()
    {
        Context.Warmup<Component1, Component2, Component3>(0);
        Context.RemoveComponent<Component1, Component2, Component3>(entityIds, 0);
        Context.Commit();
    }
    
    [Benchmark]
    public void RemoveFourComponent()
    {
        Context.Warmup<Component1, Component2, Component3, Component4>(0);
        Context.RemoveComponent<Component1, Component2, Component3, Component4>(entityIds, 0);
        Context.Commit();
    }
}