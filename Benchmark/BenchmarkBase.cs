using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark;

public abstract class BenchmarkBase
{
    public abstract int EntityCount { get; set; }
    public abstract void Setup();
    public abstract void Cleanup();
    public abstract void Run();
}

public abstract class BenchmarkBase<T> : BenchmarkBase where T : BenchmarkContextBase, new()
{
    protected T Context;

    protected virtual void OnCleanup() { }

    protected virtual void OnSetup() { }

    public override string ToString() => $"{GetType().Name}<{typeof(T).Name}>";
}

public abstract class EntitiesBenchmarkBase<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Params(Constants.EntityCount)]
    public override int EntityCount { get; set; }
    
    [IterationSetup]
    public override void Setup()
    {
        Context = new T();
        Context.Setup(EntityCount);
        OnSetup();
        Context.FinishSetup();
    }

    [IterationCleanup]
    public override void Cleanup()
    {
        OnCleanup();
        Context.Cleanup();
        Context.Dispose();
        Context = null;
    }
}

public abstract class SystemBenchmarkBase<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Params(Constants.SystemEntityCount)]
    public override int EntityCount { get; set; }
    
    [IterationSetup]
    public override void Setup()
    {
        Context = new T();
        Context.Setup(EntityCount);
        OnSetup();
        Context.FinishSetup();
    }

    [IterationCleanup]
    public override void Cleanup()
    {
        OnCleanup();
        Context.Cleanup();
        Context.Dispose();
        Context = null;
    }
}
