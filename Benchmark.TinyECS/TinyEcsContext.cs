using Benchmark._Context;
using TinyEcs;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using MorpehComponent = Scellecs.Morpeh.IComponent;

namespace Benchmark.TinyECS;

public sealed class TinyEcsContext(int entityCount = 4096) : IBenchmarkContext
{
    private World? _world;
    private readonly Dictionary<int, Query>? _queries = new();

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int EntityCount { get; private set; }

    public void Setup()
    {
        _world = new World();
    }

    public void FinishSetup()
    {
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent
        => _queries![poolId] = _world!.QueryBuilder().With<T1>().Build();

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
        => _queries![poolId] = _world!.QueryBuilder().With<T1>().With<T2>().Build();

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent
        => _queries![poolId] = _world!.QueryBuilder().With<T1>().With<T2>().With<T3>().Build();

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent where T4 : struct, MorpehComponent, DragonComponent
        => _queries![poolId] = _world!.QueryBuilder().With<T1>().With<T2>().With<T3>().With<T4>().Build();

    public void Cleanup()
    {
        _queries!.Clear();
        
        _world!.Dispose();
        _world = null;
    }

    public void Lock() => _world!.BeginDeferred();

    public void Commit() => _world!.EndDeferred();

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Delete();

        EntityCount -= entities.Length;
    }

    public Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((EntityView[])entitySet);
        return entitySet;
    }

    public Array PrepareSet(in int count) => new EntityView[count];

    public void CreateEntities(in Array entitySet)
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Entity();

        EntityCount += entities.Length;
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default) where T1 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Entity().Set(c1);

        EntityCount += entities.Length;
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Entity().Set(c1).Set(c2);

        EntityCount += entities.Length;
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Entity().Set(c1).Set(c2).Set(c3);

        EntityCount += entities.Length;
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent where T4 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Entity().Set(c1).Set(c2).Set(c3).Set(c4);

        EntityCount += entities.Length;
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default) where T1 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1);
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2);
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2).Set(c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent where T4 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2).Set(c3).Set(c4);
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>();
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>().Unset<T2>();
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>().Unset<T2>().Unset<T3>();
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent where T4 : struct, MorpehComponent, DragonComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>().Unset<T2>().Unset<T3>().Unset<T4>();
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent
        => _world!.Query<T1>().Count();

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
        => _world!.Query<(T1, T2)>().Count();

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent
        => _world!.Query<(T1, T2, T3)>().Count();

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent where T4 : struct, MorpehComponent, DragonComponent
        => _world!.Query<(T1, T2, T3, T4)>().Count();

    public bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1) where T1 : struct, MorpehComponent, DragonComponent
    {
        if (entity == null) return false;

        var e = (EntityView)entity;
        c1 = e.Get<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
    {
        if (entity == null) return false;

        var e = (EntityView)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent
    {
        if (entity == null) return false;

        var e = (EntityView)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent where T4 : struct, MorpehComponent, DragonComponent
    {
        if (entity == null) return false;

        var e = (EntityView)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        c4 = e.Get<T4>();
        return true;
    }

    public void Tick(float delta)
    {
        //throw new NotImplementedException();
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId) where T1 : struct, MorpehComponent, DragonComponent
    {
        //throw new NotImplementedException();
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent
    {
        //throw new NotImplementedException();
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent
    {
        //throw new NotImplementedException();
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId) where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent where T3 : struct, MorpehComponent, DragonComponent where T4 : struct, MorpehComponent, DragonComponent
    {
        //throw new NotImplementedException();
    }
    
    public void Dispose()
    {
    }
}