using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntityWith3Components<T>))]
[BenchmarkCategory(Categories.StructuralChanges)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class CreateEntityWith3Components<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    private int[] _entityIds;

    protected override void OnSetup()
    {
        base.OnSetup();
        _entityIds = new int[EntityCount];
        Context.Warmup<Component1, Component2, Component3>(0);
    }

    [Benchmark]
    public void UseCache()
    {
        Context.Lock();
        Context.CreateEntities<Component1, Component2, Component3>(_entityIds, 0);
        Context.Commit();
    }

    [Benchmark]
    public void NoCache()
    {
        Context.Lock();
        Context.CreateEntities<Component1, Component2, Component3>(_entityIds);
        Context.Commit();
    }
}