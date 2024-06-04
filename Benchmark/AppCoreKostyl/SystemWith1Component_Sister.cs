using Benchmark.KremAppCore;
using BenchmarkDotNet.Attributes;

namespace Benchmark.AppCoreKostyl
{
    [ArtifactsPath(".benchmark_results/SystemWith1Component")]
    [BenchmarkCategory(Categories.PerInvocationSetup)]
    [MemoryDiagnoser]
    public class SystemWith1Component_Sister : IBenchmark
    {
        [Params(Constants.SystemEntityCount)]
        public int EntityCount { get; set; }

        [Params(0, 10)]
        public int Padding { get; set; }

        [Params(100)]
        public int Iterations { get; set; }
        public SisterContext Context { get; set; }

        [IterationSetup]
        public void Setup()
        {
            Context = new SisterContext(EntityCount);
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

        private static void Update(ref Component1 c1)
        {
            c1.Value++;
        }
    }
}