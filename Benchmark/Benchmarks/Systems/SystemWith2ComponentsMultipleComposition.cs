using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(SystemWith2ComponentsMultipleComposition<T, TE>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class SystemWith2ComponentsMultipleComposition<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
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
        Context.Warmup<Component2>(1);
        Context.Warmup<Component1, Component2>(2);
        Context.Warmup<Padding1>(3);
        Context.Warmup<Padding2>(4);
        Context.Warmup<Padding3>(5);
        Context.Warmup<Padding4>(6);

        unsafe
        {
            // set up systems
            Context.AddSystem<Component1, Component2>(&Update, 2);
        }

        Context.FinishSetup();

        set = Context.PrepareSet(1);

        // set up entities
        for (var _i = 0; _i < EntityCount; ++_i)
        {
            for (var j = 0; j < Padding; ++j)
                switch (j % 2)
                {
                    case 0:
                        Context.CreateEntities<Component1>(set, 0, default(Component1));
                        break;
                    case 1:
                        Context.CreateEntities<Component2>(set, 1, default(Component2));
                        break;
                }

            {
                Context.CreateEntities<Component1, Component2>(set, 2, default(Component1), new Component2 { Value = 1 });
            }

            switch (_i % 4)
            {
                case 0:
                    Context.AddComponent<Padding1>(set, 3, default(Padding1));
                    break;
                case 2:
                    Context.AddComponent<Padding2>(set, 4, default(Padding2));
                    break;
                case 3:
                    Context.AddComponent<Padding3>(set, 5, default(Padding3));
                    break;
                case 4:
                    Context.AddComponent<Padding4>(set, 6, default(Padding4));
                    break;
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

    private static void Update(ref Component1 c1, ref Component2 c2)
    {
        c1.Value += c2.Value;
    }
}
