using Benchmark._Context;
using Benchmark.Utils;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[ArtifactsPath(".benchmark_results/" + nameof(AddComponent<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class AddComponent<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    private int[] entityIds;

    [Params(false, true)] public bool Shuffled { get; set; }
    
    protected override void OnSetup()
    {
        entityIds = new int[EntityCount];
        for (var i = 0; i < EntityCount; i++)
            entityIds[i] = Context.CreateEntity();
        
        if (Shuffled) entityIds.Shuffle();
    }

    protected override void OnCleanup()
    {
        Context.RemoveComponent<Component1, Component2, Component3, Component4>(entityIds);
    }

    [Benchmark]
    public void AddOneComponent()
    {
        Context.Warmup<Component1>(0);
        Context.AddComponent<Component1>(entityIds, 0);
        Context.Commit();
    }
    
    [Benchmark]
    public void AddTwoComponent()
    {
        Context.Warmup<Component1, Component2>(0);
        Context.AddComponent<Component1, Component2>(entityIds, 0);
        Context.Commit();
    }
    
    [Benchmark]
    public void AddThreeComponent()
    {
        Context.Warmup<Component1, Component2, Component3>(0);
        Context.AddComponent<Component1, Component2, Component3>(entityIds, 0);
        Context.Commit();
    }
    
    [Benchmark]
    public void AddFourComponent()
    {
        Context.Warmup<Component1, Component2, Component3, Component4>(0);
        Context.AddComponent<Component1, Component2, Component3, Component4>(entityIds, 0);
        Context.Commit();
    }
}