using Benchmark._Context;
using DCFApixels.DragonECS;
using Leopotam.Ecs;
using Scellecs.Morpeh;
using EcsWorld = Leopotam.Ecs.EcsWorld;

namespace Benchmark.LeoEcs;

public sealed class LeoEcsContext(int entityCount = 4096) : IBenchmarkContext
{
    private readonly Dictionary<int, EcsFilter>? _filters = new();
    private readonly List<EcsSystems>? _systems = new();
    private EcsWorld? _world;

    public bool DeletesEntityOnLastComponentDeletion => true;

    public int EntityCount => _world!.GetStats().ActiveEntities;

    public void Setup()
    {
        _world = new EcsWorld();
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
        _systems.Clear();
        _world!.Destroy();
        _world = null;
    }

    public void Dispose()
    {
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent
    {
        _filters![poolId] = _world!.GetFilter(typeof(EcsFilter<T1>));
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
    {
        _filters![poolId] = _world!.GetFilter(typeof(EcsFilter<T1, T2>));
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        _filters![poolId] = _world!.GetFilter(typeof(EcsFilter<T1, T2, T3>));
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        _filters![poolId] = _world!.GetFilter(typeof(EcsFilter<T1, T2, T3, T4>));
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
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.NewEntity();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            entities[i].Replace(c1);
        }
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            entities[i].Replace(c1);
            entities[i].Replace(c2);
        }
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            entities[i].Replace(c1);
            entities[i].Replace(c2);
            entities[i].Replace(c3);
        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            entities[i].Replace(c1);
            entities[i].Replace(c2);
            entities[i].Replace(c3);
            entities[i].Replace(c4);
        }
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive()) entities[i].Destroy();
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Replace(c1);
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Replace(c1);
            entities[i].Replace(c2);
        }
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Replace(c1);
            entities[i].Replace(c2);
            entities[i].Replace(c3);
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Replace(c1);
            entities[i].Replace(c2);
            entities[i].Replace(c3);
            entities[i].Replace(c4);
        }
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive() && entities[i].Has<T1>()) entities[i].Del<T1>();
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            if (entities[i].IsAlive() && entities[i].Has<T1>()) entities[i].Del<T1>();
            if (entities[i].IsAlive() && entities[i].Has<T2>()) entities[i].Del<T2>();
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            if (entities[i].IsAlive() && entities[i].Has<T1>()) entities[i].Del<T1>();
            if (entities[i].IsAlive() && entities[i].Has<T2>()) entities[i].Del<T2>();
            if (entities[i].IsAlive() && entities[i].Has<T3>()) entities[i].Del<T3>();
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            if (entities[i].IsAlive() && entities[i].Has<T1>()) entities[i].Del<T1>();
            if (entities[i].IsAlive() && entities[i].Has<T2>()) entities[i].Del<T2>();
            if (entities[i].IsAlive() && entities[i].Has<T3>()) entities[i].Del<T3>();
            if (entities[i].IsAlive() && entities[i].Has<T4>()) entities[i].Del<T4>();
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

        var e = (EcsEntity)entity;
        c1 = e.Get<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (EcsEntity)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (EcsEntity)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (EcsEntity)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        c4 = e.Get<T4>();
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
        _systems!.Add(new EcsSystems(_world!).Add(new System<T1>(method)).ProcessInjects());
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new EcsSystems(_world!).Add(new System<T1, T2>(method)).ProcessInjects());
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new EcsSystems(_world!).Add(new System<T1, T2, T3>(method)).ProcessInjects());
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new EcsSystems(_world!).Add(new System<T1, T2, T3, T4>(method)).ProcessInjects());
    }

    public Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((EcsEntity[])entitySet);
        return entitySet;
    }

    public Array PrepareSet(in int count) => count > 0 ? new EcsEntity[count] : [];
}