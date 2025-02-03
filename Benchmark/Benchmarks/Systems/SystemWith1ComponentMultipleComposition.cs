using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(SystemWith1ComponentMultipleComposition<T, TE>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class SystemWith1ComponentMultipleComposition<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
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
        Context.Warmup<Padding2>(2);
        Context.Warmup<Padding3>(3);
        Context.Warmup<Padding4>(4);

        unsafe
        {
            // set up systems
            Context.AddSystem<Component1>(&Update, 0);
        }

        Context.FinishSetup();

        set = Context.PrepareSet(1);
        // set up entities
        for (var _i = 0; _i < EntityCount; ++_i)
        {
            for (var j = 0; j < Padding; ++j)
                switch (_i % 4)
                {
                    case 0:
                        Context.CreateEntities<Padding1>(set, 1, default(Padding1));
                        break;
                    case 2:
                        Context.CreateEntities<Padding2>(set, 2, default(Padding2));
                        break;
                    case 3:
                        Context.CreateEntities<Padding3>(set, 3, default(Padding3));
                        break;
                    case 4:
                        Context.CreateEntities<Padding4>(set, 4, default(Padding4));
                        break;
                }

            {
                Context.CreateEntities<Component1>(set, 0, new Component1 { Value = 0 });
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
