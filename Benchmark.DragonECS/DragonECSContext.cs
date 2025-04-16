using System;
using System.Collections.Generic;
using System.Linq;
using Benchmark.Context;
using DCFApixels.DragonECS;
using Scellecs.Morpeh;

// ReSharper disable ForCanBeConvertedToForeach

namespace Benchmark.DragonECS;

public sealed class DragonECSContext : IBenchmarkContext<entlong>
{
    private readonly Dictionary<int, IEcsPool[]> _pools = new();
    private readonly List<IEcsProcess> _systems = new();
    private EcsPipeline _pipeline;
    private EcsWorld _world;

    public bool DeletesEntityOnLastComponentDeletion => true;

    public int NumberOfLivingEntities => _world.Count;

    public void Setup()
    {
        _world = new EcsWorld(new EcsWorldConfig(), 0);
    }

    public void FinishSetup()
    {
        var builder = EcsPipeline.New();
        foreach (var system in _systems!)
            builder.Add(system);

        _pipeline = builder
            .Inject(_world)
            .Build();
        _pipeline.Init();
    }

    public void Cleanup()
    {
        foreach (var pool in _pools.Values.SelectMany(p => p))
            pool.ClearAll();
    }

    public void Dispose()
    {
        _pipeline.Destroy();
        _pipeline = null;

        _systems.Clear();

        _world.Destroy();
        _world = null;
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _pools![poolId] = [_world.GetPool<T1>()];
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _pools![poolId] = [_world.GetPool<T1>(), _world.GetPool<T2>()];
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _pools![poolId] = [_world.GetPool<T1>(), _world.GetPool<T2>(), _world.GetPool<T3>()];
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _pools![poolId] = [_world.GetPool<T1>(), _world.GetPool<T2>(), _world.GetPool<T3>(), _world.GetPool<T4>()];
    }

    public void CreateEntities(in entlong[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.NewEntityLong();
    }

    public void CreateEntities<T1>(in entlong[] entities, in int poolId, in T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool = (EcsPool<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.NewEntityLong();
            ecsPool.Add(entities[i].ID) = c1;
        }
    }

    public void CreateEntities<T1, T2>(in entlong[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.NewEntityLong();
            ecsPool1.Add(entities[i].ID) = c1;
            ecsPool2.Add(entities[i].ID) = c2;
        }
    }

    public void CreateEntities<T1, T2, T3>(in entlong[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.NewEntityLong();
            ecsPool1.Add(entities[i].ID) = c1;
            ecsPool2.Add(entities[i].ID) = c2;
            ecsPool3.Add(entities[i].ID) = c3;
        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in entlong[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        var ecsPool4 = (EcsPool<T4>)pool[3];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.NewEntityLong();
            ecsPool1.Add(entities[i].ID) = c1;
            ecsPool2.Add(entities[i].ID) = c2;
            ecsPool3.Add(entities[i].ID) = c3;
            ecsPool4.Add(entities[i].ID) = c4;
        }
    }

    public void DeleteEntities(in entlong[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive)
                _world.DelEntity(entities[i]);

        _world.ReleaseDelEntityBufferAll();
    }

    public void AddComponent<T1>(in entlong[] entities, in int poolId, in T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            ecsPool1.Add(entities[i].ID) = c1;
    }

    public void AddComponent<T1, T2>(in entlong[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.Add(entities[i].ID) = c1;
            ecsPool2.Add(entities[i].ID) = c2;
        }
    }

    public void AddComponent<T1, T2, T3>(in entlong[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.Add(entities[i].ID) = c1;
            ecsPool2.Add(entities[i].ID) = c2;
            ecsPool3.Add(entities[i].ID) = c3;
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in entlong[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        var ecsPool4 = (EcsPool<T4>)pool[3];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.Add(entities[i].ID) = c1;
            ecsPool2.Add(entities[i].ID) = c2;
            ecsPool3.Add(entities[i].ID) = c3;
            ecsPool4.Add(entities[i].ID) = c4;
        }
    }

    public void RemoveComponent<T1>(in entlong[] entities, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            ecsPool1.TryDel(entities[i].ID);
    }

    public void RemoveComponent<T1, T2>(in entlong[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.TryDel(entities[i].ID);
            ecsPool2.TryDel(entities[i].ID);
        }
    }

    public void RemoveComponent<T1, T2, T3>(in entlong[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];

        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.TryDel(entities[i].ID);
            ecsPool2.TryDel(entities[i].ID);
            ecsPool3.TryDel(entities[i].ID);
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in entlong[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        var ecsPool4 = (EcsPool<T4>)pool[3];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.TryDel(entities[i].ID);
            ecsPool2.TryDel(entities[i].ID);
            ecsPool3.TryDel(entities[i].ID);
            ecsPool4.TryDel(entities[i].ID);
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.Where(out Aspect<T1> _).Count;
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.Where(out Aspect<T1, T2> _).Count;
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.Where(out Aspect<T1, T2, T3> _).Count;
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.Where(out Aspect<T1, T2, T3, T4> _).Count;
    }

    public bool GetSingle<T1>(in entlong e, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];

        c1 = p1.Get(e.ID);
        return true;
    }

    public bool GetSingle<T1, T2>(in entlong e, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];

        c1 = p1.Get(e.ID);
        c2 = p2.Get(e.ID);
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in entlong e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];

        c1 = p1.Get(e.ID);
        c2 = p2.Get(e.ID);
        c3 = p3.Get(e.ID);
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in entlong e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];
        var p4 = (EcsPool<T4>)pools[3];

        c1 = p1.Get(e.ID);
        c2 = p2.Get(e.ID);
        c3 = p3.Get(e.ID);
        c4 = p4.Get(e.ID);
        return true;
    }

    public void Tick(float delta)
    {
        _pipeline.Run();
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems?.Add(new PointerInvocationSystem<T1>(_world!, method));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems?.Add(new PointerInvocationSystem<T1, T2>(_world!, method));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems?.Add(new PointerInvocationSystem<T1, T2, T3>(_world!, method));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _systems?.Add(new PointerInvocationSystem<T1, T2, T3, T4>(_world!, method));
    }

    public entlong[] PrepareSet(in int count)
    {
        return count > 0 ? new entlong[count] : [];
    }
}
