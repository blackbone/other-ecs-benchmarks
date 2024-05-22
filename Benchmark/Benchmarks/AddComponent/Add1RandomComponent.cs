using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.AddComponent;

[ArtifactsPath(".benchmark_results/" + nameof(Add1RandomComponent<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public class Add1RandomComponent<T> : AddRandomComponentBase<T> where T : BenchmarkContextBase, new()
{
    private Random _rnd;
    
    protected override void OnSetup()
    {
        base.OnSetup();
        Context.Warmup<Component1>(0);
        Context.Warmup<Component2>(1);
        Context.Warmup<Component3>(2);
        Context.Warmup<Component4>(3);
        _rnd = new Random(Constants.Seed);
    }

    [Benchmark]
    public void _()
    {
        Context.Lock();
        for (int i = 0; i < EntitySets.Length; i += ChunkSize)
            switch (_rnd.Next() % 4)
            {
                case 0: Context.AddComponent<Component1>(EntitySets[i], 0); break;
                case 1: Context.AddComponent<Component2>(EntitySets[i], 1); break;
                case 2: Context.AddComponent<Component3>(EntitySets[i], 2); break;
                case 3: Context.AddComponent<Component4>(EntitySets[i], 3); break;
            }
            
        Context.Commit();
    }
}