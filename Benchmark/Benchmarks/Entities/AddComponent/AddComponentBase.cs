using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.AddComponent;

public abstract class AddComponentBase<T> : EntitiesBenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    protected object EntitySet { get; private set; }
    
    [Params(true, false)] public bool RandomOrder { get; set; }

    protected override void OnSetup()
    {
        EntitySet = Context.PrepareSet(EntityCount);
        Context.CreateEntities(EntitySet);
        if (RandomOrder) EntitySet = Context.Shuffle(EntitySet);
    }
}