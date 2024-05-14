using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntityWithTwoComponents<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
public class CreateEntityWithTwoComponents<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Benchmark]
    public void CreateEntitiesWithTwoComponent()
    {
        Context.Warmup<Component1, Component2>(0);
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity<Component1, Component2>(0);
        Context.Commit();
    }
}