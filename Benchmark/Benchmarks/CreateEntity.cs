using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntity<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
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
        Context.Warmup<Component1>(0);
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1>(0); 
        Context.Commit();
    }
    
    [Benchmark]
    public void CreateEntitiesWithTwoComponent()
    {
        Context.Warmup<Component1, Component2>(0);
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1, Component2>(0);
        Context.Commit();
    }
    
    [Benchmark]
    public void CreateEntitiesWithThreeComponent()
    {
        Context.Warmup<Component1, Component2, Component3>(0);
        for (var i = 0; i < EntityCount; i++) 
            Context.CreateEntity<Component1, Component2, Component3>(0);
        Context.Commit();
    }
    
    [Benchmark]
    public void CreateEntitiesWithFourComponent()
    {
        Context.Warmup<Component1, Component2, Component3, Component4>(0);
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1, Component2, Component3, Component4>(0);
        Context.Commit();
    }
}