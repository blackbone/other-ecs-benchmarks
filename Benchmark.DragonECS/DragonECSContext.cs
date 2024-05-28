using Benchmark._Context;
using DCFApixels.DragonECS;
// ReSharper disable ForCanBeConvertedToForeach

namespace Benchmark.DragonECS;

public class DragonECSContext : BenchmarkContextBase
{
    private EcsWorld? _world;
    private EcsPipeline? _pipeline;
    private Dictionary<int, IEcsPool[]>? _pools;
    private List<IEcsProcess>? _systems;
    
    public override bool DeletesEntityOnLastComponentDeletion => true;

    public override int EntityCount => _world!.Count;
    
    public override void Setup(int entityCount)
    {
        _world = new EcsWorld(new EcsWorldConfig(
            entitiesCapacity: entityCount,
            poolComponentsCapacity: entityCount));

        _pools = new Dictionary<int, IEcsPool[]>();
        _systems = new List<IEcsProcess>();
    }

    public override void FinishSetup()
    {
        var builder = EcsPipeline.New();
        foreach (var system in _systems!)
            builder.Add(system);
        
        _pipeline = builder
            .Inject(_world)
            .Build();
        _pipeline.Init();
    }

    public override void Warmup<T1>(in int poolId) => _pools![poolId] = [_world!.GetPool<T1>()];

    public override void Warmup<T1, T2>(in int poolId) => _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>()];

    public override void Warmup<T1, T2, T3>(in int poolId) => _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>(), _world!.GetPool<T3>()];

    public override void Warmup<T1, T2, T3, T4>(in int poolId) => _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>(), _world!.GetPool<T3>(), _world!.GetPool<T4>()];

    public override void Cleanup()
    {
        _pipeline!.Destroy();
        _pipeline = null;
        
        _world!.Destroy();
        _world = null;
    }

    public override void Lock()
    {
        // no op
    }

    public override void Commit()
    {
        // no op
    }

    public override void CreateEntities(in Array entitySet)
    {
        var entities = (int[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.NewEntity();
    }

    public override void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool = (EcsPool<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            ecsPool.Add(entities[i]) = c1;
        }
    }

    public override void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            ecsPool1.Add(entities[i]) = c1;
            ecsPool2.Add(entities[i]) = c2;
        }
    }

    public override void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            ecsPool1.Add(entities[i]) = c1;
            ecsPool2.Add(entities[i]) = c2;
            ecsPool3.Add(entities[i]) = c3;
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        var ecsPool4 = (EcsPool<T4>)pool[3];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            ecsPool1.Add(entities[i]) = c1;
            ecsPool2.Add(entities[i]) = c2;
            ecsPool3.Add(entities[i]) = c3;
            ecsPool4.Add(entities[i]) = c4;
        }
    }

    public override void DeleteEntities(in Array entitySet)
    {
        var entities = (int[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.DelEntity(entities[i]);
    }

    public override void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            ecsPool1.Add(entities[i]) = c1;
    }

    public override void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.Add(entities[i]) = c1;
            ecsPool2.Add(entities[i]) = c2;
        }
    }

    public override void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.Add(entities[i]) = c1;
            ecsPool2.Add(entities[i]) = c2;
            ecsPool3.Add(entities[i]) = c3;
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        var ecsPool4 = (EcsPool<T4>)pool[3];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.Add(entities[i]) = c1;
            ecsPool2.Add(entities[i]) = c2;
            ecsPool3.Add(entities[i]) = c3;
            ecsPool4.Add(entities[i]) = c4;
        }
    }

    public override void RemoveComponent<T1>(in Array entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        for (var i = 0; i < entities.Length; i++)
            ecsPool1.Del(entities[i]);
    }

    public override void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.Del(entities[i]);
            ecsPool2.Del(entities[i]);
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.Del(entities[i]);
            ecsPool2.Del(entities[i]);
            ecsPool3.Del(entities[i]);
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        var pool = _pools![poolId];
        var ecsPool1 = (EcsPool<T1>)pool[0];
        var ecsPool2 = (EcsPool<T2>)pool[1];
        var ecsPool3 = (EcsPool<T3>)pool[2];
        var ecsPool4 = (EcsPool<T4>)pool[3];
        for (var i = 0; i < entities.Length; i++)
        {
            ecsPool1.Del(entities[i]);
            ecsPool2.Del(entities[i]);
            ecsPool3.Del(entities[i]);
            ecsPool4.Del(entities[i]);
        }
    }

    public override int CountWith<T1>(in int poolId) => _world!.Where(out Aspect<T1> _).Count;
    
    public override int CountWith<T1, T2>(in int poolId) => _world!.Where(out Aspect<T1, T2> _).Count;
    
    public override int CountWith<T1, T2, T3>(in int poolId) => _world!.Where(out Aspect<T1, T2, T3> _).Count;
    
    public override int CountWith<T1, T2, T3, T4>(in int poolId) => _world!.Where(out Aspect<T1, T2, T3, T4> _).Count;
    public override bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1)
    {
        if (entity == null) return false;
        var e = (int)entity;

        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];

        c1 = p1.Get(e);
        return true;
    }

    public override bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2)
    {
        if (entity == null) return false;
        var e = (int)entity;

        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];

        c1 = p1.Get(e);
        c2 = p2.Get(e);
        return true;
    }

    public override bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
    {
        if (entity == null) return false;
        var e = (int)entity;

        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];

        c1 = p1.Get(e);
        c2 = p2.Get(e);
        c3 = p3.Get(e);
        return true;
    }

    public override bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
    {
        if (entity == null) return false;
        var e = (int)entity;

        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];
        var p4 = (EcsPool<T4>)pools[3];

        c1 = p1.Get(e);
        c2 = p2.Get(e);
        c3 = p3.Get(e);
        c4 = p4.Get(e);
        return true;
    }

    public override void Tick(float delta) => _pipeline!.Run();

    public override unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        => _systems?.Add(new PointerInvocationSystem<T1>(_world!, method));

    public override unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        => _systems?.Add(new PointerInvocationSystem<T1, T2>(_world!, method));

    public override unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        => _systems?.Add(new PointerInvocationSystem<T1, T2, T3>(_world!, method));
    
    public override unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        => _systems?.Add(new PointerInvocationSystem<T1, T2, T3, T4>(_world!, method));

    public override Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((int[])entitySet);
        return entitySet;
    }

    public override Array PrepareSet(in int count) => new int[count];
}