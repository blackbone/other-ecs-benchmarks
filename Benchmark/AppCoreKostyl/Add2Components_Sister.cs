using System;
using Benchmark.KremAppCore;
using BenchmarkDotNet.Attributes;

namespace Benchmark.AppCoreKostyl
{
    [ArtifactsPath(".benchmark_results/Add2Components")]
    [BenchmarkCategory(Categories.PerInvocationSetup)]
    [MemoryDiagnoser]
    public class Add2Components_Sister : IBenchmark
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
            _entitySet = Context?.PrepareSet(EntityCount);
            Context?.CreateEntities(_entitySet);
            Context?.Warmup<Component1, Component2>(0);
            Context?.FinishSetup();
        }

        [IterationCleanup]
        public void Cleanup()
        {
            Context?.RemoveComponent<Component1, Component2>(_entitySet, 0);
            Context?.DeleteEntities(_entitySet);
            Context?.Cleanup();
            Context?.Dispose();
            Context = default;
        }

        [Benchmark]
        public void Run()
        {
            Context?.Lock();
            Context?.AddComponent<Component1, Component2>(_entitySet, 0);
            Context?.Commit();
        }
    }
}