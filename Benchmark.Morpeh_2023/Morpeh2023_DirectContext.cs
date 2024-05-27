using Benchmark._Context;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Workaround;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming

namespace Benchmark.Morpeh_2023;

public class Morpeh2023_DirectContext : BenchmarkContextBase
{
    private World? _world;
    private SystemsGroup? _systems;
    
    public override int EntityCount => _world!.EntityCount();

    public override void Setup(int entityCount)
    {
        _world = World.Create();
        _systems = _world.CreateSystemsGroup();
        _world.AddSystemsGroup(0, _systems);
    }

    public override void Cleanup()
    {
        _world?.Dispose();
        _world = null;
    }

    public override void Lock()
    {
        /* no op */
    }

    public override void Commit()
    {
        _world!.Commit();
    }

    public override void Warmup<T1>(int poolId)
    {
    }

    public override void Warmup<T1, T2>(int poolId)
    {
    }

    public override void Warmup<T1, T2, T3>(int poolId)
    {
    }

    public override void Warmup<T1, T2, T3, T4>(int poolId)
    {
    }

    public override void CreateEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity();
    }

    public override void CreateEntities<T1>(in object entitySet, in int poolId = -1, in T1 c1 = default)
    {
        var entities = (Entity[])entitySet;

        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].AddComponent<T1>() = c1;
        }
    }

    public override void CreateEntities<T1, T2>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].AddComponent<T1>() = c1;
            entities[i].AddComponent<T2>() = c2;
        }
    }

    public override void CreateEntities<T1, T2, T3>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].AddComponent<T1>() = c1;
            entities[i].AddComponent<T2>() = c2;
            entities[i].AddComponent<T3>() = c3;
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = _world!.CreateEntity();
            entities[i].AddComponent<T1>() = c1;
            entities[i].AddComponent<T2>() = c2;
            entities[i].AddComponent<T3>() = c3;
            entities[i].AddComponent<T4>() = c4;
        }
    }

    public override void DeleteEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.RemoveEntity(entities[i]);
    }

    public override void AddComponent<T1>(in object entitySet, in int poolId = -1, in T1 c1 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponent<T1>() = c1;
    }

    public override void AddComponent<T1, T2>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].AddComponent<T1>() = c1;
            entities[i].AddComponent<T2>() = c2;
        }
    }

    public override void AddComponent<T1, T2, T3>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].AddComponent<T1>() = c1;
            entities[i].AddComponent<T2>() = c2;
            entities[i].AddComponent<T3>() = c3;
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].AddComponent<T1>() = c1;
            entities[i].AddComponent<T2>() = c2;
            entities[i].AddComponent<T3>() = c3;
            entities[i].AddComponent<T4>() = c4;
        }
    }

    public override void RemoveComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].RemoveComponent<T1>();
    }

    public override void RemoveComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].RemoveComponent<T1>();
            entities[i].RemoveComponent<T2>();
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].RemoveComponent<T1>();
            entities[i].RemoveComponent<T2>();
            entities[i].RemoveComponent<T3>();
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].RemoveComponent<T1>();
            entities[i].RemoveComponent<T2>();
            entities[i].RemoveComponent<T3>();
            entities[i].RemoveComponent<T4>();
        }
    }

    public override int CountWith<T1>(int poolId) => _world!.Filter.With<T1>().Build().GetLengthSlow();
    public override int CountWith<T1, T2>(int poolId) => _world!.Filter.With<T1>().With<T2>().Build().GetLengthSlow();
    public override int CountWith<T1, T2, T3>(int poolId) => _world!.Filter.With<T1>().With<T2>().With<T3>().Build().GetLengthSlow();
    public override int CountWith<T1, T2, T3, T4>(int poolId) => _world!.Filter.With<T1>().With<T2>().With<T3>().With<T4>().Build().GetLengthSlow();
    public override void Tick(float delta) => _world.Update(delta);

    public override unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        => _systems.AddSystem(new PointerInvocationDirectSystem<T1>(method));

    public override unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        => _systems.AddSystem(new PointerInvocationDirectSystem<T1, T2>(method));
    
    public override unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        => _systems.AddSystem(new PointerInvocationDirectSystem<T1, T2, T3>(method));

    public override unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        => _systems.AddSystem(new PointerInvocationDirectSystem<T1, T2, T3, T4>(method));

    public override object Shuffle(in object entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public override object PrepareSet(in int count) => new Entity[count];
}