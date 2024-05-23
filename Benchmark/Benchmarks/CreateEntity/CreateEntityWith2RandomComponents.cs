using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.CreateEntity;

[ArtifactsPath(".benchmark_results/" + nameof(CreateEntityWith2RandomComponents<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class CreateEntityWith2RandomComponents<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    private object _entitySet;
    private Random _rnd;
    
    [Params(1, 4, 32)] public int ChunkSize { get; set; }

    protected override void OnSetup()
    {
        base.OnSetup();
        _entitySet = Context.PrepareSet(ChunkSize);
        Context.Warmup<Component1, Component2>(0);
        Context.Warmup<Component2, Component3>(1);
        Context.Warmup<Component3, Component4>(2);
        Context.Warmup<Component4, Component1>(3);
        _rnd = new Random(Constants.Seed);
    }

    [Benchmark]
    public override void Run()
    {
        Context.Lock();
        for (int i = 0; i < EntityCount; i += ChunkSize)
            switch (_rnd.Next() % 4)
            {
                case 0: Context.CreateEntities<Component1, Component2>(_entitySet, 0); break;
                case 1: Context.CreateEntities<Component2, Component3>(_entitySet, 1); break;
                case 2: Context.CreateEntities<Component3, Component4>(_entitySet, 2); break;
                case 3: Context.CreateEntities<Component4, Component1>(_entitySet, 3); break;
            }
            
        Context.Commit();
    }
}