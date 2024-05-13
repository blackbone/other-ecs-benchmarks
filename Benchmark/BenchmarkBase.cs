using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark
{
    public abstract class BenchmarkBase;
    
    [MemoryDiagnoser]
    public abstract class BenchmarkBase<T> : BenchmarkBase where T : BenchmarkContextBase, new()
    {
        protected T Context;

        [Params(Constants.EntityCount)] public int EntityCount { get; set; }
        
        [IterationSetup]
        public void Setup()
        {
            Context = new T();
            Context.Setup(Constants.EntityCount);
            OnSetup();
        }

        [IterationCleanup]
        public void Cleanup()
        {
            OnCleanup();
            Context.Cleanup();
            Context.Dispose();
            Context = null;
        }

        protected virtual void OnCleanup() { }
        protected virtual void OnSetup() { }
    }
}