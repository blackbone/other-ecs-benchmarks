using Benchmark._Context;
using DCFApixels.DragonECS;

namespace Benchmark.DragonECS;

public class DragonECSContext : BenchmarkContextBase
{
    private EcsWorld? _world;
    private EcsPipeline? _pipeline;
    private Dictionary<int, object[]>? _pools;
    
    public override void Setup(int entityCount)
    {
        _world = new EcsWorld(new EcsWorldConfig(
            entitiesCapacity: entityCount,
            poolComponentsCapacity: entityCount));
        _pipeline = EcsPipeline.New()
            // Добавление систем.
            // .Add(new SomeSystem1())
            // .Add(new SomeSystem2())
            // .Add(new SomeSystem3())

            // Внедрение мира в системы.
            .Inject(_world)
            // Прочие внедрения.
            // .Inject(SomeData)

            // Завершение построения пайплайна.
            .Build();
        
        // Иницивлизация пайплайна и запуск IEcsPreInit.PreInit
        // и IEcsInit.Init у всех добавленных систем.
        _pipeline.Init();

        _pools = new Dictionary<int, object[]>();
    }

    public override void Warmup<T1>(int poolId) => _pools![poolId] = [_world!.GetPool<T1>()];

    public override void Warmup<T1, T2>(int poolId) => _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>()];

    public override void Warmup<T1, T2, T3>(int poolId) => _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>(), _world!.GetPool<T3>()];

    public override void Warmup<T1, T2, T3, T4>(int poolId) => _pools![poolId] = [_world!.GetPool<T1>(), _world!.GetPool<T2>(), _world!.GetPool<T3>(), _world!.GetPool<T4>()];

    public override void Cleanup()
    {
        // Запускает IEcsDestroyInit.Destroy у всех добавленных систем.
        _pipeline!.Destroy();
        _pipeline = null;
        
        // Обязательно удалять миры которые больше не будут использованы.
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

    public override void CreateEntities(in object entitySet)
    {
        var entities = (int[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world!.NewEntity();
    }

    public override void CreateEntities<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool = (EcsPool<T1>)pool[0];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                ecsPool.Add(entities[i]);
            }
        }
        else
        {
            var ecsPool = _world!.GetPool<T1>();
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                ecsPool.Add(entities[i]);
            }
        }
    }

    public override void CreateEntities<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            var ecsPool2 = (EcsPool<T2>)pool[1];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
            }
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            var ecsPool2 = _world!.GetPool<T2>();
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
            }
        }
    }

    public override void CreateEntities<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            var ecsPool2 = (EcsPool<T2>)pool[1];
            var ecsPool3 = (EcsPool<T3>)pool[2];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
                ecsPool3.Add(entities[i]);
            }
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            var ecsPool2 = _world!.GetPool<T2>();
            var ecsPool3 = _world!.GetPool<T3>();
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
                ecsPool3.Add(entities[i]);
            }
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            var ecsPool2 = (EcsPool<T2>)pool[1];
            var ecsPool3 = (EcsPool<T3>)pool[2];
            var ecsPool4 = (EcsPool<T4>)pool[3];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
                ecsPool3.Add(entities[i]);
                ecsPool4.Add(entities[i]);
            }
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            var ecsPool2 = _world!.GetPool<T2>();
            var ecsPool3 = _world!.GetPool<T3>();
            var ecsPool4 = _world!.GetPool<T4>();
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.NewEntity();
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
                ecsPool3.Add(entities[i]);
                ecsPool4.Add(entities[i]);
            }
        }
    }

    public override void DeleteEntities(in object entitySet)
    {
        var entities = (int[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            _world!.DelEntity(entities[i]);
    }

    public override void AddComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            for (int i = 0; i < entities.Length; i++)
                ecsPool1.Add(entities[i]);
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            for (int i = 0; i < entities.Length; i++)
                ecsPool1.Add(entities[i]);
        }
    }

    public override void AddComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            var ecsPool2 = (EcsPool<T2>)pool[1];
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
            }
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            var ecsPool2 = _world!.GetPool<T2>();
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
            }
        }
    }

    public override void AddComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            var ecsPool2 = (EcsPool<T2>)pool[1];
            var ecsPool3 = (EcsPool<T3>)pool[2];
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
                ecsPool3.Add(entities[i]);
            }
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            var ecsPool2 = _world!.GetPool<T2>();
            var ecsPool3 = _world!.GetPool<T3>();
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
                ecsPool3.Add(entities[i]);
            }
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            var ecsPool2 = (EcsPool<T2>)pool[1];
            var ecsPool3 = (EcsPool<T3>)pool[2];
            var ecsPool4 = (EcsPool<T4>)pool[3];
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
                ecsPool3.Add(entities[i]);
                ecsPool4.Add(entities[i]);
            }
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            var ecsPool2 = _world!.GetPool<T2>();
            var ecsPool3 = _world!.GetPool<T3>();
            var ecsPool4 = _world!.GetPool<T4>();
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Add(entities[i]);
                ecsPool2.Add(entities[i]);
                ecsPool3.Add(entities[i]);
                ecsPool4.Add(entities[i]);
            }
        }
    }

    public override void RemoveComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            for (int i = 0; i < entities.Length; i++)
                ecsPool1.Del(entities[i]);
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            for (int i = 0; i < entities.Length; i++)
                ecsPool1.Del(entities[i]);
        }
    }

    public override void RemoveComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            var ecsPool2 = (EcsPool<T2>)pool[1];
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Del(entities[i]);
                ecsPool2.Del(entities[i]);
            }
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            var ecsPool2 = _world!.GetPool<T2>();
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Del(entities[i]);
                ecsPool2.Del(entities[i]);
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            var ecsPool2 = (EcsPool<T2>)pool[1];
            var ecsPool3 = (EcsPool<T3>)pool[2];
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Del(entities[i]);
                ecsPool2.Del(entities[i]);
                ecsPool3.Del(entities[i]);
            }
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            var ecsPool2 = _world!.GetPool<T2>();
            var ecsPool3 = _world!.GetPool<T3>();
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Del(entities[i]);
                ecsPool2.Del(entities[i]);
                ecsPool3.Del(entities[i]);
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (int[])entitySet;
        if (_pools!.TryGetValue(poolId, out var pool))
        {
            var ecsPool1 = (EcsPool<T1>)pool[0];
            var ecsPool2 = (EcsPool<T2>)pool[1];
            var ecsPool3 = (EcsPool<T3>)pool[2];
            var ecsPool4 = (EcsPool<T4>)pool[3];
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Del(entities[i]);
                ecsPool2.Del(entities[i]);
                ecsPool3.Del(entities[i]);
                ecsPool4.Del(entities[i]);
            }
        }
        else
        {
            var ecsPool1 = _world!.GetPool<T1>();
            var ecsPool2 = _world!.GetPool<T2>();
            var ecsPool3 = _world!.GetPool<T3>();
            var ecsPool4 = _world!.GetPool<T4>();
            for (int i = 0; i < entities.Length; i++)
            {
                ecsPool1.Del(entities[i]);
                ecsPool2.Del(entities[i]);
                ecsPool3.Del(entities[i]);
                ecsPool4.Del(entities[i]);
            }
        }
    }

    public override object Shuffle(in object entitySet)
    {
        Random.Shared.Shuffle((int[])entitySet);
        return entitySet;
    }

    public override object PrepareSet(in int count) => new int[count];
}