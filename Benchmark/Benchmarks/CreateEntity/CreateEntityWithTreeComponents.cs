using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntityWithTreeComponents<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
public class CreateEntityWithTreeComponents<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Benchmark]
    public void CreateEntitiesWithThreeComponent()
    {
        Context.Warmup<Component1, Component2, Component3>(0);
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1, Component2, Component3>(0);
        Context.Commit();
    }
}