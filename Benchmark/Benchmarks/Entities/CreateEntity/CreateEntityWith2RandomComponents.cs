using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntityWith2RandomComponents<T>))]
[MemoryDiagnoser]

#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class CreateEntityWith2RandomComponents<T> : IBenchmark<T> where T : IBenchmarkContext
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    [Params(1, 4, 32)] public int ChunkSize { get; set; }
    public T Context { get; set; }
    private Array _entitySet;
    private Array _tmp;
    

    [GlobalSetup]
    public void GlobalSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();
        _entitySet = Context.PrepareSet(EntityCount);
        _tmp = Context.PrepareSet(ChunkSize);
        Context.Warmup<Component1, Component2>(0);
        Context.Warmup<Component2, Component3>(1);
        Context.Warmup<Component3, Component4>(2);
        Context.Warmup<Component4, Component1>(3);
        Context.FinishSetup();
    }

    [Benchmark]
    public void Run() {
        for (var i = 0; i < _entitySet.Length; i += ChunkSize) {
            var count = Math.Min(ChunkSize, _entitySet.Length - i);
            Array.Copy(_entitySet, 0, _tmp, 0, count);

            switch (ArrayExtensions.Rnd.Next() % 4) {
                case 0:
                    Context.CreateEntities<Component1, Component2>(_tmp, 0);
                    break;
                case 1:
                    Context.CreateEntities<Component2, Component3>(_tmp, 1);
                    break;
                case 2:
                    Context.CreateEntities<Component3, Component4>(_tmp, 2);
                    break;
                case 3:
                    Context.CreateEntities<Component4, Component1>(_tmp, 3);
                    break;
            }
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
