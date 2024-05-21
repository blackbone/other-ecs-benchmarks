using Benchmark._Context;
using fennecs;

namespace Benchmark.Fennecs;

public class FennecsContext : BenchmarkContextBase
{
    private World? _world;
    private World.WorldLock _lock;
    private Entity[]? _entities;
    private int _n;
    private Dictionary<int, Query>? _queries;
    private Query<Identity> _all;

    public override void Setup(int entityCount)
    {
        _world = new World(entityCount);
        _entities = new Entity[entityCount];
        _queries = new Dictionary<int, Query>();
        _n = 0;

        _all = _world.Query().Build().Warmup();
    }

    public override void Warmup<T1>(int poolId) => _world!.Query<T1>().Build().Warmup();

    public override void Warmup<T1, T2>(int poolId) => _world!.Query<T1, T2>().Build().Warmup();

    public override void Warmup<T1, T2, T3>(int poolId) => _world!.Query<T1, T2, T3>().Build().Warmup();

    public override void Warmup<T1, T2, T3, T4>(int poolId) => _world!.Query<T1, T2, T3, T4>().Build().Warmup();

    public override void Cleanup() => _world!.GC();

    public override void Lock() => _lock = _world!.Lock;

    public override void Commit() => _lock.Dispose();

    public override void CreateEntities(in Span<int> entityIds)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = _world!.Spawn();
            entityIds[i] = _n;
            _entities![_n] = entity;
            ++_n;
        }
    }

    public override void CreateEntities<T1>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out _))
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world!.Spawn();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
            
            _all.Add<T1>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world!.Spawn();
                entity.Add<T1>();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }   
        }
    }

    public override void CreateEntities<T1, T2>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out _))
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world!.Spawn();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
            
            _all.Add<T1>();
            _all.Add<T2>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world!.Spawn();
                entity.Add<T1>();
                entity.Add<T2>();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
    }

    public override void CreateEntities<T1, T2, T3>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out _))
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world!.Spawn();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
            
            _all.Add<T1>();
            _all.Add<T2>();
            _all.Add<T3>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world!.Spawn();
                entity.Add<T1>();
                entity.Add<T2>();
                entity.Add<T3>();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out _))
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world!.Spawn();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
            
            _all.Add<T1>();
            _all.Add<T2>();
            _all.Add<T3>();
            _all.Add<T4>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                var entity = _world!.Spawn();
                entity.Add<T1>();
                entity.Add<T2>();
                entity.Add<T3>();
                entity.Add<T4>();
                entityIds[i] = _n;
                _entities![_n] = entity;
                ++_n;
            }
        }
    }

    public override void AddComponent<T1>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out var query))
        {
            query.Add<T1>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![entityIds[i]].Add<T1>();
            }
        }
    }

    public override void AddComponent<T1, T2>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out var query))
        {
            query.Add<T1>();
            query.Add<T2>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![entityIds[i]].Add<T1>().Add<T2>();
            }
        }
    }

    public override void AddComponent<T1, T2, T3>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out var query))
        {
            query.Add<T1>();
            query.Add<T2>();
            query.Add<T3>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![entityIds[i]].Add<T1>().Add<T2>().Add<T3>();
            }
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out var query))
        {
            query.Add<T1>();
            query.Add<T2>();
            query.Add<T3>();
            query.Add<T4>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![entityIds[i]].Add<T1>().Add<T2>().Add<T3>().Add<T4>();
            }
        }
    }

    public override void RemoveComponent<T1>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out var query))
        {
            query.Remove<T1>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![entityIds[i]].Remove<T1>();
            }
        }
    }

    public override void RemoveComponent<T1, T2>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out var query))
        {
            query.Remove<T1>();
            query.Remove<T2>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![entityIds[i]].Remove<T1>().Remove<T2>();
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out var query))
        {
            query.Remove<T1>();
            query.Remove<T2>();
            query.Remove<T3>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![entityIds[i]].Remove<T1>().Remove<T2>().Remove<T3>();
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_queries!.TryGetValue(poolId, out var query))
        {
            query.Remove<T1>();
            query.Remove<T2>();
            query.Remove<T3>();
            query.Remove<T4>();
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![entityIds[i]].Remove<T1>().Remove<T2>().Remove<T3>().Remove<T4>();
            }
        }
    }
}