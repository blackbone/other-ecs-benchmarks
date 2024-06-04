using System;
using Benchmark.KremAppCore;
using BenchmarkDotNet.Attributes;

namespace Benchmark.AppCoreKostyl
{
    [ArtifactsPath(".benchmark_results/CreateEmptyEntity")]
    [BenchmarkCategory(Categories.PerInvocationSetup)]
    [MemoryDiagnoser]
    public class CreateEmptyEntity_Sister : IBenchmark
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
            Context?.FinishSetup();
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
            Context?.CreateEntities(_entitySet);
            Context?.Commit();
        }
    }
}