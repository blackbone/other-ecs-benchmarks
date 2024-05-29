using Benchmark._Context;
using DCFApixels.DragonECS;
using fennecs;
using Scellecs.Morpeh;
using Entity = fennecs.Entity;
using World = fennecs.World;

namespace Benchmark.Fennecs;

public readonly struct FennecsContext(in int entityCount = 4096) : IBenchmarkContext
{
    private readonly ManagedRef<World.WorldLock>? _lock = new();
    private readonly Dictionary<int, Query>? _queries = new();
    private readonly List<ISystem>? _systems = new();
    private readonly World? _world = new(entityCount);

    public bool DeletesEntityOnLastComponentDeletion => false;

    public int EntityCount => _world!.Count;

    public void Setup()
    {
    }

    public void FinishSetup()
    {
    }

    public void Cleanup()
    {
        _systems!.Clear();
        _queries!.Clear();
        _world!.GC();
        _world.Dispose();
    }

    public void Dispose()
    {
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent
    {
        _queries![poolId] = _world!.Query<T1>().Compile().Warmup();
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
    {
        _queries![poolId] = _world!.Query<T1, T2>().Compile().Warmup();
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        _queries![poolId] = _world!.Query<T1, T2, T3>().Compile().Warmup();
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        _queries![poolId] = _world!.Query<T1, T2, T3, T4>().Compile().Warmup();
    }

    public void Lock()
    {
        _lock.V = _world!.Lock();
    }

    public void Commit()
    {
        _lock.V.Dispose();
    }

    public void CreateEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1);
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1).Add(c2);
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1).Add(c2).Add(c3);
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
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
            _world!.Despawn(entities[i].Id);
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1);
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1).Add(c2);
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1).Add(c2).Add(c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1).Add(c2).Add(c3).Add(c4);
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Remove<T1>();
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Remove<T1>().Remove<T2>();
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Remove<T1>().Remove<T2>().Remove<T3>();
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Remove<T1>().Remove<T2>().Remove<T3>().Remove<T4>();
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent
    {
        return _queries![poolId].Count;
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
    {
        return _queries![poolId].Count;
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        return _queries![poolId].Count;
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        return _queries![poolId].Count;
    }

    public bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Ref<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Ref<T1>();
        c2 = e.Ref<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.Ref<T1>();
        c2 = e.Ref<T2>();
        c3 = e.Ref<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
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
        where T1 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new System<T1>(method, (Query<T1>)_queries![poolId]));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new System<T1, T2>(method, (Query<T1, T2>)_queries![poolId]));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new System<T1, T2, T3>(method, (Query<T1, T2, T3>)_queries![poolId]));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent
        where T2 : struct, IComponent, IEcsComponent
        where T3 : struct, IComponent, IEcsComponent
        where T4 : struct, IComponent, IEcsComponent
    {
        _systems!.Add(new System<T1, T2, T3, T4>(method, (Query<T1, T2, T3, T4>)_queries![poolId]));
    }

    public Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public Array PrepareSet(in int count)
    {
        return new Entity[count];
    }
}