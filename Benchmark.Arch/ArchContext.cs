using System;
using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using Benchmark._Context;
using DCFApixels.DragonECS;
using Scellecs.Morpeh;
using Entity = Arch.Core.Entity;

// ReSharper disable ForCanBeConvertedToForeach

namespace Benchmark.Arch;

public sealed class ArchContext : IBenchmarkContext<Entity>
{
    private readonly Dictionary<int, ComponentType[]> _archetypes = new();
    private readonly Dictionary<int, QueryDescription> _queries = new();
    private readonly List<Action<global::Arch.Core.World>> _systems = new();
    private global::Arch.Core.World _world;

    public bool DeletesEntityOnLastComponentDeletion => false;

    public int NumberOfLivingEntities => _world.Size;

    public void Setup()
    {
        _world = global::Arch.Core.World.Create();
    }

    public void FinishSetup()
    {
    }

    public void Cleanup()
    {
        global::Arch.Core.World.SharedJobScheduler?.Dispose();
        global::Arch.Core.World.SharedJobScheduler = null;

        _world.Clear();
        _world.Dispose();
        _world = null;
        _systems.Clear();
        _archetypes.Clear();
        _queries.Clear();
    }

    public void Dispose()
    {
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _archetypes![poolId] = [typeof(T1)];
        _queries![poolId] = new QueryDescription().WithAll<T1>();
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2)];
        _queries![poolId] = new QueryDescription().WithAll<T1, T2>();
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2), typeof(T3)];
        _queries![poolId] = new QueryDescription().WithAll<T1, T2, T3>();
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2), typeof(T3), typeof(T4)];
        _queries![poolId] = new QueryDescription().WithAll<T1, T2, T3, T4>();
    }

    public void CreateEntities(in global::Arch.Core.Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Create();
    }

    public void CreateEntities<T1>(in global::Arch.Core.Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Create(c1);
    }

    public void CreateEntities<T1, T2>(in global::Arch.Core.Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Create(c1, c2);
    }

    public void CreateEntities<T1, T2, T3>(in global::Arch.Core.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Create(c1, c2, c3);
    }

    public void CreateEntities<T1, T2, T3, T4>(in global::Arch.Core.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Create(c1, c2, c3, c4);
    }

    public void DeleteEntities(in global::Arch.Core.Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                _world.Destroy(entities[i]);
    }

    public void AddComponent<T1>(in global::Arch.Core.Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            _world.Add(entities[i], c1);
    }

    public void AddComponent<T1, T2>(in global::Arch.Core.Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            _world.Add(entities[i], c1, c2);
    }

    public void AddComponent<T1, T2, T3>(in global::Arch.Core.Entity[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            _world.Add(entities[i], c1, c2, c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in global::Arch.Core.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            _world.Add(entities[i], c1, c2, c3, c4);
    }

    public void RemoveComponent<T1>(in global::Arch.Core.Entity[] entities, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var archetype = _archetypes![poolId];
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                _world.RemoveRange(entities[i], archetype);
    }

    public void RemoveComponent<T1, T2>(in global::Arch.Core.Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var archetype = _archetypes![poolId];
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                _world.RemoveRange(entities[i], archetype);
    }

    public void RemoveComponent<T1, T2, T3>(in global::Arch.Core.Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var archetype = _archetypes![poolId];
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                _world.RemoveRange(entities[i], archetype);
    }

    public void RemoveComponent<T1, T2, T3, T4>(in global::Arch.Core.Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var archetype = _archetypes![poolId];
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                _world.RemoveRange(entities[i], archetype);
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.CountEntities(_queries![poolId]);
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.CountEntities(_queries![poolId]);
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.CountEntities(_queries![poolId]);
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.CountEntities(_queries![poolId]);
    }

    public bool GetSingle<T1>(in global::Arch.Core.Entity e, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = _world.Get<T1>(e);
        return true;
    }

    public bool GetSingle<T1, T2>(in global::Arch.Core.Entity e, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var c = e.Get<T1, T2>();
        c1 = c.t0;
        c2 = c.t1;
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in global::Arch.Core.Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var c = e.Get<T1, T2, T3>();
        c1 = c.t0;
        c2 = c.t1;
        c3 = c.t2;
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in global::Arch.Core.Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var c = e.Get<T1, T2, T3, T4>();
        c1 = c.t0;
        c2 = c.t1;
        c3 = c.t2;
        c4 = c.t3;
        return true;
    }

    public void Tick(float delta)
    {
        foreach (var system in _systems!)
            system(_world!);
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var system = new Arch.System<T1>(method);
        _systems.Add(system.ForEachQuery);
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var system = new Arch.System<T1, T2>(method);
        _systems.Add(system.ForEachQuery);
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var system = new Arch.System<T1, T2, T3>(method);
        _systems.Add(system.ForEachQuery);
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var system = new Arch.System<T1, T2, T3, T4>(method);
        _systems.Add(system.ForEachQuery);
    }

    public global::Arch.Core.Entity[] PrepareSet(in int count)
    {
        return count > 0 ? new global::Arch.Core.Entity[count] : [];
    }
}
