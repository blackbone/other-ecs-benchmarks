using Benchmark._Context;
using DCFApixels.DragonECS;
using Leopotam.Ecs;
using EcsWorld = Leopotam.Ecs.EcsWorld;
using EcsWorldConfig = Leopotam.Ecs.EcsWorldConfig;

namespace Benchmark.LeoEcs;

public class LeoEcsContext : BenchmarkContextBase
{
    private EcsWorld? _world;
    private Dictionary<int, object[]>? _pools;
    
    public override void Setup(int entityCount)
    {
        _world = new EcsWorld(new EcsWorldConfig
        {
            EntityComponentsCacheSize = entityCount,
            WorldEntitiesCacheSize = entityCount
        });

        _pools = new Dictionary<int, object[]>();
    }

    public override void Warmup<T1>(int poolId) => _pools![poolId] = [_world!.GetPool<T1>()];

    public override void Warmup<T1, T2>(int poolId) => _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>()];

    public override void Warmup<T1, T2, T3>(int poolId) => _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>(), _world!.GetPool<T3>()];

    public override void Warmup<T1, T2, T3, T4>(int poolId) => _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>(), _world!.GetPool<T3>(), _world!.GetPool<T4>()];

    public override void Cleanup()
    {
        _world!.Destroy();
        _world = null;
        
        _pools!.Clear();
    }

    public override void Lock()
    {
        // no op
    }

    public override void Commit()
    {
        // no op
    }

    public override void CreateEntities(in object entitySet)
    {
        var entities = (EcsEntity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.NewEntity();
    }

    public override void CreateEntities<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                c1.Add(entities[i].GetInternalId());
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                entities[i].Replace(default(T1));
            }
        }
    }

    public override void CreateEntities<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            var c2 = (EcsPool<T2>)pools[1];
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                c1.Add(entities[i].GetInternalId());
                c2.Add(entities[i].GetInternalId());
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                entities[i].Replace(default(T1));
                entities[i].Replace(default(T2));
            }
        }
    }

    public override void CreateEntities<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            var c2 = (EcsPool<T2>)pools[1];
            var c3 = (EcsPool<T3>)pools[2];
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                c1.Add(entities[i].GetInternalId());
                c2.Add(entities[i].GetInternalId());
                c3.Add(entities[i].GetInternalId());
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                entities[i].Replace(default(T1));
                entities[i].Replace(default(T2));
                entities[i].Replace(default(T3));
            }
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            var c2 = (EcsPool<T2>)pools[1];
            var c3 = (EcsPool<T3>)pools[2];
            var c4 = (EcsPool<T4>)pools[3];
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                c1.Add(entities[i].GetInternalId());
                c2.Add(entities[i].GetInternalId());
                c3.Add(entities[i].GetInternalId());
                c4.Add(entities[i].GetInternalId());
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                entities[i].Replace(default(T1));
                entities[i].Replace(default(T2));
                entities[i].Replace(default(T3));
                entities[i].Replace(default(T4));
            }
        }
    }

    public override void DeleteEntities(in object entitySet)
    {
        throw new NotImplementedException();
    }

    public override void AddComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            for (var i = 0; i < entities.Length; i++)
                c1.Add(entities[i].GetInternalId());
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
                entities[i].Replace(default(T1));
        }
    }

    public override void AddComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            var c2 = (EcsPool<T2>)pools[1];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Add(entities[i].GetInternalId());
                c2.Add(entities[i].GetInternalId());
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].Replace(default(T1));
                entities[i].Replace(default(T2));
            }
        }
    }

    public override void AddComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            var c2 = (EcsPool<T2>)pools[1];
            var c3 = (EcsPool<T3>)pools[2];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Add(entities[i].GetInternalId());
                c2.Add(entities[i].GetInternalId());
                c3.Add(entities[i].GetInternalId());
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].Replace(default(T1));
                entities[i].Replace(default(T2));
                entities[i].Replace(default(T3));
            }
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            var c2 = (EcsPool<T2>)pools[1];
            var c3 = (EcsPool<T3>)pools[2];
            var c4 = (EcsPool<T4>)pools[3];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Add(entities[i].GetInternalId());
                c2.Add(entities[i].GetInternalId());
                c3.Add(entities[i].GetInternalId());
                c4.Add(entities[i].GetInternalId());
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].Replace(default(T1));
                entities[i].Replace(default(T2));
                entities[i].Replace(default(T3));
                entities[i].Replace(default(T4));
            }
        }
    }

    public override void RemoveComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            for (var i = 0; i < entities.Length; i++)
                c1.Del(entities[i].GetInternalId());
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
                entities[i].Del<T1>();
        }
    }

    public override void RemoveComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            var c2 = (EcsPool<T2>)pools[1];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Del(entities[i].GetInternalId());
                c2.Del(entities[i].GetInternalId());
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].Del<T1>();
                entities[i].Del<T2>();
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            var c2 = (EcsPool<T2>)pools[1];
            var c3 = (EcsPool<T3>)pools[2];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Del(entities[i].GetInternalId());
                c2.Del(entities[i].GetInternalId());
                c3.Del(entities[i].GetInternalId());
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].Del<T1>();
                entities[i].Del<T2>();
                entities[i].Del<T3>();
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (EcsEntity[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pools))
        {
            var c1 = (EcsPool<T1>)pools[0];
            var c2 = (EcsPool<T2>)pools[1];
            var c3 = (EcsPool<T3>)pools[2];
            var c4 = (EcsPool<T4>)pools[3];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Del(entities[i].GetInternalId());
                c2.Del(entities[i].GetInternalId());
                c3.Del(entities[i].GetInternalId());
                c4.Del(entities[i].GetInternalId());
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].Del<T1>();
                entities[i].Del<T2>();
                entities[i].Del<T3>();
                entities[i].Del<T4>();
            }
        }
    }

    public override object Shuffle(in object entitySet)
    {
        Random.Shared.Shuffle((EcsEntity[])entitySet);
        return entitySet;
    }

    public override object PrepareSet(in int count)
    {
        return new EcsEntity[count];
    }
}

