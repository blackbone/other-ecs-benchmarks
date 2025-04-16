using Benchmark.Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(SystemWith3Components<T, TE>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class SystemWith3Components<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
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

        Context.Warmup<Component1, Component2, Component3>(0);
        Context.Warmup<Component1>(1);
        Context.Warmup<Component2>(2);
        Context.Warmup<Component3>(3);

        unsafe
        {
            // set up systems
            Context.AddSystem<Component1, Component2, Component3>(&Update, 0);
        }

        Context.FinishSetup();

        set = Context.PrepareSet(1);
        var _i = 0;
        while (_i < EntityCount) {
            {
                Context.CreateEntities<Component1, Component2, Component3>(set, 0, default(Component1), new Component2 { Value = 1 }, new Component3 { Value = 1 });
                _i++;
            }

            for (var j = 0; j < Padding; ++j) {
                switch (j % 3) {
                    case 0: Context.CreateEntities<Component1>(set, 1, default(Component1)); break;
                    case 1: Context.CreateEntities<Component2>(set, 2, default(Component2)); break;
                    case 2: Context.CreateEntities<Component3>(set, 3, default(Component3)); break;
                }
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

    private static void Update(ref Component1 c1, ref Component2 c2, ref Component3 c3)
    {
        c1.Value += c2.Value + c3.Value;
    }
}
