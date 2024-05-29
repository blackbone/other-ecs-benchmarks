using Benchmark._Context;
using DCFApixels.DragonECS;
using Leopotam.EcsLite;
using Scellecs.Morpeh;
using EcsWorld = Leopotam.EcsLite.EcsWorld;
using IEcsPool = Leopotam.EcsLite.IEcsPool;

namespace Benchmark.LeoEcsLite;

public readonly struct LeoEcsLiteContext(in int entityCount = 4096) : IBenchmarkContext
{
    private readonly int _maxEntityCount = entityCount;
    private readonly Dictionary<int, EcsFilter>? _filters = new();
    private readonly Dictionary<int, IEcsPool[]>? _pools = new();
    private readonly List<IEcsSystems>? _systems = new();
    private readonly EcsWorld? _world = new(new EcsWorld.Config { Entities = entityCount });

    public bool DeletesEntityOnLastComponentDeletion => true;

    public int EntityCount => _world!.GetEntitiesCount();

    public void Setup()
    {
    }

    public void FinishSetup()
    {
        foreach (var system in _systems!)
            system.Init();
    }

    public void Cleanup()
    {
        foreach (var system in _systems!)
            system.Destroy();
        _systems!.Clear();
        _filters!.Clear();
        _pools!.Clear();
    }

    public void Dispose()
    {
        _world!.Destroy();
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent
    {
        _pools![poolId] = [_world!.GetPool<T1>()];
        _filters![poolId] = _world!.Filter<T1>().End(_maxEntityCount);
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
    {
        _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>()];
        _filters![poolId] = _world!.Filter<T1>().Inc<T2>().End(_maxEntityCount);
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>(), _world!.GetPool<T3>()];
        _filters![poolId] = _world!.Filter<T1>().Inc<T2>().Inc<T3>().End(_maxEntityCount);
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>(), _world!.GetPool<T3>(), _world!.GetPool<T4>()];
        _filters![poolId] = _world!.Filter<T1>().Inc<T2>().Inc<T3>().Inc<T4>().End(_maxEntityCount);
    }

    public void Lock()
    {
        // no op
    }

    public void Commit()
    {
        // no op
    }

    public void CreateEntities(in Array entitySet)
    {
        var entities = (int[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.NewEntity();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            p1.Add(entities[i]) = c1;
        }
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
        }
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        var p3 = (Leopotam.EcsLite.EcsPool<T3>)pools[2];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
            p3.Add(entities[i]) = c3;
        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        var p3 = (Leopotam.EcsLite.EcsPool<T3>)pools[2];
        var p4 = (Leopotam.EcsLite.EcsPool<T4>)pools[3];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
            p3.Add(entities[i]) = c3;
            p4.Add(entities[i]) = c4;
        }
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (int[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.DelEntity(entities[i]);
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        for (var i = 0; i < entities.Length; i++)
            p1.Add(entities[i]) = c1;
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        for (var i = 0; i < entities.Length; i++)
        {
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
        }
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        var p3 = (Leopotam.EcsLite.EcsPool<T3>)pools[2];
        for (var i = 0; i < entities.Length; i++)
        {
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
            p3.Add(entities[i]) = c3;
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        var p3 = (Leopotam.EcsLite.EcsPool<T3>)pools[2];
        var p4 = (Leopotam.EcsLite.EcsPool<T4>)pools[3];
        for (var i = 0; i < entities.Length; i++)
        {
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
            p3.Add(entities[i]) = c3;
            p4.Add(entities[i]) = c4;
        }
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        for (var i = 0; i < entities.Length; i++)
            p1.Del(entities[i]);
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        for (var i = 0; i < entities.Length; i++)
        {
            p1.Del(entities[i]);
            p2.Del(entities[i]);
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        var p3 = (Leopotam.EcsLite.EcsPool<T3>)pools[2];
        for (var i = 0; i < entities.Length; i++)
        {
            p1.Del(entities[i]);
            p2.Del(entities[i]);
            p3.Del(entities[i]);
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        var p3 = (Leopotam.EcsLite.EcsPool<T3>)pools[2];
        var p4 = (Leopotam.EcsLite.EcsPool<T4>)pools[3];
        for (var i = 0; i < entities.Length; i++)
        {
            p1.Del(entities[i]);
            p2.Del(entities[i]);
            p3.Del(entities[i]);
            p4.Del(entities[i]);
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent
    {
        return _filters![poolId].GetEntitiesCount();
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
    {
        return _filters![poolId].GetEntitiesCount();
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        return _filters![poolId].GetEntitiesCount();
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        return _filters![poolId].GetEntitiesCount();
    }

    public bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (int)entity;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        c1 = p1.Get(e);
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (int)entity;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        c1 = p1.Get(e);
        c2 = p2.Get(e);
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (int)entity;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        var p3 = (Leopotam.EcsLite.EcsPool<T3>)pools[2];
        c1 = p1.Get(e);
        c2 = p2.Get(e);
        c3 = p3.Get(e);
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (int)entity;
        var pools = _pools![poolId];
        var p1 = (Leopotam.EcsLite.EcsPool<T1>)pools[0];
        var p2 = (Leopotam.EcsLite.EcsPool<T2>)pools[1];
        var p3 = (Leopotam.EcsLite.EcsPool<T3>)pools[2];
        var p4 = (Leopotam.EcsLite.EcsPool<T4>)pools[3];
        c1 = p1.Get(e);
        c2 = p2.Get(e);
        c3 = p3.Get(e);
        c4 = p4.Get(e);
        return true;
    }

    public void Tick(float delta)
    {
        foreach (var system in _systems!)
            system.Run();
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new EcsSystems(_world!).Add(new System<T1>(method)));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new EcsSystems(_world!).Add(new System<T1, T2>(method)));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new EcsSystems(_world!).Add(new System<T1, T2, T3>(method)));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new EcsSystems(_world!).Add(new System<T1, T2, T3, T4>(method)));
    }

    public Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((int[])entitySet);
        return entitySet;
    }

    public Array PrepareSet(in int count)
    {
        return new int[count];
    }
}