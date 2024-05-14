using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[ArtifactsPath(".benchmark_results/" + nameof(AddFourComponents<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
public class AddFourComponents<T> : AddComponentBase<T> where T : BenchmarkContextBase, new()
{
    [Benchmark]
    public void Run()
    {
        Context.Warmup<Component1, Component2, Component3, Component4>(0);
        Context.AddComponent<Component1, Component2, Component3, Component4>(entityIds, 0);
        Context.Commit();
    }
}