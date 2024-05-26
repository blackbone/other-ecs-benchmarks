using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using Benchmark._Context;
// ReSharper disable ForCanBeConvertedToForeach

namespace Benchmark.Arch;

public class ArchContext_Naive : BenchmarkContextBase
{
    private int _entityCount;
    private World? _world;
    private Dictionary<int, ComponentType[]>? _archetypes;
    private Dictionary<int, QueryDescription>? _queries;
    private List<Action<float>>? _systems; 
    
    public override int EntityCount => _world!.Size;

    public override void Setup(int entityCount)
    {
        _entityCount = entityCount;
        _world = World.Create();
        _archetypes = new Dictionary<int, ComponentType[]>();
        _queries = new Dictionary<int, QueryDescription>();
        _systems = new List<Action<float>>();
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
        _queries![poolId] = new QueryDescription().WithAll<T1>();
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void Warmup<T1, T2>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2)];
        _queries![poolId] = new QueryDescription().WithAll<T1, T2>();
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void Warmup<T1, T2, T3>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2), typeof(T3)];
        _queries![poolId] = new QueryDescription().WithAll<T1, T2, T3>();
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void Warmup<T1, T2, T3, T4>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2), typeof(T3), typeof(T4)];
        _queries![poolId] = new QueryDescription().WithAll<T1, T2, T3, T4>();
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void CreateEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Create();
    }

    public override void CreateEntities<T1>(in object entitySet, in int poolId = -1, in T1 c1 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.Create(c1);
            entities[i].Add(c1);
        }
    }

    public override void CreateEntities<T1, T2>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.Create(c1, c2);
            entities[i].Add(c1, c2);
        }
    }

    public override void CreateEntities<T1, T2, T3>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.Create<T1, T2, T3>();
            entities[i].Add(c1, c2, c3);
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.Create<T1, T2, T3, T4>();
            entities[i].Add(c1, c2, c3, c4);
        }
    }

    public override void DeleteEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.Destroy(entities[i]);
    }

    public override void AddComponent<T1>(in object entitySet, in int poolId = -1, in T1 c1 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.Add(entities[i], c1);
    }

    public override void AddComponent<T1, T2>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.Add(entities[i], c1, c2);
    }

    public override void AddComponent<T1, T2, T3>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.Add(entities[i], c1, c2, c3);
    }

    public override void AddComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.Add(entities[i], c1, c2, c3, c4);
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
    
    public override void Tick(float delta)
    {
        foreach (var system in _systems!)
            system(delta);
    }

    public override unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
    {
        _systems!.Add(_ => _world!.Query(_queries![poolId], (ref T1 t1) => method(ref t1)));
    }

    public override unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
    {
        _systems!.Add(_ => _world!.Query(_queries![poolId], (ref T1 t1, ref T2 t2) => method(ref t1, ref t2)));
    }

    public override unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
    {
        _systems!.Add(_ => _world!.Query(_queries![poolId], (ref T1 t1, ref T2 t2, ref T3 t3) => method(ref t1, ref t2, ref t3)));
    }

    public override unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
    {
        _systems!.Add(_ => _world!.Query(_queries![poolId], (ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4) => method(ref t1, ref t2, ref t3, ref t4)));
    }

    public override object Shuffle(in object entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public override object PrepareSet(in int count) => new Entity[count];
}