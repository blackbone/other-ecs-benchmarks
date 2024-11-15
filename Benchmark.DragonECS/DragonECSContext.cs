using Benchmark._Context;
using DCFApixels.DragonECS;
using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using XenoComponent = Xeno.IComponent;
using FrifloComponent = Friflo.Engine.ECS.IComponent;

// ReSharper disable ForCanBeConvertedToForeach

namespace Benchmark.DragonECS;

public sealed class DragonECSContext : IBenchmarkContext
{
    private readonly Dictionary<int, IEcsPool[]> _pools = new();
    private readonly List<IEcsProcess> _systems = new();
    private EcsPipeline _pipeline;
    private EcsWorld _world;

    public bool DeletesEntityOnLastComponentDeletion => true;

    public int EntityCount => _world.Count;

    public void Setup()
    {
        _world = new EcsWorld(new EcsWorldConfig());
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
        _world.ReleaseDelEntityBufferAll();
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

    public void Warmup<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _pools![poolId] = [_world.GetPool<T1>()];
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _pools![poolId] = [_world.GetPool<T1>(), _world.GetPool<T2>()];
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _pools![poolId] = [_world.GetPool<T1>(), _world.GetPool<T2>(), _world.GetPool<T3>()];
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _pools![poolId] = [_world.GetPool<T1>(), _world.GetPool<T2>(), _world.GetPool<T3>(), _world.GetPool<T4>()];
    }

    public void CreateEntities(in Array entitySet)
    {
        var entities = (entlong[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.NewEntityLong();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
        var pool = _pools![poolId];
        var ecsPool = (EcsPool<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world.NewEntityLong();
            ecsPool.Add(entities[i].ID) = c1;
        }
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
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

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
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

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
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

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (entlong[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive)
                _world.DelEntity(entities[i]);
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            ecsPool1.Add(entities[i].ID) = c1;
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.Add(entities[i].ID) = c1;
            ecsPool2.Add(entities[i].ID) = c2;
        }
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
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

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
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

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            ecsPool1.TryDel(entities[i].ID);
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.TryDel(entities[i].ID);
            ecsPool2.TryDel(entities[i].ID);
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
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

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (entlong[])entitySet;
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

    public int CountWith<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _world.Where(out Aspect<T1> _).Count;
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _world.Where(out Aspect<T1, T2> _).Count;
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _world.Where(out Aspect<T1, T2, T3> _).Count;
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _world.Where(out Aspect<T1, T2, T3, T4> _).Count;
    }

    public bool GetSingle<T1>(in object entity, in int poolId, ref T1 c1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;
        var e = (entlong)entity;

        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];

        c1 = p1.Get(e.ID);
        return true;
    }

    public bool GetSingle<T1, T2>(in object entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;
        var e = (entlong)entity;

        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];

        c1 = p1.Get(e.ID);
        c2 = p2.Get(e.ID);
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;
        var e = (entlong)entity;

        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];

        c1 = p1.Get(e.ID);
        c2 = p2.Get(e.ID);
        c3 = p3.Get(e.ID);
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;
        var e = (entlong)entity;

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
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _systems?.Add(new PointerInvocationSystem<T1>(_world!, method));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _systems?.Add(new PointerInvocationSystem<T1, T2>(_world!, method));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _systems?.Add(new PointerInvocationSystem<T1, T2, T3>(_world!, method));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _systems?.Add(new PointerInvocationSystem<T1, T2, T3, T4>(_world!, method));
    }

    public Array PrepareSet(in int count)
    {
        return count > 0 ? new entlong[count] : [];
    }
}
