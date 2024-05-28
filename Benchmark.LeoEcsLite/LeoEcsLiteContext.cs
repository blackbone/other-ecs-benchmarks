using Benchmark._Context;
using Leopotam.EcsLite;
using EcsWorld = Leopotam.EcsLite.EcsWorld;

namespace Benchmark.LeoEcsLite;

public class LeoEcsLiteContext : BenchmarkContextBase
{
    private int _maxEntityCount;
    
    private EcsWorld? _world;
    private List<IEcsSystems>? _systems;
    private Dictionary<int, IEcsPool[]>? _pools;
    private Dictionary<int, EcsFilter>? _filters;

    public override bool DeletesEntityOnLastComponentDeletion => true;

    public override int EntityCount => _world!.GetEntitiesCount();
    
    public override void Setup(int entityCount)
    {
        _maxEntityCount = entityCount;
        _world = new EcsWorld(new EcsWorld.Config { Entities = entityCount });
        _systems = new List<IEcsSystems>();
        _pools = new Dictionary<int, IEcsPool[]>();
        _filters = new Dictionary<int, EcsFilter>();
    }

    public override void FinishSetup()
    {
        foreach (var system in _systems!)
            system.Init();
    }

    public override void Warmup<T1>(in int poolId)
    {
        _pools![poolId] = [_world!.GetPool<T1>()];
        _filters![poolId] = _world!.Filter<T1>().End(_maxEntityCount);
    }

    public override void Warmup<T1, T2>(in int poolId)
    {
        _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>()];
        _filters![poolId] = _world!.Filter<T1>().Inc<T2>().End(_maxEntityCount);
    }

    public override void Warmup<T1, T2, T3>(in int poolId)
    {
        _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>(), _world!.GetPool<T3>()];
        _filters![poolId] = _world!.Filter<T1>().Inc<T2>().Inc<T3>().End(_maxEntityCount);
    }

    public override void Warmup<T1, T2, T3, T4>(in int poolId)
    {
        _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>(), _world!.GetPool<T3>(), _world!.GetPool<T4>()];
        _filters![poolId] = _world!.Filter<T1>().Inc<T2>().Inc<T3>().Inc<T4>().End(_maxEntityCount);
    }

    public override void Cleanup()
    {
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
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            p1.Add(entities[i]) = c1;
        }
    }

    public override void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
        }
    }

    public override void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
            p3.Add(entities[i]) = c3;
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];
        var p4 = (EcsPool<T4>)pools[3];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
            p3.Add(entities[i]) = c3;
            p4.Add(entities[i]) = c4;
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
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        for (var i = 0; i < entities.Length; i++)
            p1.Add(entities[i]) = c1;
    }

    public override void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        for (var i = 0; i < entities.Length; i++)
        {
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
        }
    }

    public override void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];
        for (var i = 0; i < entities.Length; i++)
        {
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
            p3.Add(entities[i]) = c3;
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];
        var p4 = (EcsPool<T4>)pools[3];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            p1.Add(entities[i]) = c1;
            p2.Add(entities[i]) = c2;
            p3.Add(entities[i]) = c3;
            p4.Add(entities[i]) = c4;
        }
    }

    public override void RemoveComponent<T1>(in Array entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        for (var i = 0; i < entities.Length; i++)
            p1.Del(entities[i]);
    }

    public override void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        for (var i = 0; i < entities.Length; i++)
        {
            p1.Del(entities[i]);
            p2.Del(entities[i]);
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];
        for (var i = 0; i < entities.Length; i++)
        {
            p1.Del(entities[i]);
            p2.Del(entities[i]);
            p3.Del(entities[i]);
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        var pools = _pools![poolId];
        var p1 = (EcsPool<T1>)pools[0];
        var p2 = (EcsPool<T2>)pools[1];
        var p3 = (EcsPool<T3>)pools[2];
        var p4 = (EcsPool<T4>)pools[3];
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.NewEntity();
            p1.Del(entities[i]);
            p2.Del(entities[i]);
            p3.Del(entities[i]);
            p4.Del(entities[i]);
        }
    }

    public override int CountWith<T1>(in int poolId) => _filters![poolId].GetEntitiesCount();

    public override int CountWith<T1, T2>(in int poolId) => _filters![poolId].GetEntitiesCount();

    public override int CountWith<T1, T2, T3>(in int poolId) => _filters![poolId].GetEntitiesCount();

    public override int CountWith<T1, T2, T3, T4>(in int poolId) => _filters![poolId].GetEntitiesCount();
    
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

    public override void Tick(float delta)
    {
        foreach (var system in _systems!)
            system.Run();
    }
    
    public override unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        => _systems!.Add(new EcsSystems(_world!).Add(new System<T1>(method)));

    public override unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        => _systems!.Add(new EcsSystems(_world!).Add(new System<T1, T2>(method)));

    public override unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        => _systems!.Add(new EcsSystems(_world!).Add(new System<T1, T2, T3>(method)));

    public override unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        => _systems!.Add(new EcsSystems(_world!).Add(new System<T1, T2, T3, T4>(method)));

    public override Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((int[])entitySet);
        return entitySet;
    }

    public override Array PrepareSet(in int count)
    {
        return new int[count];
    }
}

