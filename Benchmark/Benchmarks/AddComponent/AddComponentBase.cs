using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.AddComponent;

public abstract class AddComponentBase<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    protected object EntitySet { get; private set; }
    
    [Params(true, false)] public bool Random { get; set; }

    protected override void OnSetup()
    {
        EntitySet = Context.PrepareSet(EntityCount);
        Context.CreateEntities(EntitySet);
        if (Random) EntitySet = Context.Shuffle(EntitySet);
    }
}