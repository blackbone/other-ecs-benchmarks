using System;
using Benchmark._Context;
using Benchmark.Utils;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.AddComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Add2RandomComponents<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class Add2RandomComponents<T> : IBenchmark<T> where T : IBenchmarkContext
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }
    public T Context { get; set; }
    private Array[] _entitySets;
    private Random _rnd;

    [IterationSetup]
    public void IterationSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context?.Setup();
        _entitySets = new Array[EntityCount];
        for (var i = 0; i < EntityCount; i++)
        {
            _entitySets[i] = Context?.PrepareSet(1);
            Context?.CreateEntities(_entitySets[i]);
        }

        Context?.Warmup<Component1, Component2>(0);
        Context?.Warmup<Component2, Component3>(1);
        Context?.Warmup<Component3, Component4>(2);
        Context?.Warmup<Component4, Component1>(3);
        Context?.FinishSetup();

        _rnd = new Random(Constants.Seed);
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        for (var i = 0; i < EntityCount; i++)
            Context?.DeleteEntities(_entitySets[i]);

        Context?.Cleanup();
        Context?.Dispose();
        Context = default;
    }

    [Benchmark]
    public void Run()
    {
        for (var i = 0; i < _entitySets.Length; i++)
        {
            Context?.Lock();
            switch (_rnd.Next() % 4)
            {
                case 0:
                    Context?.AddComponent<Component1, Component2>(_entitySets[i], 0);
                    break;
                case 1:
                    Context?.AddComponent<Component2, Component3>(_entitySets[i], 1);
                    break;
                case 2:
                    Context?.AddComponent<Component3, Component4>(_entitySets[i], 2);
                    break;
                case 3:
                    Context?.AddComponent<Component4, Component1>(_entitySets[i], 3);
                    break;
            }

            Context?.Commit();
        }
    }
}