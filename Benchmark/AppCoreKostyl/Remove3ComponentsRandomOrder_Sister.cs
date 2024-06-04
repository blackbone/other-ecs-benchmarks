using System;
using Benchmark.KremAppCore;
using BenchmarkDotNet.Attributes;

namespace Benchmark.AppCoreKostyl
{
    [BenchmarkCategory(Categories.PerInvocationSetup)]
    [ArtifactsPath(".benchmark_results/Remove3ComponentsRandomOrder")]
    [MemoryDiagnoser]
    public class Remove3ComponentsRandomOrder_Sister : IBenchmark
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
            Context?.Warmup<Component1, Component2, Component3>(0);
            _entitySet = Context?.PrepareSet(EntityCount);
            Context?.CreateEntities<Component1, Component2, Component3>(_entitySet, 0);
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
            Context?.RemoveComponent<Component1, Component2, Component3>(_entitySet, 0);
            Context?.Commit();
        }
    }
}