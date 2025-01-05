using System.Collections.Generic;
using System.Linq;
using Benchmark._Context;
using DCFApixels.DragonECS;
using DefaultEcs;
using DefaultEcs.System;
using Scellecs.Morpeh;
using Entity = DefaultEcs.Entity;

namespace Benchmark.DefaultECS;

public sealed class DefaultECSContext : IBenchmarkContext<Entity>
{
    private readonly List<ISystem<float>> _systems = new();
    private readonly Dictionary<int, EntityQueryBuilder> _queries = new();
    private DefaultEcs.World _world;

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int NumberOfLivingEntities => _world.Count();

    public void Setup()
    {
        _world = new DefaultEcs.World();
    }

    public void FinishSetup()
    {
    }

    public void Cleanup()
    {
        _systems.Clear();
        _world.Dispose();
        _world = null;
    }

    public void Dispose()
    {
    }

    public void Warmup<T1>(in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _queries![poolId] = _world.GetEntities().With<T1>();

    public void Warmup<T1, T2>(in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _queries![poolId] = _world.GetEntities().With<T1>().With<T2>();

    public void Warmup<T1, T2, T3>(in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _queries![poolId] = _world.GetEntities().With<T1>().With<T2>().With<T3>();

    public void Warmup<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _queries![poolId] = _world.GetEntities().With<T1>().With<T2>().With<T3>().With<T4>();

    public void DeleteEntities(in DefaultEcs.Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive) entities[i].Dispose();
    }

    public DefaultEcs.Entity[] PrepareSet(in int count)
    {
        return count > 0 ? new DefaultEcs.Entity[count] : [];
    }

    public void CreateEntities(in DefaultEcs.Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.CreateEntity();
    }

    public void CreateEntities<T1>(in DefaultEcs.Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.CreateEntity();
            entities[i].Set(c1);
        }
    }

    public void CreateEntities<T1, T2>(in DefaultEcs.Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.CreateEntity();
            entities[i].Set(c1);
            entities[i].Set(c2);
        }
    }

    public void CreateEntities<T1, T2, T3>(in DefaultEcs.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2,
        in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.CreateEntity();
            entities[i].Set(c1);
            entities[i].Set(c2);
            entities[i].Set(c3);
        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in DefaultEcs.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2,
        in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.CreateEntity();
            entities[i].Set(c1);
            entities[i].Set(c2);
            entities[i].Set(c3);
            entities[i].Set(c4);
        }
    }

    public void AddComponent<T1>(in DefaultEcs.Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++) entities[i].Set(c1);
    }

    public void AddComponent<T1, T2>(in DefaultEcs.Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Set(c1);
            entities[i].Set(c2);
        }
    }

    public void AddComponent<T1, T2, T3>(in DefaultEcs.Entity[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Set(c1);
            entities[i].Set(c2);
            entities[i].Set(c3);
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in DefaultEcs.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2,
        in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Set(c1);
            entities[i].Set(c2);
            entities[i].Set(c3);
            entities[i].Set(c4);
        }
    }

    public void RemoveComponent<T1>(in DefaultEcs.Entity[] entities, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++) entities[i].Remove<T1>();
    }

    public void RemoveComponent<T1, T2>(in DefaultEcs.Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Remove<T1>();
            entities[i].Remove<T2>();
        }
    }

    public void RemoveComponent<T1, T2, T3>(in DefaultEcs.Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Remove<T1>();
            entities[i].Remove<T2>();
            entities[i].Remove<T3>();
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in DefaultEcs.Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Remove<T1>();
            entities[i].Remove<T2>();
            entities[i].Remove<T3>();
            entities[i].Remove<T4>();
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _queries![poolId].AsEnumerable().Count();
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _queries![poolId].AsEnumerable().Count();
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _queries![poolId].AsEnumerable().Count();
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _queries![poolId].AsEnumerable().Count();
    }

    public bool GetSingle<T1>(in DefaultEcs.Entity e, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.Get<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in DefaultEcs.Entity e, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in DefaultEcs.Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in DefaultEcs.Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
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

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems.Add(new System<T1>(_queries![poolId], method));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems.Add(new System<T1, T2>(_queries![poolId], method));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems.Add(new System<T1, T2, T3>(_queries![poolId], method));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems.Add(new System<T1, T2, T3, T4>(_queries![poolId], method));
    }
}
