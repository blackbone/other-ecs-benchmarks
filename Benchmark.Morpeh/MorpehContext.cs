using Benchmark._Context;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Workaround;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming

namespace Benchmark.Morpeh_2023;

public class MorpehContext : BenchmarkContextBase
{
    private World? _world;
    private Dictionary<int, IStash[]>? _stashes;
    private Dictionary<int, Filter>? _filters;
    private SystemsGroup? _systems;
    
    public override bool DeletesEntityOnLastComponentDeletion => true;

    public override int EntityCount => _world!.EntityCount();

    public override void Setup(int entityCount)
    {
        _world = World.Create();
        _systems = _world.CreateSystemsGroup();
        _world.AddSystemsGroup(0, _systems);
        _stashes = new Dictionary<int, IStash[]>();
        _filters = new Dictionary<int, Filter>();
    }

    public override void Cleanup()
    {
        _world?.Dispose();
        _world = null;
        
        _stashes!.Clear();
    }

    public override void Lock()
    {
        /* no op */
    }

    public override void Commit()
    {
        _world!.Commit();
    }

    public override void Warmup<T1>(in int poolId)
    {
        _stashes![poolId] = [_world!.GetStash<T1>()];
        _filters![poolId] = _world!.Filter.With<T1>().Build();
    }

    public override void Warmup<T1, T2>(in int poolId)
    {
        _stashes![poolId] = [_world!.GetStash<T1>(), _world!.GetStash<T2>()];
        _filters![poolId] = _world!.Filter.With<T1>().With<T2>().Build();
    }

    public override void Warmup<T1, T2, T3>(in int poolId)
    {
        _stashes![poolId] = [_world!.GetStash<T1>(), _world!.GetStash<T2>(), _world!.GetStash<T3>()];
        _filters![poolId] = _world!.Filter.With<T1>().With<T2>().With<T3>().Build();
    }

    public override void Warmup<T1, T2, T3, T4>(in int poolId)
    {
        _stashes![poolId] = [_world!.GetStash<T1>(), _world!.GetStash<T2>(), _world!.GetStash<T3>(), _world!.GetStash<T4>()];
        _filters![poolId] = _world!.Filter.With<T1>().With<T2>().With<T3>().With<T4>().Build();
    }

    public override int CountWith<T1>(in int poolId) => _filters![poolId].GetLengthSlow(); 
    public override int CountWith<T1, T2>(in int poolId) => _filters![poolId].GetLengthSlow(); 
    public override int CountWith<T1, T2, T3>(in int poolId) => _filters![poolId].GetLengthSlow(); 
    public override int CountWith<T1, T2, T3, T4>(in int poolId) => _filters![poolId].GetLengthSlow();

    public override bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1)
    {
        if (entity == null) return false;
        
        var stashes = _stashes![poolId];
        var s1 = (Stash<T1>)stashes[0];

        var e = (Entity)entity;
        c1 = s1.Get(e);
        return true;
    }
    
    public override bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2)
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
    
    public override bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
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
    
    public override bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
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
    
    public override void Tick(float delta) => _world!.Update(delta);

    public override unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        => _systems.AddSystem(new PointerInvocationStashSystem<T1>(method));

    public override unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        => _systems.AddSystem(new PointerInvocationStashSystem<T1, T2>(method));
    
    public override unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        => _systems.AddSystem(new PointerInvocationStashSystem<T1, T2, T3>(method));

    public override unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        => _systems.AddSystem(new PointerInvocationStashSystem<T1, T2, T3, T4>(method));

    public override void CreateEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity();
    }

    public override void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
    {
        var entities = (Entity[])entitySet;
        var s1 = (Stash<T1>)_stashes![poolId][0];
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            s1.Add(entities[i]) = c1;
        }
    }

    public override void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
        }
    }

    public override void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        var s3 = (Stash<T3>)pool[2];
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
            s3.Add(entities[i]) = c3;
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        var s3 = (Stash<T3>)pool[2];
        var s4 = (Stash<T4>)pool[3];
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
            s3.Add(entities[i]) = c3;
            s4.Add(entities[i]) = c4;
        }
    }

    public override void DeleteEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.RemoveEntity(entities[i]);
    }

    public override void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            s1.Add(entities[i]) = c1;
    }

    public override void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
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

    public override void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
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

    public override void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
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

    public override void RemoveComponent<T1>(in Array entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        var pool = _stashes![poolId];
        var c1 = (Stash<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            c1.Remove(entities[i]);
    }

    public override void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
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

    public override void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
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

    public override void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
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

    public override Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public override Array PrepareSet(in int count) => new Entity[count];
}