using Benchmark.KremAppCore;
using BenchmarkDotNet.Attributes;

namespace Benchmark.AppCoreKostyl;

[ArtifactsPath(".benchmark_results/SystemWith2Components")]
[BenchmarkCategory(Categories.PerInvocationSetup)]
[MemoryDiagnoser]
public class SystemWith2Components_Sister : IBenchmark
{
    [Params(Constants.SystemEntityCount)] public int EntityCount { get; set; }

    [Params(0, 10)] public int Padding { get; set; }

    [Params(100)] public int Iterations { get; set; }
    public SisterContext Context { get; set; }

    [IterationSetup]
    public void Setup()
    {
        Context = new SisterContext(EntityCount);
        Context?.Setup();
        Context?.Warmup<Component1>(0);
        Context?.Warmup<Component2>(1);
        Context?.Warmup<Component1, Component2>(2);
        var set = Context?.PrepareSet(1);
        Context?.Lock();
        // set up entities
        for (var i = 0; i < EntityCount; ++i)
        {
            for (var j = 0; j < Padding; ++j)
                switch (j % 2)
                {
                    case 0:
                        Context?.CreateEntities<Component1>(set, 0);
                        break;
                    case 1:
                        Context?.CreateEntities<Component2>(set, 1);
                        break;
                }

            Context?.CreateEntities(set, 2, default(Component1), new Component2 { Value = 1 });
        }

        Context?.Commit();
        unsafe
        {
            // set up systems
            Context?.AddSystem<Component1, Component2>(&Update, 2);
        }

        Context?.FinishSetup();
    }

    [IterationCleanup]
    public void Cleanup()
    {
        Context?.Cleanup();
        Context?.Dispose();
        Context = default;
    }

    [Benchmark]
    public void Run()
    {
        var i = Iterations;
        while (i-- > 0)
            Context?.Tick(0.1f);
    }

    private static void Update(ref Component1 c1, ref Component2 c2)
    {
        c1.Value += c2.Value;
    }
}