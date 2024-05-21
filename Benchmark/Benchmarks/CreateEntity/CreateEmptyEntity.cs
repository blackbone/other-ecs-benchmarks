using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEmptyEntity<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class CreateEmptyEntity<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    private int[] _entityIds;

    protected override void OnSetup()
    {
        base.OnSetup();
        _entityIds = new int[EntityCount];
    }

    [Benchmark]
    public void _()
    {
        Context.Lock();
        Context.CreateEntities(_entityIds);
        Context.Commit();
    }
}