using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.DeleteEntity;

public class DeleteEntityBenchmark<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    private object _entitySet;

    protected override void OnSetup()
    {
        base.OnSetup();
        _entitySet = Context.PrepareSet(EntityCount);
        
        Context.Lock();
        Context.CreateEntities(_entitySet);
        Context.Commit();
    }

    [Benchmark]
    public void _()
    {
        Context.Lock();
        Context.DeleteEntities(_entitySet);
        Context.Commit();
    }
}