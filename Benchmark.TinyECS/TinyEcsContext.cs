using System;
using Benchmark.Context;
using DCFApixels.DragonECS;
using TinyEcs;
using IComponent = Scellecs.Morpeh.IComponent;

namespace Benchmark.TinyECS;

public sealed class TinyEcsContext : IBenchmarkContext<EntityView>
{
    private TinyEcs.World _world;
    private Scheduler _scheduler;

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int NumberOfLivingEntities { get; private set; }

    public void Setup() {
        _world = new TinyEcs.World(8192ul);
        _scheduler = new Scheduler(_world);
    }

    public void FinishSetup() {
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        _world.Entity().Set<T1>(default(T1)).Delete();
    }
    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        _world.Entity().Set<T1>(default(T1)).Set<T2>(default(T2)).Delete();
    }
    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        _world.Entity().Set<T1>(default(T1)).Set<T2>(default(T2)).Set<T3>(default(T3)).Delete();
    }
    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        _world.Entity().Set<T1>(default(T1)).Set<T2>(default(T2)).Set<T3>(default(T3)).Set<T4>(default(T4)).Delete();
    }
    public void Cleanup() {}
    public void Dispose()
    {
        _scheduler = null;

        _world.Dispose();
        _world = null;
    }

    public void DeleteEntities(in EntityView[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            if (entities[i] != default)
                entities[i].Delete();

        NumberOfLivingEntities -= entities.Length;
    }

    public EntityView[] PrepareSet(in int count) => new EntityView[count];

    public void CreateEntities(in EntityView[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity();

        NumberOfLivingEntities += entities.Length;
    }

    public void CreateEntities<T1>(in EntityView[] entities, in int poolId, in T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1);

        NumberOfLivingEntities += entities.Length;
    }

    public void CreateEntities<T1, T2>(in EntityView[] entities, in int poolId, in T1 c1, in T2 c2) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2);

        NumberOfLivingEntities += entities.Length;
    }

    public void CreateEntities<T1, T2, T3>(in EntityView[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2).Set(c3);

        NumberOfLivingEntities += entities.Length;
    }

    public void CreateEntities<T1, T2, T3, T4>(in EntityView[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2).Set(c3).Set(c4);

        NumberOfLivingEntities += entities.Length;
    }

    public void AddComponent<T1>(in EntityView[] entities, in int poolId, in T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1);
    }

    public void AddComponent<T1, T2>(in EntityView[] entities, in int poolId, in T1 c1, in T2 c2) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2);
    }

    public void AddComponent<T1, T2, T3>(in EntityView[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2).Set(c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in EntityView[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2).Set(c3).Set(c4);
    }

    public void RemoveComponent<T1>(in EntityView[] entities, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>();
    }

    public void RemoveComponent<T1, T2>(in EntityView[] entities, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>().Unset<T2>();
    }

    public void RemoveComponent<T1, T2, T3>(in EntityView[] entities, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>().Unset<T2>().Unset<T3>();
    }

    public void RemoveComponent<T1, T2, T3, T4>(in EntityView[] entities, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Unset<T1>().Unset<T2>().Unset<T3>().Unset<T4>();
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _world.QueryBuilder().With<T1>().Build().Count();

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _world.QueryBuilder().With<T1>().With<T2>().Build().Count();

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _world.QueryBuilder().With<T1>().With<T2>().With<T3>().Build().Count();

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _world.QueryBuilder().With<T1>().With<T2>().With<T3>().With<T4>().Build().Count();

    public bool GetSingle<T1>(in EntityView e, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.Get<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in EntityView e, in int poolId, ref T1 c1, ref T2 c2) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in EntityView e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in EntityView e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.Get<T1>();
        c2 = e.Get<T2>();
        c3 = e.Get<T3>();
        c4 = e.Get<T4>();
        return true;
    }

    public void Tick(float delta) => _scheduler.Run();

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        var sysDelegate = Sys;
        _scheduler.AddSystem(sysDelegate);

        void Sys(Query<Data<T1>> query) {
            foreach (var (_, c)  in query)
                method(ref c.Ref);
        }
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var sysDelegate = Sys;
        _scheduler.AddSystem(sysDelegate);

        void Sys(Query<Data<T1, T2>> query) {
            foreach (var (c1, c2) in query)
                method(ref c1.Ref, ref c2.Ref);
        }
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var sysDelegate = Sys;
        _scheduler.AddSystem(sysDelegate);

        void Sys(Query<Data<T1, T2, T3>> query) {
            foreach (var (c1, c2, c3) in query)
                method(ref c1.Ref, ref c2.Ref, ref c3.Ref);
        }
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var sysDelegate = Sys;
        _scheduler.AddSystem(sysDelegate);

        void Sys(Query<Data<T1, T2, T3, T4>> query) {
            foreach (var (c1, c2, c3, c4) in query)
                method(ref c1.Ref, ref c2.Ref, ref c3.Ref, ref c4.Ref);
        }
    }
}
