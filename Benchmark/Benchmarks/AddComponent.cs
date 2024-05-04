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
        for (var i = 0; i < EntityCount; i++)
            Context.RemoveComponent<Component1, Component2, Component3, Component4>(entityIds[i]);
    }

    [Benchmark]
    public void AddOneComponent()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.AddComponent<Component1>(entityIds[i]);
        Context.Commit();
    }
    
    [Benchmark]
    public void AddTwoComponent()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.AddComponent<Component1, Component2>(entityIds[i]);
        Context.Commit();
    }
    
    [Benchmark]
    public void AddThreeComponent()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.AddComponent<Component1, Component2, Component3>(entityIds[i]);
        Context.Commit();
    }
    
    [Benchmark]
    public void AddFourComponent()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.AddComponent<Component1, Component2, Component3, Component4>(entityIds[i]);
        Context.Commit();
    }
}