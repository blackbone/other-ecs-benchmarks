using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[ArtifactsPath(".benchmark_results/" + nameof(AddOneComponent<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
public class AddOneComponent<T> : AddComponentBase<T> where T : BenchmarkContextBase, new()
{
    [Benchmark]
    public void Run()
    {
        Context.Warmup<Component1>(0);
        Context.AddComponent<Component1>(entityIds, 0);
        Context.Commit();
    }
}