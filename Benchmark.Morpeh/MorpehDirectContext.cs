using Benchmark._Context;
using Scellecs.Morpeh;
// ReSharper disable ForCanBeConvertedToForeach

namespace Benchmark.Morpeh;

public class MorpehDirectContext : BenchmarkContextBase
{
    private World? _world;

    public override void Setup(int entityCount)
    {
        _world = World.Create();
    }

    public override void Cleanup()
    {
        _world?.Dispose();
        _world = null;
    }

    public override void Lock()
    {
        /* no op */
    }

    public override void Commit()
    {
        _world!.Commit();
    }

    public override void Warmup<T1>(int poolId)
    {
    }

    public override void Warmup<T1, T2>(int poolId)
    {
    }

    public override void Warmup<T1, T2, T3>(int poolId)
    {
    }

    public override void Warmup<T1, T2, T3, T4>(int poolId)
    {
    }

    public override void CreateEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity();
    }

    public override void CreateEntities<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i] .AddComponent<T1>();
        }
    }

    public override void CreateEntities<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].AddComponent<T1>();
            entities[i].AddComponent<T2>();
        }
    }

    public override void CreateEntities<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].AddComponent<T1>();
            entities[i].AddComponent<T2>();
            entities[i].AddComponent<T3>();
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].AddComponent<T1>();
            entities[i].AddComponent<T2>();
            entities[i].AddComponent<T3>();
            entities[i].AddComponent<T4>();
        }
    }

    public override void DeleteEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.RemoveEntity(entities[i]);
    }

    public override void AddComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponent<T1>();
    }

    public override void AddComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].AddComponent<T1>();
            entities[i].AddComponent<T2>();
        }
    }

    public override void AddComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].AddComponent<T1>();
            entities[i].AddComponent<T2>();
            entities[i].AddComponent<T3>();
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].AddComponent<T1>();
            entities[i].AddComponent<T2>();
            entities[i].AddComponent<T3>();
            entities[i].AddComponent<T4>();
        }
    }

    public override void RemoveComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponent<T1>();
    }

    public override void RemoveComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].RemoveComponent<T1>();
            entities[i].RemoveComponent<T2>();
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].RemoveComponent<T1>();
            entities[i].RemoveComponent<T2>();
            entities[i].RemoveComponent<T3>();
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].RemoveComponent<T1>();
            entities[i].RemoveComponent<T2>();
            entities[i].RemoveComponent<T3>();
            entities[i].RemoveComponent<T4>();
        }
    }

    public override object Shuffle(in object entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public override object PrepareSet(in int count) => new Entity[count];
}