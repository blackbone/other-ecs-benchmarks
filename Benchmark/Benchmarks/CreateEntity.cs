using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntity<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
    [HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class CreateEntity<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Benchmark]
    public void CreateEmptyEntities()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity();
        Context.Commit();
    }
    
    [Benchmark]
    public void CreateEntitiesWithOneComponent()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1>(); 
        Context.Commit();
    }
    
    [Benchmark]
    public void CreateEntitiesWithTwoComponent()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1, Component2>();
        Context.Commit();
    }
    
    [Benchmark]
    public void CreateEntitiesWithThreeComponent()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1, Component2, Component3>();
        Context.Commit();
    }
    
    [Benchmark]
    public void CreateEntitiesWithFourComponent()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1, Component2, Component3, Component4>();
        Context.Commit();
    }
}