using Benchmark.Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(MultiSystems<T, TE>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class MultiSystems<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
{
    private const int System1 = 0;
    private const int System2 = 1;
    private const int System3 = 2;
    private const float Delta = 0.1f;

    [Params(Constants.SystemEntityCount)]
    public int EntityCount { get; set; }

    public T Context { get; set; }

    private TE[] _set1;
    private TE[] _set2;
    private TE[] _set3;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();

        Context.Warmup<Component1>(System1);
        Context.Warmup<Component2>(System2);
        Context.Warmup<Component1, Component2>(System3);

        unsafe
        {
            Context.AddSystem<Component1>(&Update1, System1);
            Context.AddSystem<Component2>(&Update2, System2);
            Context.AddSystem<Component1, Component2>(&Update3, System3);
        }

        Context.FinishSetup();

        var third = EntityCount / 3;
        _set1 = Context.PrepareSet(third);
        _set2 = Context.PrepareSet(third);
        _set3 = Context.PrepareSet(EntityCount - third - third);

        Context.CreateEntities<Component1>(_set1, System1, default(Component1));
        Context.CreateEntities<Component2>(_set2, System2, default(Component2));
        Context.CreateEntities<Component1, Component2>(_set3, System3, default(Component1), default(Component2));
    }

    [IterationSetup]
    public void IterationSetup()
    {
    }

    [Benchmark]
    public void Run()
    {
        Context.Tick(Delta);
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        Context.DeleteEntities(_set1);
        Context.DeleteEntities(_set2);
        Context.DeleteEntities(_set3);

        Context.Cleanup();
        Context.Dispose();
        Context = default;
    }

    private static void Update1(ref Component1 c1) => c1.Value++;
    private static void Update2(ref Component2 c2) => c2.Value++;
    private static void Update3(ref Component1 c1, ref Component2 c2) => c1.Value += c2.Value;
}
