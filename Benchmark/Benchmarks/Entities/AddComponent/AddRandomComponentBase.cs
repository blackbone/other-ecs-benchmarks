using Benchmark._Context;
using Benchmark.Utils;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.AddComponent;

public abstract class AddRandomComponentBase<T> : EntitiesBenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    protected object[] EntitySets { get; private set; }
    
    [Params(true, false)] public bool RandomOrder { get; set; }
    [Params(1, 32)] public int ChunkSize { get; set; }

    protected override void OnSetup()
    {
        var setsCount = EntityCount / ChunkSize + 1;
        EntitySets = new object[setsCount];
        for (var i = 0; i < setsCount; i++)
        {
            EntitySets[i] = Context.PrepareSet(ChunkSize);
            Context.CreateEntities(EntitySets[i]);
        }
        if (RandomOrder) EntitySets.Shuffle();
    }
}