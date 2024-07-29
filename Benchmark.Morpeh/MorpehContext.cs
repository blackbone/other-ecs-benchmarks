using Benchmark._Context;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Workaround;
using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using XenoComponent = Xeno.IComponent;
using FrifloComponent = Friflo.Engine.ECS.IComponent;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming

namespace Benchmark.Morpeh;

public sealed class MorpehContext(int entityCount = 4096) : IBenchmarkContext
{
    private readonly Dictionary<int, Filter>? _filters = new();
    private readonly Dictionary<int, IStash[]>? _stashes = new();
    private SystemsGroup? _systems;
    private World? _world;

    public bool DeletesEntityOnLastComponentDeletion => true;

    public int EntityCount => _world!.EntityCount();

    public void Setup()
    {
        _world = World.Create();
        _systems = _world.CreateSystemsGroup();
        _world.AddSystemsGroup(0, _systems);
    }

    public void FinishSetup()
    {
        _world!.WarmupArchetypes(entityCount);
    }

    public void Cleanup()
    {
        _filters!.Clear();

        foreach (var stash in _stashes!.Values.SelectMany(s => s))
            stash.RemoveAll();
        _stashes!.Clear();

        _systems!.Dispose();
        _systems = null;

        _world?.Dispose();
        _world = null;
    }

    public void Dispose()
    {
    }

    public void Lock()
    {
        /* no op */
    }

    public void Commit()
    {
        _world!.Commit();
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var s1 = _world!.GetStash<T1>();
        
        _stashes![poolId] = [s1];
        _filters![poolId] = _world!.Filter.With<T1>().Build();
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var s1 = _world!.GetStash<T1>();
        var s2 = _world!.GetStash<T2>();
        
        _stashes![poolId] = [s1, s2];
        _filters![poolId] = _world!.Filter.With<T1>().With<T2>().Build();
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var s1 = _world!.GetStash<T1>();
        var s2 = _world!.GetStash<T2>();
        var s3 = _world!.GetStash<T3>();
        
        _stashes![poolId] = [s1, s2, s3];
        _filters![poolId] = _world!.Filter.With<T1>().With<T2>().With<T3>().Build();
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var s1 = _world!.GetStash<T1>();
        var s2 = _world!.GetStash<T2>();
        var s3 = _world!.GetStash<T3>();
        var s4 = _world!.GetStash<T4>();
        
        _stashes![poolId] = [s1, s2, s3, s4];
        _filters![poolId] = _world!.Filter.With<T1>().With<T2>().With<T3>().With<T4>().Build();
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _filters![poolId].GetLengthSlow();
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _filters![poolId].GetLengthSlow();
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _filters![poolId].GetLengthSlow();
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _filters![poolId].GetLengthSlow();
    }

    public bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var stashes = _stashes![poolId];
        var s1 = (Stash<T1>)stashes[0];

        var e = (Entity)entity;
        c1 = s1.Get(e);
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var stashes = _stashes![poolId];
        var s1 = (Stash<T1>)stashes[0];
        var s2 = (Stash<T2>)stashes[1];

        var e = (Entity)entity;
        c1 = s1.Get(e);
        c2 = s2.Get(e);
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var stashes = _stashes![poolId];
        var s1 = (Stash<T1>)stashes[0];
        var s2 = (Stash<T2>)stashes[1];
        var s3 = (Stash<T3>)stashes[2];

        var e = (Entity)entity;
        c1 = s1.Get(e);
        c2 = s2.Get(e);
        c3 = s3.Get(e);
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var stashes = _stashes![poolId];
        var s1 = (Stash<T1>)stashes[0];
        var s2 = (Stash<T2>)stashes[1];
        var s3 = (Stash<T3>)stashes[2];
        var s4 = (Stash<T4>)stashes[3];

        var e = (Entity)entity;
        c1 = s1.Get(e);
        c2 = s2.Get(e);
        c3 = s3.Get(e);
        c4 = s4.Get(e);
        return true;
    }

    public void Tick(float delta)
    {
        _world!.Update(delta);
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _systems.AddSystem(new PointerInvocationStashSystem<T1>(method));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _systems.AddSystem(new PointerInvocationStashSystem<T1, T2>(method));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _systems.AddSystem(new PointerInvocationStashSystem<T1, T2, T3>(method));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _systems.AddSystem(new PointerInvocationStashSystem<T1, T2, T3, T4>(method));
    }

    public void CreateEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var s1 = (Stash<T1>)_stashes![poolId][0];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            s1.Add(entities[i]) = c1;
        }
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
        }
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        var s3 = (Stash<T3>)pool[2];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
            s3.Add(entities[i]) = c3;
        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        var s3 = (Stash<T3>)pool[2];
        var s4 = (Stash<T4>)pool[3];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
            s3.Add(entities[i]) = c3;
            s4.Add(entities[i]) = c4;
        }
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.RemoveEntity(entities[i]);
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            s1.Add(entities[i]) = c1;
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
        }
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        var s3 = (Stash<T3>)pool[2];
        for (var i = 0; i < entities.Length; i++)
        {
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
            s3.Add(entities[i]) = c3;
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        var s3 = (Stash<T3>)pool[2];
        var s4 = (Stash<T4>)pool[3];
        for (var i = 0; i < entities.Length; i++)
        {
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
            s3.Add(entities[i]) = c3;
            s4.Add(entities[i]) = c4;
        }
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var c1 = (Stash<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            c1.Remove(entities[i]);
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var c1 = (Stash<T1>)pool[0];
        var c2 = (Stash<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            c1.Remove(entities[i]);
            c2.Remove(entities[i]);
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var c1 = (Stash<T1>)pool[0];
        var c2 = (Stash<T2>)pool[1];
        var c3 = (Stash<T3>)pool[2];
        for (var i = 0; i < entities.Length; i++)
        {
            c1.Remove(entities[i]);
            c2.Remove(entities[i]);
            c3.Remove(entities[i]);
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var c1 = (Stash<T1>)pool[0];
        var c2 = (Stash<T2>)pool[1];
        var c3 = (Stash<T3>)pool[2];
        var c4 = (Stash<T4>)pool[3];
        for (var i = 0; i < entities.Length; i++)
        {
            c1.Remove(entities[i]);
            c2.Remove(entities[i]);
            c3.Remove(entities[i]);
            c4.Remove(entities[i]);
        }
    }

    public Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public Array PrepareSet(in int count)
    {
        return count > 0 ? new Entity[count] : [];
    }
}