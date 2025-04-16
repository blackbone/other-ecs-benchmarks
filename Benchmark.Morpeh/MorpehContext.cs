using System.Collections.Generic;
using System.Linq;
using Benchmark.Context;
using DCFApixels.DragonECS;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Workaround;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming

namespace Benchmark.Morpeh;

public sealed class MorpehContext : IBenchmarkContext<Entity>
{
    private readonly Dictionary<int, Filter> _filters = new();
    private readonly Dictionary<int, IStash[]> _stashes = new();
    private SystemsGroup _systems;
    private World _world;

    public bool DeletesEntityOnLastComponentDeletion => true;

    public int NumberOfLivingEntities => _world!.EntityCount();

    public void Setup()
    {
        _world = World.Create();
        _systems = _world.CreateSystemsGroup();
        _world.AddSystemsGroup(0, _systems);
    }

    public void FinishSetup()
    {
        // _world!.WarmupArchetypes(entityCount);
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

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _stashes![poolId] = [_world!.GetStash<T1>()];
        _filters![poolId] = _world!.Filter.With<T1>().Build();
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _stashes![poolId] = [_world!.GetStash<T1>(), _world!.GetStash<T2>()];
        _filters![poolId] = _world!.Filter.With<T1>().With<T2>().Build();
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _stashes![poolId] = [_world!.GetStash<T1>(), _world!.GetStash<T2>(), _world!.GetStash<T3>()];
        _filters![poolId] = _world!.Filter.With<T1>().With<T2>().With<T3>().Build();
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _stashes![poolId] = [_world!.GetStash<T1>(), _world!.GetStash<T2>(), _world!.GetStash<T3>(), _world!.GetStash<T4>()];
        _filters![poolId] = _world!.Filter.With<T1>().With<T2>().With<T3>().With<T4>().Build();
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _filters![poolId].GetLengthSlow();
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _filters![poolId].GetLengthSlow();
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _filters![poolId].GetLengthSlow();
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _filters![poolId].GetLengthSlow();
    }

    public bool GetSingle<T1>(in Entity e, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var stashes = _stashes![poolId];
        var s1 = (Stash<T1>)_stashes![poolId][0];

        c1 = s1.Get(e);
        return true;
    }

    public bool GetSingle<T1, T2>(in Entity e, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {

        var stashes = _stashes![poolId];
        var s1 = (Stash<T1>)stashes[0];
        var s2 = (Stash<T2>)stashes[1];

        c1 = s1.Get(e);
        c2 = s2.Get(e);
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {

        var stashes = _stashes![poolId];
        var s1 = (Stash<T1>)stashes[0];
        var s2 = (Stash<T2>)stashes[1];
        var s3 = (Stash<T3>)stashes[2];

        c1 = s1.Get(e);
        c2 = s2.Get(e);
        c3 = s3.Get(e);
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {

        var stashes = _stashes![poolId];
        var s1 = (Stash<T1>)stashes[0];
        var s2 = (Stash<T2>)stashes[1];
        var s3 = (Stash<T3>)stashes[2];
        var s4 = (Stash<T4>)stashes[3];

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
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems.AddSystem(new PointerInvocationStashSystem<T1>(method));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems.AddSystem(new PointerInvocationStashSystem<T1, T2>(method));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems.AddSystem(new PointerInvocationStashSystem<T1, T2, T3>(method));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems.AddSystem(new PointerInvocationStashSystem<T1, T2, T3, T4>(method));
    }

    public void CreateEntities(in Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity();

        _world!.Commit();
    }

    public void CreateEntities<T1>(in Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var s1 = (Stash<T1>)_stashes![poolId][0];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            s1.Add(entities[i]) = c1;
        }

        _world!.Commit();
    }

    public void CreateEntities<T1, T2>(in Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
        }

        _world!.Commit();
    }

    public void CreateEntities<T1, T2, T3>(in Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
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

        _world!.Commit();
    }

    public void CreateEntities<T1, T2, T3, T4>(in Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
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

        _world!.Commit();
    }

    public void DeleteEntities(in Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            _world!.RemoveEntity(entities[i]);

        _world!.Commit();
    }

    public void AddComponent<T1>(in Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            s1.Add(entities[i]) = c1;

        _world!.Commit();
    }

    public void AddComponent<T1, T2>(in Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _stashes![poolId];
        var s1 = (Stash<T1>)pool[0];
        var s2 = (Stash<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            s1.Add(entities[i]) = c1;
            s2.Add(entities[i]) = c2;
        }

        _world!.Commit();
    }

    public void AddComponent<T1, T2, T3>(in Entity[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
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

        _world!.Commit();
    }

    public void AddComponent<T1, T2, T3, T4>(in Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
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

        _world!.Commit();
    }

    public void RemoveComponent<T1>(in Entity[] entities, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _stashes![poolId];
        var c1 = (Stash<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            c1.Remove(entities[i]);

        _world!.Commit();
    }

    public void RemoveComponent<T1, T2>(in Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _stashes![poolId];
        var c1 = (Stash<T1>)pool[0];
        var c2 = (Stash<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            c1.Remove(entities[i]);
            c2.Remove(entities[i]);
        }

        _world!.Commit();
    }

    public void RemoveComponent<T1, T2, T3>(in Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
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

    public void RemoveComponent<T1, T2, T3, T4>(in Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
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

        _world!.Commit();
    }

    public Entity[] PrepareSet(in int count)
    {
        return count > 0 ? new Entity[count] : [];
    }
}
