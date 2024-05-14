using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[BenchmarkCategory(Categories.StructuralChanges)]
[ArtifactsPath(".benchmark_results/" + nameof(RemoveFourComponents<T>))]
[MemoryDiagnoser]
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
public class RemoveFourComponents<T> : RemoveComponentBase<T> where T : BenchmarkContextBase, new()
{
    [Benchmark]
    public void Run()
    {
        Context.Warmup<Component1, Component2, Component3, Component4>(0);
        Context.RemoveComponent<Component1, Component2, Component3, Component4>(entityIds, 0);
        Context.Commit();
    }
}