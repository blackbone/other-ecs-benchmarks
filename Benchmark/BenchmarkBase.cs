using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark;

public abstract class BenchmarkBase
{
    [Params(Constants.EntityCount)] public int EntityCount { get; set; }

    public abstract void Setup();
    public abstract void Cleanup();
    public abstract void Run();
}

[MemoryDiagnoser]
public abstract class BenchmarkBase<T> : BenchmarkBase where T : BenchmarkContextBase, new()
{
    protected T Context;

    [IterationSetup]
    public override void Setup()
    {
        Context = new T();
        Context.Setup(Constants.EntityCount);
        OnSetup();
    }

    [IterationCleanup]
    public override void Cleanup()
    {
        OnCleanup();
        Context.Cleanup();
        Context.Dispose();
        Context = null;
    }

    protected virtual void OnCleanup()
    {
    }

    protected virtual void OnSetup()
    {
    }

    public override string ToString() => $"{GetType().Name}<{typeof(T).Name}>";
}