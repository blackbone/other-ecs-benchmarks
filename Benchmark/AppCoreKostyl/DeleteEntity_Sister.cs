using System;
using Benchmark.KremAppCore;
using BenchmarkDotNet.Attributes;

namespace Benchmark.AppCoreKostyl
{
    [ArtifactsPath(".benchmark_results/DeleteEntity")]
    [BenchmarkCategory(Categories.PerInvocationSetup)]
    [MemoryDiagnoser]
    public class DeleteEntity_Sister : IBenchmark
    {
        [Params(Constants.EntityCount)]
        public int EntityCount { get; set; }
        public SisterContext Context { get; set; }

        private Array _entitySet;
        [IterationSetup]
        public void Setup()
        {
            Context = new SisterContext(EntityCount);
            Context.Setup();
            _entitySet = Context.PrepareSet(EntityCount);
            Context.Lock();
            Context.CreateEntities(_entitySet);
            Context.Commit();
            Context.FinishSetup();
        }

        [IterationCleanup]
        public void Cleanup()
        {
            Context?.DeleteEntities(_entitySet);
            Context?.Cleanup();
            Context?.Dispose();
            Context = default;
        }

        [Benchmark]
        public void Run()
        {
            Context?.Lock();
            Context?.DeleteEntities(_entitySet);
            Context?.Commit();
            _entitySet = Context?.PrepareSet(0);
        }
    }
}