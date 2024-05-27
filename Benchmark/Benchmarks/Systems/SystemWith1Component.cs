using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(SystemWith1Component<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
// ReSharper disable once InconsistentNaming
public class SystemWith1Component<T> : SystemBenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Params(0, 10)] public int Padding { get; set; }
    [Params(100)] public int Iterations { get; set; }

    protected override void OnSetup()
    {
        base.OnSetup();

        Context.Warmup<Component1>(0);
        
        var set = Context.PrepareSet(1);
        Context.Lock();
        // set up entities
        for (var i = 0; i < EntityCount; ++i)
        {
            for (var j = 0; j < Padding; ++j)
                Context.CreateEntities(set);
            
            Context.CreateEntities(set, 0, new Component1 { Value = 0 });
        }
        Context.Commit();

        unsafe
        {
            // set up systems
            Context.AddSystem<Component1>(&Update, 0);
        }
    }

    private static void Update(ref Component1 c1) => c1.Value++;

    [Benchmark]
    public override void Run()
    {
        var i = Iterations;
        while (i-- > 0) Context.Tick(0.1f);
    }
}