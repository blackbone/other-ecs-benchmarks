using Benchmark.Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntityWith1RandomComponent<T, TE>))]
[MemoryDiagnoser]

#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class CreateEntityWith1RandomComponent<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    public T Context { get; set; }
    private TE[] _entitySet;
    private TE[] _tmp;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();
        _entitySet =  Context.PrepareSet(EntityCount);
        _tmp = Context.PrepareSet(1);
        Context.Warmup<Component1>(0);
        Context.Warmup<Component2>(1);
        Context.Warmup<Component3>(2);
        Context.Warmup<Component4>(3);
        Context.FinishSetup();
    }

    [Benchmark]
    public void Run()
    {
        for (int _i = 0; _i < _entitySet.Length; _i++) {
            _tmp[0] = _entitySet[_i];
            switch (ArrayExtensions.Rnd.Next() % 4)
            {
                case 0:
                    Context.CreateEntities<Component1>(_tmp, 0, default(Component1));
                    break;
                case 1:
                    Context.CreateEntities<Component2>(_tmp, 1, default(Component2));
                    break;
                case 2:
                    Context.CreateEntities<Component3>(_tmp, 2, default(Component3));
                    break;
                case 3:
                    Context.CreateEntities<Component4>(_tmp, 3, default(Component4));
                    break;
            }
            _entitySet[_i] = _tmp[0];
        }
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        Context.DeleteEntities(_entitySet);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        Context.Cleanup();
        Context.Dispose();
        Context = default;
    }
}
