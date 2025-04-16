using System.Collections.Generic;
using System.Linq;
using Benchmark.Context;
using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using XenoComponent = Xeno.IComponent;
using FrifloComponent = Friflo.Engine.ECS.IComponent;
using StaticEcsComponent = FFS.Libraries.StaticEcs.IComponent;

namespace Benchmark.FriFlo;

#pragma warning disable CS9113 // Parameter is unread.
public class FrifloContext : IBenchmarkContext<Entity>
#pragma warning restore CS9113 // Parameter is unread.
{
    private readonly Dictionary<int, ArchetypeQuery> _queries = new();
    private EntityStore _world;
    private SystemRoot _root;

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int NumberOfLivingEntities => _world.Count;

    public void Dispose()
    {
        _root = null;
        _world = null;
    }

    public void Setup()
    {
        _world = new EntityStore(PidType.UsePidAsId);
        _root = new SystemRoot(_world);
    }

    public void FinishSetup()
    {
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        _queries[poolId] = _world.Query<T1>();
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        _queries[poolId] = _world.Query<T1, T2>();
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        _queries[poolId] = _world.Query<T1, T2, T3>();
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T4 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        _queries[poolId] = _world.Query<T1, T2, T3, T4>();
    }

    public void Cleanup()
    {
        foreach (var entity in _world.Entities)
            entity.DeleteEntity();
    }

    public void DeleteEntities(in Entity[] entities)
    {
        // TODO: batch
        foreach (var entity in entities)
            if (entity != default)
                entity.DeleteEntity();
    }

    public Entity[] PrepareSet(in int count) => new Entity[count];

    public void CreateEntities(in Entity[] entities)
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world.CreateEntity();
    }

    public void CreateEntities<T1>(in Entity[] entities, in int poolId, in T1 c1) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.CreateEntity();
            entities[i].AddComponent(c1);
        }
    }

    public void CreateEntities<T1, T2>(in Entity[] entities, in int poolId, in T1 c1, in T2 c2) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.CreateEntity();
            entities[i].AddComponent(c1);
            entities[i].AddComponent(c2);
        }
    }

    public void CreateEntities<T1, T2, T3>(in Entity[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.CreateEntity();
            entities[i].AddComponent(c1);
            entities[i].AddComponent(c2);
            entities[i].AddComponent(c3);
        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in Entity[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3, in T4 c4) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T4 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.CreateEntity();
            entities[i].AddComponent(c1);
            entities[i].AddComponent(c2);
            entities[i].AddComponent(c3);
            entities[i].AddComponent(c4);
        }
    }

    public void AddComponent<T1>(in Entity[] entities, in int poolId, in T1 c1) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].AddComponent(c1);
        }
    }

    public void AddComponent<T1, T2>(in Entity[] entities, in int poolId, in T1 c1, in T2 c2) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].AddComponent(c1);
            entities[i].AddComponent(c2);
        }
    }

    public void AddComponent<T1, T2, T3>(in Entity[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].AddComponent(c1);
            entities[i].AddComponent(c2);
            entities[i].AddComponent(c3);
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in Entity[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3, in T4 c4) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T4 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].AddComponent(c1);
            entities[i].AddComponent(c2);
            entities[i].AddComponent(c3);
            entities[i].AddComponent(c4);
        }
    }

    public void RemoveComponent<T1>(in Entity[] entities, in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].RemoveComponent<T1>();
        }
    }

    public void RemoveComponent<T1, T2>(in Entity[] entities, in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].RemoveComponent<T1>();
            entities[i].RemoveComponent<T2>();
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Entity[] entities, in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].RemoveComponent<T1>();
            entities[i].RemoveComponent<T2>();
            entities[i].RemoveComponent<T3>();
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Entity[] entities, in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T4 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        // TODO: batch
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].RemoveComponent<T1>();
            entities[i].RemoveComponent<T2>();
            entities[i].RemoveComponent<T3>();
            entities[i].RemoveComponent<T4>();
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        => ((ArchetypeQuery<T1>)_queries[poolId]).Count;

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        => ((ArchetypeQuery<T1, T2>)_queries[poolId]).Count;

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        => ((ArchetypeQuery<T1, T2, T3>)_queries[poolId]).Count;

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T4 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        => ((ArchetypeQuery<T1, T2, T3, T4>)_queries[poolId]).Count;

    public bool GetSingle<T1>(in Entity e, in int poolId, ref T1 c1) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        if (CountWith<T1>(poolId) == 0) return false;
        c1 = ((ArchetypeQuery<T1>)_queries[poolId]).Entities.First().GetComponent<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in Entity e, in int poolId, ref T1 c1, ref T2 c2) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        c1 = e.GetComponent<T1>();
        c2 = e.GetComponent<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        c1 = e.GetComponent<T1>();
        c2 = e.GetComponent<T2>();
        c3 = e.GetComponent<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T4 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        c1 = e.GetComponent<T1>();
        c2 = e.GetComponent<T2>();
        c3 = e.GetComponent<T3>();
        c4 = e.GetComponent<T4>();
        return true;
    }

    public void Tick(float delta) => _root.Update(default);

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        => _root.Add(new FriFloSystem<T1>(method));

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        => _root.Add(new FriFloSystem<T1, T2>(method));

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        => _root.Add(new FriFloSystem<T1, T2, T3>(method));

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T2 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T3 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent where T4 : struct, Scellecs.Morpeh.IComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        => _root.Add(new FriFloSystem<T1, T2, T3, T4>(method));
}
