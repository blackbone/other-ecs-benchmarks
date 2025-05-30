using Benchmark.Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(SystemWith1Component<T, TE>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class SystemWith1Component<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
{
    [Params(Constants.SystemEntityCount)] public int EntityCount { get; set; }
    [Params(0, 10)] public int Padding { get; set; }

    public T Context { get; set; }

    private TE[] set;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();
        Context.Warmup<Component1>(0);
        Context.Warmup<Padding1>(1);

        unsafe
        {
            // set up systems
            Context.AddSystem<Component1>(&Update, 0);
        }

        Context.FinishSetup();

        set = Context.PrepareSet(1);
        var _i = 0;
        while (_i < EntityCount) {
            {
                Context.CreateEntities<Component1>(set, 0, new Component1 { Value = 0 });
                _i++;
            }

            for (var j = 0; j < Padding; ++j) {
                Context.CreateEntities<Padding1>(set, 1, default(Padding1));
                ++_i;

                if (_i >= EntityCount) return;
            }
        }
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        Context.Cleanup();
        Context.Dispose();
        Context = default;
    }

    [Benchmark]
    public void Run()
    {
        Context.Tick(0.1f);
    }

    private static void Update(ref Component1 c1)
    {
        c1.Value++;
    }
}
