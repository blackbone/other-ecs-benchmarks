using Arch.Core;
using Arch.Core.Utils;
using Benchmark._Context;
// ReSharper disable ForCanBeConvertedToForeach

namespace Benchmark.Arch;

public class ArchContext_Naive : BenchmarkContextBase
{
    private int _entityCount;
    private World? _world;
    private Dictionary<int, ComponentType[]>? _archetypes;
    
    public override int EntityCount => _world!.Size;

    public override void Setup(int entityCount)
    {
        _entityCount = entityCount;
        _world = World.Create();
        _archetypes = new Dictionary<int, ComponentType[]>();
    }

    public override void Cleanup()
    {
        _world?.Clear();
        _world?.Dispose();
        _world = null;
    }

    public override void Lock()
    {
        /* no op */
    }

    public override void Commit()
    {
        /* no op */
    }

    public override void Warmup<T1>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1)];
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void Warmup<T1, T2>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2)];
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void Warmup<T1, T2, T3>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2), typeof(T3)];
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void Warmup<T1, T2, T3, T4>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2), typeof(T3), typeof(T4)];
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void CreateEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Create();
    }

    public override void CreateEntities<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Create<T1>();
    }

    public override void CreateEntities<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Create<T1, T2>();
    }

    public override void CreateEntities<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (var i = 0; i < entities.Length; i++)
                entities[i] = _world!.Create(archetype);
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
                entities[i] = _world!.Create<T1, T2, T3>();
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Create<T1, T2, T3, T4>();
    }

    public override void DeleteEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.Destroy(entities[i]);
    }

    public override void AddComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            _world!.Add<T1>(entities[i]);
    }

    public override void AddComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            _world!.Add<T1, T2>(entities[i]);
    }

    public override void AddComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            _world!.Add<T1, T2, T3>(entities[i]);
    }

    public override void AddComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            _world!.Add<T1, T2, T3, T4>(entities[i]);
    }

    public override void RemoveComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            _world!.Remove<T1>(entities[i]);
    }

    public override void RemoveComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            _world!.Remove<T1, T2>(entities[i]);
    }

    public override void RemoveComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            _world!.Remove<T1, T2, T3>(entities[i]);
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            _world!.Remove<T1, T2, T3, T4>(entities[i]);
    }

    public override int CountWith<T1>(int poolId) => _world!.CountEntities(new QueryDescription().WithAll<T1>());

    public override int CountWith<T1, T2>(int poolId) => _world!.CountEntities(new QueryDescription().WithAll<T1, T2>());

    public override int CountWith<T1, T2, T3>(int poolId) => _world!.CountEntities(new QueryDescription().WithAll<T1, T2, T3>());

    public override int CountWith<T1, T2, T3, T4>(int poolId) => _world!.CountEntities(new QueryDescription().WithAll<T1, T2, T3, T4>());

    public override object Shuffle(in object entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public override object PrepareSet(in int count) => new Entity[count];
}