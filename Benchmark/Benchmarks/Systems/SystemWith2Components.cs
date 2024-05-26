using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(SystemWith2Components<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
// ReSharper disable once InconsistentNaming
public class SystemWith2Components<T> : SystemBenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Params(0, 10)] public int Padding { get; set; }

    protected override void OnSetup()
    {
        base.OnSetup();

        Context.Warmup<Component1>(0);
        Context.Warmup<Component2>(1);
        Context.Warmup<Component1, Component2>(2);
        
        var set = Context.PrepareSet(1);
        Context.Lock();
        // set up entities
        for (var i = 0; i < EntityCount; ++i)
        {
            for (var j = 0; j < Padding; ++j)
            {
                switch (j % 2)
                {
                    case 0:
                        Context.CreateEntities<Component1>(set, 0);
                        break;

                    case 1:
                        Context.CreateEntities<Component2>(set, 1);
                        break;
                }
            }
            Context.CreateEntities(set, 2, default(Component1), new Component2 { Value = 1 });
        }
        Context.Commit();

        unsafe
        {
            // set up systems
            Context.AddSystem<Component1, Component2>(&Update, 2);
        }
    }

    private static void Update(ref Component1 c1, ref Component2 c2) => c1.Value += c2.Value;

    [Benchmark]
    public override void Run()
    {
        Context.Tick(0.1f);
    }
}