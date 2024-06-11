using Benchmark.KremAppCore;
using BenchmarkDotNet.Attributes;

namespace Benchmark.AppCoreKostyl
{
    [ArtifactsPath(".benchmark_results/SystemWith1ComponentMultipleComposition")]
    [BenchmarkCategory(Categories.PerInvocationSetup)]
    [MemoryDiagnoser]
    public class SystemWith1ComponentMultipleComposition_Sister : IBenchmark
    {
        [Params(Constants.SystemEntityCount)]
        public int EntityCount { get; set; }

        [Params(0, 10)]
        public int Padding { get; set; }

        [Params(Constants.IterationCount)] 
        public int Iterations { get; set; }
        public SisterContext Context { get; set; }

        [IterationSetup]
        public void Setup()
        {
            Context = new SisterContext(EntityCount);
            Context?.Setup();
            Context?.Warmup<Component1>(0);
            Context?.Warmup<Padding1>(1);
            Context?.Warmup<Padding2>(2);
            Context?.Warmup<Padding3>(3);
            Context?.Warmup<Padding4>(4);
            var set = Context?.PrepareSet(1);
            Context?.Lock();
            // set up entities
            for (var i = 0; i < EntityCount; ++i)
            {
                for (var j = 0; j < Padding; ++j)
                    switch (i % 4)
                    {
                        case 0:
                            Context?.CreateEntities(set, 1, default(Padding1));
                            break;
                        case 2:
                            Context?.CreateEntities(set, 2, default(Padding2));
                            break;
                        case 3:
                            Context?.CreateEntities(set, 3, default(Padding3));
                            break;
                        case 4:
                            Context?.CreateEntities(set, 4, default(Padding4));
                            break;
                    }

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