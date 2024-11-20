using Benchmark._Context;
using TinyEcs;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using MorpehComponent = Scellecs.Morpeh.IComponent;
using XenoComponent = Xeno.IComponent;
using FrifloComponent = Friflo.Engine.ECS.IComponent;
// ReSharper disable PossibleNullReferenceException

namespace Benchmark.TinyECS;

public sealed class TinyEcsContext : IBenchmarkContext {
    private World _world;
    private Scheduler _scheduler;
    private readonly Dictionary<int, Query> _queries = new();

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int EntityCount { get; private set; }

    public void Setup() {
        _world = new World();
        _scheduler = new Scheduler(_world);
    }

    public void FinishSetup() {}

    public void Warmup<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        => _queries![poolId] = _world.QueryBuilder().With<T1>().Build();

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        => _queries![poolId] = _world.QueryBuilder().With<T1>().With<T2>().Build();

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        => _queries![poolId] = _world.QueryBuilder().With<T1>().With<T2>().With<T3>().Build();

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        => _queries![poolId] = _world.QueryBuilder().With<T1>().With<T2>().With<T3>().With<T4>().Build();

    public void Cleanup() {
        _queries.Clear();

    }

    public void Dispose()
    {
        _scheduler = null;

        _world.Dispose();
        _world = null;
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            if (entities[i] != default)
                entities[i].Delete();
        
        EntityCount -= entitySet.Length;
        _world.BeginDeferred();
    }

    public Array PrepareSet(in int count) => new EntityView[count];

    public void CreateEntities(in Array entitySet)
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity();

        EntityCount += entitySet.Length;
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1);
        
        EntityCount += entitySet.Length;
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2);
        
        EntityCount += entitySet.Length;
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2).Set(c3);
        
        EntityCount += entitySet.Length;
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2).Set(c3).Set(c4);
        
        EntityCount += entitySet.Length;
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1);
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2);
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2).Set(c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2).Set(c3).Set(c4);
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>();
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>().Unset<T2>();
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>().Unset<T2>().Unset<T3>();
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (EntityView[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>().Unset<T2>().Unset<T3>().Unset<T4>();
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        => _world.Query<T1>().Count();

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        => _world.Query<(T1, T2)>().Count();

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        => _world.Query<(T1, T2, T3)>().Count();

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        => _world.Query<(T1, T2, T3, T4)>().Count();

    public bool GetSingle<T1>(in object entity, in int poolId, ref T1 c1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var e = (EntityView)entity;
        c1 = e.Get<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in object entity, in int poolId, ref T1 c1, ref T2 c2) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var e = (EntityView)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var e = (EntityView)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var e = (EntityView)entity;
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        c4 = e.Get<T4>();
        return true;
    }

    public void Tick(float delta) => _scheduler.Run();

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent =>
        _scheduler.AddSystem((Query<T1> query) => query.EachJob((ref T1 c1) => method(ref c1)));

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent =>
        _scheduler.AddSystem((Query<(T1, T2)> query) => query.EachJob((ref T1 c1, ref T2 c2) => method(ref c1, ref c2)));

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent =>
        _scheduler.AddSystem((Query<(T1, T2, T3)> query) => query.EachJob((ref T1 c1, ref T2 c2, ref T3 c3) => method(ref c1, ref c2, ref c3)));

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent =>
        _scheduler.AddSystem((Query<(T1, T2, T3, T4)> query) => query.EachJob((ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) => method(ref c1, ref c2, ref c3, ref c4)));

    private readonly struct Defer : IDisposable {
        private readonly World _world;
        public Defer(World world) {
            _world = world;
            _world.BeginDeferred();
        }

        public void Dispose() => _world.EndDeferred();
    }
}
