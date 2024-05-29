using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(SystemWith1ComponentMultipleComposition<T>))]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class SystemWith1ComponentMultipleComposition<T> : IBenchmark<T> where T : struct, IBenchmarkContext
{
    [Params(Constants.SystemEntityCount)] public int EntityCount { get; set; }
    [Params(0, 10)] public int Padding { get; set; }
    [Params(100)] public int Iterations { get; set; }

    public T Context { get; set; }

    [IterationSetup]
    public void Setup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();

        Context.Warmup<Component1>(0);
        Context.Warmup<Padding1>(1);
        Context.Warmup<Padding2>(2);
        Context.Warmup<Padding3>(3);
        Context.Warmup<Padding4>(4);

        var set = Context.PrepareSet(1);
        Context.Lock();
        // set up entities
        for (var i = 0; i < EntityCount; ++i)
        {
            for (var j = 0; j < Padding; ++j)
                Context.CreateEntities(set);

            Context.CreateEntities(set, 0, new Component1 { Value = 0 });

            switch (i % 4)
            {
                case 0:
                    Context.AddComponent(set, 1, default(Padding1));
                    break;
                case 2:
                    Context.AddComponent(set, 2, default(Padding2));
                    break;
                case 3:
                    Context.AddComponent(set, 3, default(Padding3));
                    break;
                case 4:
                    Context.AddComponent(set, 4, default(Padding4));
                    break;
            }
        }

        Context.Commit();

        unsafe
        {
            // set up systems
            Context.AddSystem<Component1>(&Update, 0);
        }

        Context.FinishSetup();
    }

    [IterationCleanup]
    public void Cleanup()
    {
        Context.Cleanup();
        Context.Dispose();
        Context = default;
    }

    [Benchmark]
    public void Run()
    {
        var i = Iterations;
        while (i-- > 0) Context.Tick(0.1f);
    }

    private static void Update(ref Component1 c1)
    {
        c1.Value++;
    }
}