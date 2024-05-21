using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using Benchmark._Context;

namespace Benchmark.Arch;

public class ArchContext : BenchmarkContextBase
{
    private int _entityCount;
    private World? _world;
    private Entity[]? _entities;
    private Dictionary<int, ComponentType[]>? _archetypes;
    private int _n;

    public override void Setup(int entityCount)
    {
        _entityCount = entityCount;
        _world = World.Create();
        _entities = new Entity[entityCount];
        _archetypes = new Dictionary<int, ComponentType[]>();
        _n = 0;
    }

    public override void Cleanup()
    {
        _world?.Clear();
        _world?.Dispose();
        _world = null;
    }

    public override void Lock()
    {
        /* no op */
    }

    public override void Commit()
    {
        /* no op */
    }

    public override void Warmup<T1>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1)];
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void Warmup<T1, T2>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2)];
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void Warmup<T1, T2, T3>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2), typeof(T3)];
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void Warmup<T1, T2, T3, T4>(int poolId)
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2), typeof(T3), typeof(T4)];
        _world!.Reserve(_archetypes[poolId], _entityCount);
    }

    public override void CreateEntities(in Span<int> entityIds)
    {
        for (int i = 0; i < entityIds.Length; i++)
        {
            var entity = _world!.Create();
            entityIds[i] = _n;
            _entities![_n] = entity;
            ++_n;
        }
    }

    public override void CreateEntities<T1>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![_n] = _world!.Create(archetype);
                entityIds[i] = _n;
                ++_n;
            }
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![_n] = _world!.Create<T1>();
                entityIds[i] = _n;
                ++_n;
            }
        }
    }

    public override void CreateEntities<T1, T2>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![_n] = _world!.Create(archetype);
                entityIds[i] = _n;
                ++_n;
            }
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![_n] = _world!.Create<T1, T2>();
                entityIds[i] = _n;
                ++_n;
            }
        }
    }

    public override void CreateEntities<T1, T2, T3>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![_n] = _world!.Create(archetype);
                entityIds[i] = _n;
                ++_n;
            }
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![_n] = _world!.Create<T1, T2, T3>();
                entityIds[i] = _n;
                ++_n;
            }
        }
    }

    public override void CreateEntities<T1, T2, T3, T4>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![_n] = _world!.Create(archetype);
                entityIds[i] = _n;
                ++_n;
            }
        }
        else
        {
            for (int i = 0; i < entityIds.Length; i++)
            {
                _entities![_n] = _world!.Create<T1, T2, T3, T4>();
                entityIds[i] = _n;
                ++_n;
            }
        }
    }

    public override void AddComponent<T1>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.AddRange(entity, archetype);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.Add<T1>(entity);
            }
        }
    }

    public override void AddComponent<T1, T2>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.AddRange(entity, archetype);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.Add<T1, T2>(entity);
            }
        }
    }

    public override void AddComponent<T1, T2, T3>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.AddRange(entity, archetype);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.Add<T1, T2, T3>(entity);
            }
        }
    }

    public override void AddComponent<T1, T2, T3, T4>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.AddRange(entity, archetype);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.Add<T1, T2, T3, T4>(entity);
            }
        }
    }

    public override void RemoveComponent<T1>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.RemoveRange(entity, archetype);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.Remove<T1>(entity);
            }
        }
    }

    public override void RemoveComponent<T1, T2>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.RemoveRange(entity, archetype);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.Remove<T1, T2>(entity);
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.RemoveRange(entity, archetype);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.Remove<T1, T2, T3>(entity);
            }
        }
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in Span<int> entityIds, in int poolId = -1)
    {
        if (_archetypes!.TryGetValue(poolId, out var archetype))
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.RemoveRange(entity, archetype);
            }
        }
        else
        {
            for (var i = 0; i < entityIds.Length; i++)
            {
                var entity = _entities![entityIds[i]];
                _world!.Remove<T1, T2, T3, T4>(entity);
            }
        }
    }
}