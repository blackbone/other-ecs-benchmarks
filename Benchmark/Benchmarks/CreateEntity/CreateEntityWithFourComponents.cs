using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntityWithFourComponents<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
public class CreateEntityWithFourComponents<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Benchmark]
    public void CreateEntitiesWithFourComponent()
    {
        Context.Warmup<Component1, Component2, Component3, Component4>(0);
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1, Component2, Component3, Component4>(0);
        Context.Commit();
    }
}