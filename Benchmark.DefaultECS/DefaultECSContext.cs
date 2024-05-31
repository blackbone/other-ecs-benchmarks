using Benchmark._Context;
using DCFApixels.DragonECS;
using DefaultEcs.System;
using Scellecs.Morpeh;
using Entity = DefaultEcs.Entity;
using World = DefaultEcs.World;

namespace Benchmark.DefaultECS;

#pragma warning disable CS9113 // Parameter is unread.
public sealed class DefaultECSContext(int entityCount = 4096) : IBenchmarkContext
#pragma warning restore CS9113 // Parameter is unread.
{
    private readonly List<ISystem<float>>? _systems = new();
    private World? _world;

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int EntityCount => _world!.Count();
    
    public void Setup()
    {
        _world = new World();
    }

    public void FinishSetup()
    {
        _world!.TrimExcess();
        _world!.Optimize();
    }

    public void Cleanup()
    {
        _systems!.Clear();
        
        _world!.Dispose();
        _world = null;
    }

    public void Dispose()
    {
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent { }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent { }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent { }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent { }

    public void Lock()
    {
        // no op
    }

    public void Commit()
    {
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive) entities[i].Dispose();
    }

    public Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public Array PrepareSet(in int count) => count > 0 ? new Entity[count] : [];

    public void CreateEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default) where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].Set(c1);
        }
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].Set(c1);
            entities[i].Set(c2);
        }
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].Set(c1);
            entities[i].Set(c2);
            entities[i].Set(c3);
        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].Set(c1);
            entities[i].Set(c2);
            entities[i].Set(c3);
            entities[i].Set(c4);
        }
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default) where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Set(c1);
        }
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Set(c1);
            entities[i].Set(c2);
        }
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Set(c1);
            entities[i].Set(c2);
            entities[i].Set(c3);
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Set(c1);
            entities[i].Set(c2);
            entities[i].Set(c3);
            entities[i].Set(c4);
        }
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Remove<T1>();
        }
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Remove<T1>();
            entities[i].Remove<T2>();
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Remove<T1>();
            entities[i].Remove<T2>();
            entities[i].Remove<T3>();
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Remove<T1>();
            entities[i].Remove<T2>();
            entities[i].Remove<T3>();
            entities[i].Remove<T4>();
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent
    {
        return _world!.GetEntities().With<T1>().AsEnumerable().Count();
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        return _world!.GetEntities().With<T1>().With<T2>().AsEnumerable().Count();
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        return _world!.GetEntities().With<T1>().With<T2>().With<T3>().AsEnumerable().Count();
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        return _world!.GetEntities().With<T1>().With<T2>().With<T3>().With<T4>().AsEnumerable().Count();
    }

    public bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Get<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        c4 = e.Get<T4>();
        return true;
    }

    public void Tick(float delta)
    {
        foreach (var system in _systems!)
            system.Update(delta);
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent
        => _systems!.Add(new System<T1>(_world!, method));

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
        => _systems!.Add(new System<T1, T2>(_world!, method));

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
        => _systems!.Add(new System<T1, T2, T3>(_world!, method));

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
        => _systems!.Add(new System<T1, T2, T3, T4>(_world!, method));
}