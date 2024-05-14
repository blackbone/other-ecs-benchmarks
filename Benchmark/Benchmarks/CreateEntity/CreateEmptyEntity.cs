using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEmptyEntity<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
public class CreateEmptyEntity<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Benchmark]
    public void CreateEmptyEntities()
    {
        for (var i = 0; i < EntityCount; i++)
            Context.CreateEntity();
        Context.Commit();
    }
}