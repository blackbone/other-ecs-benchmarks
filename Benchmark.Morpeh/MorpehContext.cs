using Benchmark._Context;
using Scellecs.Morpeh;

namespace Benchmark.Morpeh;

public class MorpehContext : BenchmarkContextBase
{
    private World world;
    private Entity[] entities;
    private Dictionary<int, Stash[]> stashes;
    private int n = -1;

    public override void Setup(int entityCount)
    {
        world = World.Create();
        entities = new Entity[entityCount];
        stashes = new Dictionary<int, Stash[]>();
        n = -1;
    }

    public override void Cleanup()
    {
        world?.Dispose();
        world = null;
    }

    public override void Lock() { /* no op */ }
    public override void Commit() => world.Commit();
    
    public override void Warmup<T1>(int poolId) => stashes[poolId] = [world.GetStash<T1>()];
    public override void Warmup<T1, T2>(int poolId) => stashes[poolId] = [world.GetStash<T1>(), world.GetStash<T2>()];
    public override void Warmup<T1, T2, T3>(int poolId) => stashes[poolId] = [world.GetStash<T1>(), world.GetStash<T2>(), world.GetStash<T3>()];
    public override void Warmup<T1, T2, T3, T4>(int poolId) => stashes[poolId] = [world.GetStash<T1>(), world.GetStash<T2>(), world.GetStash<T3>(), world.GetStash<T4>()];

    public override int CreateEntity()
    {
        n++;
        var entity = world.CreateEntity();
        entities[n] = entity;
        return n;
    }


    public override int CreateEntity<T1>(in int poolId = -1)
    {
        n++;
        var entity = world.CreateEntity();
        if (stashes.TryGetValue(poolId, out var pool))
        {
            ((Stash<T1>)pool[0]).Add(entity);
        }
        else
        {
            entity.AddComponent<T1>();
        }
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1, T2>(in int poolId = -1)
    {
        n++;
        var entity = world.CreateEntity();
        if (stashes.TryGetValue(poolId, out var pool))
        {
            ((Stash<T1>)pool[0]).Add(entity);
            ((Stash<T2>)pool[1]).Add(entity);
        }
        else
        {
            entity.AddComponent<T1>();
            entity.AddComponent<T2>();
        }
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1, T2, T3>(in int poolId = -1)
    {
        n++;
        var entity = world.CreateEntity();
        if (stashes.TryGetValue(poolId, out var pool))
        {
            ((Stash<T1>)pool[0]).Add(entity);
            ((Stash<T2>)pool[1]).Add(entity);
            ((Stash<T3>)pool[2]).Add(entity);
        }
        else
        {
            entity.AddComponent<T1>();
            entity.AddComponent<T2>();
            entity.AddComponent<T3>();
        }
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1, T2, T3, T4>(in int poolId = -1)
    {
        n++;
        var entity = world.CreateEntity();
        if (stashes.TryGetValue(poolId, out var pool))
        {
            ((Stash<T1>)pool[0]).Add(entity);
            ((Stash<T2>)pool[1]).Add(entity);
            ((Stash<T3>)pool[2]).Add(entity);
            ((Stash<T4>)pool[3]).Add(entity);
        }
        else
        {
            entity.AddComponent<T1>();
            entity.AddComponent<T2>();
            entity.AddComponent<T3>();
            entity.AddComponent<T4>();
        }
        entities[n] = entity;
        return n;
    }

    public override void AddComponent<T1>(in int[] entityIds, in int poolId = -1)
    {
        if (stashes.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            for (var i = 0; i < entityIds.Length; i++)
                c1.Add(entities[entityIds[i]]);
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
                entities[entityIds[i]].AddComponent<T1>();
        }
    }

    public override void AddComponent<T1, T2>(in int[] entityIds, in int poolId = -1)
    {
        if (stashes.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                c1.Add(entity);
                c2.Add(entity);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                entity.AddComponent<T1>();
                entity.AddComponent<T2>();
            }
        }
    }

    public override void AddComponent<T1, T2, T3>(in int[] entityIds, in int poolId = -1)
    {
        if (stashes.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                c1.Add(entity);
                c2.Add(entity);
                c3.Add(entity);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                entity.AddComponent<T1>();
                entity.AddComponent<T2>();
                entity.AddComponent<T3>();
            }
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in int[] entityIds, in int poolId = -1)
    {
        if (stashes.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            var c4 = (Stash<T4>)pool[3];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                c1.Add(entity);
                c2.Add(entity);
                c3.Add(entity);
                c4.Add(entity);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                entity.AddComponent<T1>();
                entity.AddComponent<T2>();
                entity.AddComponent<T3>();
                entity.AddComponent<T4>();
            }
        }
    }

    public override void RemoveComponent<T1>(in int[] entityIds, in int poolId = -1)
    {
        if (stashes.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            for (var i = 0; i < entityIds.Length; i++)
                c1.Remove(entities[entityIds[i]]);
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
                entities[entityIds[i]].AddComponent<T1>();
        }
    }

    public override void RemoveComponent<T1, T2>(in int[] entityIds, in int poolId = -1)
    {
        if (stashes.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                c1.Remove(entity);
                c2.Remove(entity);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                entity.RemoveComponent<T1>();
                entity.RemoveComponent<T2>();
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in int[] entityIds, in int poolId = -1)
    {
        if (stashes.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                c1.Remove(entity);
                c2.Remove(entity);
                c3.Remove(entity);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                entity.RemoveComponent<T1>();
                entity.RemoveComponent<T2>();
                entity.RemoveComponent<T3>();
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in int[] entityIds, in int poolId = -1)
    {
        if (stashes.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            var c4 = (Stash<T4>)pool[3];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                c1.Remove(entity);
                c2.Remove(entity);
                c3.Remove(entity);
                c4.Remove(entity);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = entities[entityIds[i]];
                entity.RemoveComponent<T1>();
                entity.RemoveComponent<T2>();
                entity.RemoveComponent<T3>();
                entity.RemoveComponent<T4>();
            }
        }
    }
}