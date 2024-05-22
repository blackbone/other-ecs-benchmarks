using Benchmark._Context;
using Scellecs.Morpeh;
// ReSharper disable ForCanBeConvertedToForeach

namespace Benchmark.Morpeh;

public class MorpehContext : BenchmarkContextBase
{
    private World? _world;
    private Dictionary<int, Stash[]>? _stashes;

    public override void Setup(int entityCount)
    {
        _world = World.Create();
        _stashes = new Dictionary<int, Stash[]>();
    }

    public override void Cleanup()
    {
        _world?.Dispose();
        _world = null;
        
        _stashes!.Clear();
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
        _stashes![poolId] = [_world!.GetStash<T1>()];
    }

    public override void Warmup<T1, T2>(int poolId)
    {
        _stashes![poolId] = [_world!.GetStash<T1>(), _world!.GetStash<T2>()];
    }

    public override void Warmup<T1, T2, T3>(int poolId)
    {
        _stashes![poolId] = [_world!.GetStash<T1>(), _world!.GetStash<T2>(), _world!.GetStash<T3>()];
    }

    public override void Warmup<T1, T2, T3, T4>(int poolId)
    {
        _stashes![poolId] = [_world!.GetStash<T1>(), _world!.GetStash<T2>(), _world!.GetStash<T3>(), _world!.GetStash<T4>()];
    }

    public override void CreateEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity();
    }

    public override void CreateEntities<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.CreateEntity();
                c1.Add(entities[i]);
            }
        }
        else
        {
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.CreateEntity();
                entities[i] .AddComponent<T1>();
            }
        }
    }

    public override void CreateEntities<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.CreateEntity();
                c1.Add(entities[i]);
                c2.Add(entities[i]);
            }
        }
        else
        {
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.CreateEntity();
                entities[i].AddComponent<T1>();
                entities[i].AddComponent<T2>();
            }
        }
    }

    public override void CreateEntities<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.CreateEntity();
                c1.Add(entities[i]);
                c2.Add(entities[i]);
                c3.Add(entities[i]);
            }
        }
        else
        {
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.CreateEntity();
                entities[i].AddComponent<T1>();
                entities[i].AddComponent<T2>();
                entities[i].AddComponent<T3>();
            }
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            var c4 = (Stash<T4>)pool[3];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.CreateEntity();
                c1.Add(entities[i]);
                c2.Add(entities[i]);
                c3.Add(entities[i]);
                c4.Add(entities[i]);
            }
        }
        else
        {
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = _world!.CreateEntity();
                entities[i].AddComponent<T1>();
                entities[i].AddComponent<T2>();
                entities[i].AddComponent<T3>();
                entities[i].AddComponent<T4>();
            }
        }
    }

    public override void DeleteEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.RemoveEntity(entities[i]);
    }

    public override void AddComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            for (var i = 0; i < entities.Length; i++)
                c1.Add(entities[i]);
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
                entities[i].AddComponent<T1>();
        }
    }

    public override void AddComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Add(entities[i]);
                c2.Add(entities[i]);
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].AddComponent<T1>();
                entities[i].AddComponent<T2>();
            }
        }
    }

    public override void AddComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Add(entities[i]);
                c2.Add(entities[i]);
                c3.Add(entities[i]);
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].AddComponent<T1>();
                entities[i].AddComponent<T2>();
                entities[i].AddComponent<T3>();
            }
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            var c4 = (Stash<T4>)pool[3];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Add(entities[i]);
                c2.Add(entities[i]);
                c3.Add(entities[i]);
                c4.Add(entities[i]);
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].AddComponent<T1>();
                entities[i].AddComponent<T2>();
                entities[i].AddComponent<T3>();
                entities[i].AddComponent<T4>();
            }
        }
    }

    public override void RemoveComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            for (var i = 0; i < entities.Length; i++)
                c1.Remove(entities[i]);
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
                entities[i].AddComponent<T1>();
        }
    }

    public override void RemoveComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Remove(entities[i]);
                c2.Remove(entities[i]);
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].RemoveComponent<T1>();
                entities[i].RemoveComponent<T2>();
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Remove(entities[i]);
                c2.Remove(entities[i]);
                c3.Remove(entities[i]);
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].RemoveComponent<T1>();
                entities[i].RemoveComponent<T2>();
                entities[i].RemoveComponent<T3>();
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            var c4 = (Stash<T4>)pool[3];
            for (var i = 0; i < entities.Length; i++)
            {
                c1.Remove(entities[i]);
                c2.Remove(entities[i]);
                c3.Remove(entities[i]);
                c4.Remove(entities[i]);
            }
        }
        else
        {
            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].RemoveComponent<T1>();
                entities[i].RemoveComponent<T2>();
                entities[i].RemoveComponent<T3>();
                entities[i].RemoveComponent<T4>();
            }
        }
    }

    public override object Shuffle(in object entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public override object PrepareSet(in int count) => new Entity[count];
}