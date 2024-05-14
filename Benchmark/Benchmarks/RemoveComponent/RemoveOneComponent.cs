using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[BenchmarkCategory(Categories.StructuralChanges)]
[ArtifactsPath(".benchmark_results/" + nameof(RemoveOneComponent<T>))]
[MemoryDiagnoser]
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
public class RemoveOneComponent<T> : RemoveComponentBase<T> where T : BenchmarkContextBase, new()
{
    [Benchmark]
    public void Run()
    {
        Context.Warmup<Component1>(0);
        Context.RemoveComponent<Component1>(entityIds, 0);
        Context.Commit();
    }
}