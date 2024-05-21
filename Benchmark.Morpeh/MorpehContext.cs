using Benchmark._Context;
using Scellecs.Morpeh;

namespace Benchmark.Morpeh;

public class MorpehContext : BenchmarkContextBase
{
    private World? _world;
    private Entity[]? _entities;
    private Dictionary<int, Stash[]>? _stashes;
    private int _n;

    public override void Setup(int entityCount)
    {
        _world = World.Create();
        _entities = new Entity[entityCount];
        _stashes = new Dictionary<int, Stash[]>();
        _n = 0;
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
        _world.Commit();
    }

    public override void Warmup<T1>(int poolId)
    {
        _stashes![poolId] = [_world.GetStash<T1>()];
    }

    public override void Warmup<T1, T2>(int poolId)
    {
        _stashes![poolId] = [_world.GetStash<T1>(), _world.GetStash<T2>()];
    }

    public override void Warmup<T1, T2, T3>(int poolId)
    {
        _stashes![poolId] = [_world.GetStash<T1>(), _world.GetStash<T2>(), _world.GetStash<T3>()];
    }

    public override void Warmup<T1, T2, T3, T4>(int poolId)
    {
        _stashes![poolId] = [_world.GetStash<T1>(), _world.GetStash<T2>(), _world.GetStash<T3>(), _world.GetStash<T4>()];
    }

    public override void CreateEntities(in Span<int> entityIds)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = _world.CreateEntity();
            entityIds[i] = _n;
            _entities![_n] = entity;
            ++_n;
        }
    }


    public override void CreateEntities<T1>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world.CreateEntity();
                c1.Add(entity);
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world.CreateEntity();
                entity.AddComponent<T1>();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
    }

    public override void CreateEntities<T1, T2>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world.CreateEntity();
                c1.Add(entity);
                c2.Add(entity);
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world.CreateEntity();
                entity.AddComponent<T1>();
                entity.AddComponent<T2>();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
    }

    public override void CreateEntities<T1, T2, T3>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world.CreateEntity();
                c1.Add(entity);
                c2.Add(entity);
                c3.Add(entity);
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world.CreateEntity();
                entity.AddComponent<T1>();
                entity.AddComponent<T2>();
                entity.AddComponent<T3>();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            var c4 = (Stash<T4>)pool[3];
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world.CreateEntity();
                c1.Add(entity);
                c2.Add(entity);
                c3.Add(entity);
                c4.Add(entity);
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world.CreateEntity();
                entity.AddComponent<T1>();
                entity.AddComponent<T2>();
                entity.AddComponent<T3>();
                entity.AddComponent<T4>();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
    }

    public override void AddComponent<T1>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            for (var i = 0; i < entityIds.Length; i++)
                c1.Add(_entities![entityIds[i]]);
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
                _entities![entityIds[i]].AddComponent<T1>();
        }
    }

    public override void AddComponent<T1, T2>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                c1.Add(entity);
                c2.Add(entity);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                entity.AddComponent<T1>();
                entity.AddComponent<T2>();
            }
        }
    }

    public override void AddComponent<T1, T2, T3>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                c1.Add(entity);
                c2.Add(entity);
                c3.Add(entity);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                entity.AddComponent<T1>();
                entity.AddComponent<T2>();
                entity.AddComponent<T3>();
            }
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            var c4 = (Stash<T4>)pool[3];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
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
                var entity = _entities![entityIds[i]];
                entity.AddComponent<T1>();
                entity.AddComponent<T2>();
                entity.AddComponent<T3>();
                entity.AddComponent<T4>();
            }
        }
    }

    public override void RemoveComponent<T1>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            for (var i = 0; i < entityIds.Length; i++)
                c1.Remove(_entities![entityIds[i]]);
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
                _entities![entityIds[i]].AddComponent<T1>();
        }
    }

    public override void RemoveComponent<T1, T2>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                c1.Remove(entity);
                c2.Remove(entity);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                entity.RemoveComponent<T1>();
                entity.RemoveComponent<T2>();
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                c1.Remove(entity);
                c2.Remove(entity);
                c3.Remove(entity);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                entity.RemoveComponent<T1>();
                entity.RemoveComponent<T2>();
                entity.RemoveComponent<T3>();
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_stashes!.TryGetValue(poolId, out var pool))
        {
            var c1 = (Stash<T1>)pool[0];
            var c2 = (Stash<T2>)pool[1];
            var c3 = (Stash<T3>)pool[2];
            var c4 = (Stash<T4>)pool[3];
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
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
                var entity = _entities![entityIds[i]];
                entity.RemoveComponent<T1>();
                entity.RemoveComponent<T2>();
                entity.RemoveComponent<T3>();
                entity.RemoveComponent<T4>();
            }
        }
    }
}