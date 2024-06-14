using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(SystemWith1Component<T>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class SystemWith1Component<T> : IBenchmark<T> where T : IBenchmarkContext
{
    [Params(Constants.SystemEntityCount)] public int EntityCount { get; set; }
    [Params(0, 10)] public int Padding { get; set; }

    public T Context { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context?.Setup();
        Context?.Warmup<Component1>(0);
        var set = Context?.PrepareSet(1);
        Context?.Lock();
        for (var i = 0; i < EntityCount; ++i)
        {
            for (var j = 0; j < Padding; ++j)
                Context?.CreateEntities(set);

            Context?.CreateEntities(set, 0, new Component1 { Value = 0 });
        }

        Context?.Commit();

        unsafe
        {
            // set up systems
            Context?.AddSystem<Component1>(&Update, 0);
        }

        Context?.FinishSetup();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        Context?.Cleanup();
        Context?.Dispose();
        Context = default;
    }

    [Benchmark]
    public void Run() => Context?.Tick(0.1f);

    private static void Update(ref Component1 c1)
    {
        c1.Value++;
    }
}