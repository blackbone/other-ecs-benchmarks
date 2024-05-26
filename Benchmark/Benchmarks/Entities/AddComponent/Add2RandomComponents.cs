using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.AddComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Add2RandomComponents<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class Add2RandomComponents<T> : AddRandomComponentBase<T> where T : BenchmarkContextBase, new()
{
    private Random _rnd;
    
    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1, Component2>(0);
        Context.Warmup<Component2, Component3>(1);
        Context.Warmup<Component3, Component4>(2);
        Context.Warmup<Component4, Component1>(3);
        _rnd = new Random(Constants.Seed);
    }

    [Benchmark]
    public override void Run()
    {
        for (var i = 0; i < EntitySets.Length; i += ChunkSize)
        {
            Context.Lock();
            switch (_rnd.Next() % 4)
            {
                case 0: Context.AddComponent<Component1, Component2>(EntitySets[i], 0); break;
                case 1: Context.AddComponent<Component2, Component3>(EntitySets[i], 1); break;
                case 2: Context.AddComponent<Component3, Component4>(EntitySets[i], 2); break;
                case 3: Context.AddComponent<Component4, Component1>(EntitySets[i], 3); break;
            }
            Context.Commit();
        }
    }
}