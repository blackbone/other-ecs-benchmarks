using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Benchmark._Context;
using DCFApixels.DragonECS;
using Flecs.NET.Bindings;
using Flecs.NET.Core;
using Scellecs.Morpeh;
using Entity = Flecs.NET.Core.Entity;

namespace Benchmark.FlecsNET;

#pragma warning disable CS9113 // Parameter is unread.
public sealed class FlecsNETContext : IBenchmarkContext<Entity>
#pragma warning restore CS9113 // Parameter is unread.
{
    private readonly Dictionary<int, Query> _queries = new();
    private Flecs.NET.Core.World _world;

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int NumberOfLivingEntities { get; private set; }

    public void Setup() {
        _world = Flecs.NET.Core.World.Create();
        _world.Import<Ecs.Stats>();
        _world.Set<flecs.EcsRest>(default);
    }

    public void FinishSetup()
    {
        _world.InitBuiltinComponents();
    }

    public void Cleanup()
    {
        _world.Quit();
    }

    public void Dispose()
    {
        _world.Dispose();
        _world = default;
    }

    public void Lock()
    {
        _world.DeferBegin();
    }

    public void Commit()
    {
        _world.DeferEnd();
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _queries![poolId] = _world.QueryBuilder().With<T1>().Build();
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _queries![poolId] = _world.QueryBuilder().With<T1>().With<T2>().Build();
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _queries![poolId] = _world.QueryBuilder().With<T1>().With<T2>().With<T3>().Build();
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _queries![poolId] = _world.QueryBuilder().With<T1>().With<T2>().With<T3>().With<T4>().Build();
    }

    public void DeleteEntities(in Flecs.NET.Core.Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                entities[i].Destruct();

        NumberOfLivingEntities -= entities.Length;
    }

    public Flecs.NET.Core.Entity[] PrepareSet(in int count)
    {
        return count > 0 ? new Flecs.NET.Core.Entity[count] : [];
    }

    public void CreateEntities(in Flecs.NET.Core.Entity[] entities)
    {
        using var create1_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity();

        NumberOfLivingEntities += entities.Length;
    }

    public void CreateEntities<T1>(in Flecs.NET.Core.Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1);

        NumberOfLivingEntities += entities.Length;
    }

    public void CreateEntities<T1, T2>(in Flecs.NET.Core.Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var create2_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2);

        NumberOfLivingEntities += entities.Length;
    }

    public void CreateEntities<T1, T2, T3>(in Flecs.NET.Core.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2,
        in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var create3_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2).Set(c3);

        NumberOfLivingEntities += entities.Length;
    }

    public void CreateEntities<T1, T2, T3, T4>(in Flecs.NET.Core.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2,
        in T3 c3,
        in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var create4_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2).Set(c3).Set(c4);

        NumberOfLivingEntities += entities.Length;
    }

    public void AddComponent<T1>(in Flecs.NET.Core.Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var add1_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1);
    }

    public void AddComponent<T1, T2>(in Flecs.NET.Core.Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var add2_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2);
    }

    public void AddComponent<T1, T2, T3>(in Flecs.NET.Core.Entity[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var add3_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2).Set(c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in Flecs.NET.Core.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2,
        in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var add4_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2).Set(c3).Set(c4);
    }

    public void RemoveComponent<T1>(in Flecs.NET.Core.Entity[] entities, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var remove1_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive() && entities[i].Has<T1>())
                entities[i].Remove<T1>();
    }

    public void RemoveComponent<T1, T2>(in Flecs.NET.Core.Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var remove2_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
        {
            if (!entities[i].IsAlive()) return;
            if (entities[i].Has<T1>()) entities[i].Remove<T1>();
            if (entities[i].Has<T2>()) entities[i].Remove<T2>();
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Flecs.NET.Core.Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var remove3_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
        {
            if (!entities[i].IsAlive()) return;
            if (entities[i].Has<T1>()) entities[i].Remove<T1>();
            if (entities[i].Has<T2>()) entities[i].Remove<T2>();
            if (entities[i].Has<T3>()) entities[i].Remove<T3>();
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Flecs.NET.Core.Entity[] entities, in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        using var remove4_defer = new Defer(_world);

        for (var i = 0; i < entities.Length; i++)
        {
            if (!entities[i].IsAlive()) return;
            if (entities[i].Has<T1>()) entities[i].Remove<T1>();
            if (entities[i].Has<T2>()) entities[i].Remove<T2>();
            if (entities[i].Has<T3>()) entities[i].Remove<T3>();
            if (entities[i].Has<T4>()) entities[i].Remove<T4>();
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var n = 0;
        _queries![poolId].Each(_ => n++);
        return n;
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var n = 0;
        _queries![poolId].Each(_ => n++);
        return n;
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var n = 0;
        _queries![poolId].Each(_ => n++);
        return n;
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var n = 0;
        _queries![poolId].Each(_ => n++);
        return n;
    }

    public bool GetSingle<T1>(in Flecs.NET.Core.Entity e, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.GetMut<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in Flecs.NET.Core.Entity e, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.GetMut<T1>();
        c2 = e.GetMut<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in Flecs.NET.Core.Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.GetMut<T1>();
        c2 = e.GetMut<T2>();
        c3 = e.GetMut<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in Flecs.NET.Core.Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        c1 = e.GetMut<T1>();
        c2 = e.GetMut<T2>();
        c3 = e.GetMut<T3>();
        c4 = e.GetMut<T4>();
        return true;
    }

    public void Tick(float delta) => _world.Progress();

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _world.System<T1>()
            .MultiThreaded()
            .Write<T1>()
            .Each(method);
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _world.System<T1, T2>()
            .MultiThreaded()
            .Write<T1>()
            .Write<T2>()
            .Each(method);
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _world.System<T1, T2, T3>()
            .MultiThreaded()
            .Write<T1>()
            .Write<T2>()
            .Write<T3>()
            .Each(method);
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _world.System<T1, T2, T3, T4>()
            .MultiThreaded()
            .Write<T1>()
            .Write<T2>()
            .Write<T3>()
            .Write<T4>()
            .Each(method);
    }

    private readonly struct Defer : IDisposable {
        private readonly Flecs.NET.Core.World _world;

        public Defer(Flecs.NET.Core.World world) {
            _world = world;
            _world.DeferBegin();
        }

        // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
        public void Dispose() => _world.DeferEnd();
    }
}
