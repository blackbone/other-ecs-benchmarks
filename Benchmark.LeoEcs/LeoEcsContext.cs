using Benchmark._Context;
using Leopotam.Ecs;
using EcsWorld = Leopotam.Ecs.EcsWorld;

namespace Benchmark.LeoEcs;

public class LeoEcsContext : BenchmarkContextBase
{
    private EcsWorld? _world;

    public override int EntityCount => _world!.GetStats().ActiveEntities;
    
    public override void Setup(int entityCount)
    {
        _world = new EcsWorld();
    }

    public override void Warmup<T1>(int poolId) { }

    public override void Warmup<T1, T2>(int poolId) { }

    public override void Warmup<T1, T2, T3>(int poolId) { }

    public override void Warmup<T1, T2, T3, T4>(int poolId) { }

    public override void Cleanup()
    {
        _world!.Destroy();
        _world = null;
    }

    public override void Lock()
    {
        // no op
    }

    public override void Commit()
    {
        // no op
    }

    public override void CreateEntities(in object entitySet)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.NewEntity();
    }

    public override void CreateEntities<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            entities[i].Replace(default(T1));
        }
    }

    public override void CreateEntities<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            entities[i].Replace(default(T1));
            entities[i].Replace(default(T2));
        }
    }

    public override void CreateEntities<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            entities[i].Replace(default(T1));
            entities[i].Replace(default(T2));
            entities[i].Replace(default(T3));
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            entities[i].Replace(default(T1));
            entities[i].Replace(default(T2));
            entities[i].Replace(default(T3));
            entities[i].Replace(default(T4));
        }
    }

    public override void DeleteEntities(in object entitySet)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Destroy();
    }

    public override void AddComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Replace(default(T1));
    }

    public override void AddComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Replace(default(T1));
            entities[i].Replace(default(T2));
        }
    }

    public override void AddComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Replace(default(T1));
            entities[i].Replace(default(T2));
            entities[i].Replace(default(T3));
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Replace(default(T1));
            entities[i].Replace(default(T2));
            entities[i].Replace(default(T3));
            entities[i].Replace(default(T4));
        }
    }

    public override void RemoveComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Del<T1>();
    }

    public override void RemoveComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Del<T1>();
            entities[i].Del<T2>();
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Del<T1>();
            entities[i].Del<T2>();
            entities[i].Del<T3>();
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Del<T1>();
            entities[i].Del<T2>();
            entities[i].Del<T3>();
            entities[i].Del<T4>();
        }
    }

    public override int CountWith<T1>(int poolId) => _world!.GetFilter(typeof(EcsFilter<T1>)).GetEntitiesCount();

    public override int CountWith<T1, T2>(int poolId) => _world!.GetFilter(typeof(EcsFilter<T1, T2>)).GetEntitiesCount();

    public override int CountWith<T1, T2, T3>(int poolId) => _world!.GetFilter(typeof(EcsFilter<T1, T2, T3>)).GetEntitiesCount();

    public override int CountWith<T1, T2, T3, T4>(int poolId) => _world!.GetFilter(typeof(EcsFilter<T1, T2, T3, T4>)).GetEntitiesCount();

    public override object Shuffle(in object entitySet)
    {
        Random.Shared.Shuffle((EcsEntity[])entitySet);
        return entitySet;
    }

    public override object PrepareSet(in int count)
    {
        return new EcsEntity[count];
    }
}

