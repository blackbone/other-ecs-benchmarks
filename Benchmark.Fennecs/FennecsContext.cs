using Benchmark._Context;
using fennecs;
using Entity = fennecs.Entity;
using World = fennecs.World;
using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;

namespace Benchmark.Fennecs;

public sealed class FennecsContext(int entityCount = 4096) : IBenchmarkContext
{
    private readonly Dictionary<int, Query>? _streams = new();
    private readonly List<ISystem>? _systems = new();
    private World.WorldLock? _lock;
    private World? _world = new();

    public bool DeletesEntityOnLastComponentDeletion => false;

    public int EntityCount => _world!.Count;

    public void Setup()
    {
        _world = new World(entityCount);
    }

    public void FinishSetup()
    {
    }

    public void Cleanup()
    {
        _systems!.Clear();
        _streams!.Clear();
        _world!.GC();
        _world!.Dispose();
        _world = null;
    }

    public void Dispose()
    {
    }

    public void Warmup<T1>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        => _streams![poolId] = _world!.Query<T1>().Compile().Warmup();

    public void Warmup<T1, T2>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        => _streams![poolId] = _world!.Query<T1, T2>().Compile().Warmup();

    public void Warmup<T1, T2, T3>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        => _streams![poolId] = _world!.Query<T1, T2, T3>().Compile().Warmup();

    public void Warmup<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent
        => _streams![poolId] = _world!.Query<T1, T2, T3, T4>().Compile().Warmup();

    public void Lock() => _lock = _world!.Lock();

    public void Commit() => _lock!.Value.Dispose();

    public void CreateEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1);
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1).Add(c2);
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1).Add(c2).Add(c3);
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1).Add(c2).Add(c3).Add(c4);
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        // TODO perform use overload which utilizes ReadOnlySpan<Identity>
        for (var i = 0; i < entities.Length; i++)
            if (_world!.Contains(entities[i]))
                _world!.Despawn(entities[i]);
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1);
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1).Add(c2);
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1).Add(c2).Add(c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1).Add(c2).Add(c3).Add(c4);
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].Has<T1>())
                entities[i].Remove<T1>();
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            if (entities[i].Has<T1>()) entities[i].Remove<T1>();
            if (entities[i].Has<T2>()) entities[i].Remove<T2>();
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            if (entities[i].Has<T1>()) entities[i].Remove<T1>();
            if (entities[i].Has<T2>()) entities[i].Remove<T2>();
            if (entities[i].Has<T3>()) entities[i].Remove<T3>();
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            if (entities[i].Has<T1>()) entities[i].Remove<T1>();
            if (entities[i].Has<T2>()) entities[i].Remove<T2>();
            if (entities[i].Has<T3>()) entities[i].Remove<T3>();
            if (entities[i].Has<T4>()) entities[i].Remove<T4>();
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent
    {
        return _streams![poolId].Count;
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
    {
        return _streams![poolId].Count;
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
    {
        return _streams![poolId].Count;
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent
    {
        return _streams![poolId].Count;
    }

    public bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1) where T1 : struct, MorpehComponent, DragonComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Ref<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Ref<T1>();
        c2 = e.Ref<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Ref<T1>();
        c2 = e.Ref<T2>();
        c3 = e.Ref<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Ref<T1>();
        c2 = e.Ref<T2>();
        c3 = e.Ref<T3>();
        c4 = e.Ref<T4>();
        return true;
    }

    public void Tick(float delta)
    {
        for (var i = 0; i < _systems!.Count; i++)
            _systems[i].Run(delta);
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
    {
        _systems!.Add(new System<T1>(method, _streams![poolId].Stream<T1>()));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
    {
        _systems!.Add(new System<T1, T2>(method, _streams![poolId].Stream<T1, T2>()));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
    {
        _systems!.Add(new System<T1, T2, T3>(method, _streams![poolId].Stream<T1, T2, T3>()));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent
    {
        _systems!.Add(new System<T1, T2, T3, T4>(method, _streams![poolId].Stream<T1, T2, T3, T4>()));
    }

    public Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public Array PrepareSet(in int count)
    {
        return count > 0 ? new Entity[count] : [];
    }
}