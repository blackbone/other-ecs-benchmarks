using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(SystemWith3ComponentsMultipleComposition<T, TE>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class SystemWith3ComponentsMultipleComposition<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
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
        Context.Warmup<Component3>(2);
        Context.Warmup<Component1, Component2, Component3>(3);
        Context.Warmup<Padding1>(4);
        Context.Warmup<Padding2>(5);
        Context.Warmup<Padding3>(6);
        Context.Warmup<Padding4>(7);
        set = Context.PrepareSet(1);

        // set up entities
        for (var _i = 0; _i < EntityCount; ++_i)
        {
            for (var j = 0; j < Padding; ++j)
                switch (j % 3)
                {
                    case 0:
                        Context.CreateEntities<Component1>(set, 0, default(Component1));
                        break;
                    case 1:
                        Context.CreateEntities<Component2>(set, 1, default(Component2));
                        break;
                    case 2:
                        Context.CreateEntities<Component3>(set, 2, default(Component3));
                        break;
                }

            {
                Context.CreateEntities<Component1, Component2, Component3>(set, 3, default(Component1), new Component2 { Value = 1 },
                    new Component3 { Value = 1 });
            }

            switch (_i % 4)
            {
                case 0:
                    Context.AddComponent<Padding1>(set, 4, default(Padding1));
                    break;
                case 2:
                    Context.AddComponent<Padding2>(set, 5, default(Padding2));
                    break;
                case 3:
                    Context.AddComponent<Padding3>(set, 6, default(Padding3));
                    break;
                case 4:
                    Context.AddComponent<Padding4>(set, 7, default(Padding4));
                    break;
            }
        }


        unsafe
        {
            // set up systems
            Context.AddSystem<Component1, Component2, Component3>(&Update, 3);
        }

        Context.FinishSetup();
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
