using Arch.Core;
using Arch.Core.Utils;
using Benchmark._Context;

namespace Benchmark.Arch;

public class ArchContext : BenchmarkContextBase
{
    private World world;
    private Entity[] entities;
    private Dictionary<int, ComponentType[]> archetypes;
    private int n = -1;

    public override void Setup(int entityCount)
    {
        world = World.Create();
        entities = new Entity[entityCount];
        archetypes = new Dictionary<int, ComponentType[]>();
        n = -1;
    }

    public override void Cleanup()
    {
        world?.Clear();
        world?.Dispose();
        world = null;
    }

    public override void Lock() { /* no op */ }
    public override void Commit() { /* no op */ }
    
    public override void Warmup<T1>(int poolId) => archetypes[poolId] = [typeof(T1)];
    public override void Warmup<T1, T2>(int poolId) => archetypes[poolId] = [typeof(T1), typeof(T2)];
    public override void Warmup<T1, T2, T3>(int poolId) => archetypes[poolId] = [typeof(T1), typeof(T2), typeof(T3)];
    public override void Warmup<T1, T2, T3, T4>(int poolId) => archetypes[poolId] = [typeof(T1), typeof(T2), typeof(T3), typeof(T4)];

    public override int CreateEntity()
    {
        n++;
        var entity = world.Create();
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1>(in int poolId = -1)
    {
        n++;
        Entity entity;
        if (archetypes.TryGetValue(poolId, out var archetype)) entity = world.Create(archetype);
        else entity = world.Create<T1>();
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1, T2>(in int poolId = -1)
    {
        n++;
        Entity entity;
        if (archetypes.TryGetValue(poolId, out var archetype)) entity = world.Create(archetype);
        else entity = world.Create<T1, T2>();
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1, T2, T3>(in int poolId = -1)
    {
        n++;
        Entity entity;
        if (archetypes.TryGetValue(poolId, out var archetype)) entity = world.Create(archetype);
        else entity = world.Create<T1, T2, T3>();
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1, T2, T3, T4>(in int poolId = -1)
    {
        n++;
        Entity entity;
        if (archetypes.TryGetValue(poolId, out var archetype)) entity = world.Create(archetype);
        else entity = world.Create<T1, T2, T3, T4>();
        entities[n] = entity;
        return n;
    }

    public override void AddComponent<T1>(in int[] entityIds, in int poolId = -1)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = entities[entityIds[i]];
            world.Add<T1>(entity);
        }
    }

    public override void AddComponent<T1, T2>(in int[] entityIds, in int poolId = -1)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = entities[entityIds[i]];
            world.Add<T1, T2>(entity);
        }
    }

    public override void AddComponent<T1, T2, T3>(in int[] entityIds, in int poolId = -1)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = entities[entityIds[i]];
            world.Add<T1, T2, T3>(entity);
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in int[] entityIds, in int poolId = -1)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = entities[entityIds[i]];
            world.Add<T1, T2, T3, T4>(entity);
        }
    }

    public override void RemoveComponent<T1>(in int[] entityIds, in int poolId = -1)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = entities[entityIds[i]];
            world.Remove<T1>(entity);
        }
    }

    public override void RemoveComponent<T1, T2>(in int[] entityIds, in int poolId = -1)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = entities[entityIds[i]];
            world.Remove<T1, T2>(entity);
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in int[] entityIds, in int poolId = -1)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = entities[entityIds[i]];
            world.Remove<T1, T2, T3>(entity);
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in int[] entityIds, in int poolId = -1)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = entities[entityIds[i]];
            world.Remove<T1, T2, T3, T4>(entity);
        }
    }
}