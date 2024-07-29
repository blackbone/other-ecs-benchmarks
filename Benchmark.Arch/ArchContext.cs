using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Extensions.Dangerous;
using Arch.Core.Utils;
using Benchmark._Context;
using Schedulers;
using Entity = Arch.Core.Entity;
using World = Arch.Core.World;

using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using XenoComponent = Xeno.IComponent;
using FrifloComponent = Friflo.Engine.ECS.IComponent;

// ReSharper disable ForCanBeConvertedToForeach

namespace Benchmark.Arch;

public sealed class ArchContext(int entityCount = 4096) : IBenchmarkContext
{
    private readonly Dictionary<int, ComponentType[]>? _archetypes = new();
    private readonly Dictionary<int, QueryDescription>? _queries = new();
    private readonly List<Action<World>>? _systems = new();
    private World? _world;

    public bool DeletesEntityOnLastComponentDeletion => false;

    public int EntityCount => _world!.Size;

    public void Setup()
    {
        var scheduler = new JobScheduler(new JobScheduler.Config
        {
            ThreadPrefixName = "Arch.Samples",
            ThreadCount = 0,
            MaxExpectedConcurrentJobs = 64,
            StrictAllocationMode = false,
        });
        _world = World.Create();
        World.SharedJobScheduler = scheduler;
        _world!.EnsureCapacity(entityCount);
    }

    public void FinishSetup()
    {
    }

    public void Cleanup()
    {
        World.SharedJobScheduler?.Dispose();
        World.SharedJobScheduler = null;
        
        _world!.Clear();
        _world!.Dispose();
        _world = null;
        _systems!.Clear();
        _archetypes!.Clear();
        _queries!.Clear();
    }

    public void Dispose()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void Lock()
    {
        /* no op */
    }

    public void Commit()
    {
        /* no op */
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _archetypes![poolId] = [typeof(T1)];
        _queries![poolId] = new QueryDescription().WithAll<T1>();
        _world!.Reserve(_archetypes[poolId], entityCount);
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2)];
        _queries![poolId] = new QueryDescription().WithAll<T1, T2>();
        _world!.Reserve(_archetypes[poolId], entityCount);
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2), typeof(T3)];
        _queries![poolId] = new QueryDescription().WithAll<T1, T2, T3>();
        _world!.Reserve(_archetypes[poolId], entityCount);
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        _archetypes![poolId] = [typeof(T1), typeof(T2), typeof(T3), typeof(T4)];
        _queries![poolId] = new QueryDescription().WithAll<T1, T2, T3, T4>();
        _world!.Reserve(_archetypes[poolId], entityCount);
    }

    public void CreateEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Create();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Create(c1);
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Create(c1, c2);
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Create(c1, c2, c3);
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Create(c1, c2, c3, c4);
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                _world!.Destroy(entities[i]);
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;

        for (var i = 0; i < entities.Length; i++)
            _world!.Add(entities[i], c1);
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.Add(entities[i], c1, c2);
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.Add(entities[i], c1, c2, c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.Add(entities[i], c1, c2, c3, c4);
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var archetype = _archetypes![poolId];
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                _world!.RemoveRange(entities[i], archetype);
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var archetype = _archetypes![poolId];
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                _world!.RemoveRange(entities[i], archetype);
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var archetype = _archetypes![poolId];
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                _world!.RemoveRange(entities[i], archetype);
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var entities = (Entity[])entitySet;
        var archetype = _archetypes![poolId];
        for (var i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive())
                _world!.RemoveRange(entities[i], archetype);
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _world!.CountEntities(_queries![poolId]);
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _world!.CountEntities(_queries![poolId]);
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _world!.CountEntities(_queries![poolId]);
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        return _world!.CountEntities(_queries![poolId]);
    }

    public bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = _world!.Get<T1>(e);
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        var c = e.Get<T1, T2>();
        c1 = c.t0;
        c2 = c.t1;
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        var c = e.Get<T1, T2, T3>();
        c1 = c.t0;
        c2 = c.t1;
        c3 = c.t2;
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        var c = e.Get<T1, T2, T3, T4>();
        c1 = c.t0;
        c2 = c.t1;
        c3 = c.t2;
        c4 = c.t3;
        return true;
    }

    public void Tick(float delta)
    {
        foreach (var system in _systems!)
            system(_world!);
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var system = new System<T1>(method);
        _systems!.Add(system.ForEachQuery);
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var system = new System<T1, T2>(method);
        _systems!.Add(system.ForEachQuery);
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var system = new System<T1, T2, T3>(method);
        _systems!.Add(system.ForEachQuery);
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
    {
        var system = new System<T1, T2, T3, T4>(method);
        _systems!.Add(system.ForEachQuery);
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