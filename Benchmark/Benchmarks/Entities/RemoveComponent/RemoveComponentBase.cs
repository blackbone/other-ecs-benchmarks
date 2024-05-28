using System;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Entities.RemoveComponent;

public abstract class RemoveComponentBase<T> : EntitiesBenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Params(true, false)] public bool Random { get; set; }
    protected Array EntitySet;
    
    protected override void OnCleanup()
    {
        base.OnCleanup();
        if (!Context.DeletesEntityOnLastComponentDeletion)
            Context.DeleteEntities(EntitySet);
    }

}