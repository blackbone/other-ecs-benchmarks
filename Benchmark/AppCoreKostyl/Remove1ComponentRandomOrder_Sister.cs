using System;
using Benchmark.KremAppCore;
using BenchmarkDotNet.Attributes;

namespace Benchmark.AppCoreKostyl
{
    [BenchmarkCategory(Categories.PerInvocationSetup)]
    [ArtifactsPath(".benchmark_results/Remove1ComponentRandomOrder")]
    [MemoryDiagnoser]
    public class Remove1ComponentRandomOrder_Sister : IBenchmark
    {
        [Params(Constants.EntityCount)]
        public int EntityCount { get; set; }
        public SisterContext Context { get; set; }

        private Array _entitySet;
        [IterationSetup]
        public void Setup()
        {
            Context = new SisterContext(EntityCount);
            Context?.Setup();
            Context?.Warmup<Component1>(0);
            _entitySet = Context?.PrepareSet(EntityCount);
            Context?.CreateEntities<Component1>(_entitySet, 0);
            _entitySet = Context?.Shuffle(_entitySet);
            Context?.FinishSetup();
        }

        [IterationCleanup]
        public void Cleanup()
        {
            if (!Context.DeletesEntityOnLastComponentDeletion)
                Context?.DeleteEntities(_entitySet);
            Context?.Cleanup();
            Context?.Dispose();
            Context = default;
        }

        [Benchmark]
        public void Run()
        {
            Context?.Lock();
            Context?.RemoveComponent<Component1>(_entitySet, 0);
            Context?.Commit();
        }
    }
}