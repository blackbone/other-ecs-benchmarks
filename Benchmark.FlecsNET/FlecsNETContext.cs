﻿using Benchmark._Context;
using DCFApixels.DragonECS;
using Flecs.NET.Core;
using Scellecs.Morpeh;
using Entity = Flecs.NET.Core.Entity;
using World = Flecs.NET.Core.World;

namespace Benchmark.FlecsNET;

public sealed class FlecsNETContext(int entityCount = 4096) : IBenchmarkContext
{
    private readonly Dictionary<int, Query>? _queries = new();
    private readonly List<Action<float>>? _systems = new();
    private World _world;

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int EntityCount { get; private set; }

    public void Setup()
    {
        _world = World.Create();
    }

    public void FinishSetup()
    {
    }

    public void Cleanup()
    {
        _world.Quit();
        _world.Dispose();
        _world = default;
    }

    public void Dispose()
    {
    }

    public void Lock() => _world.DeferBegin();

    public void Commit() => _world.DeferEnd();

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent
    {
        _queries![poolId] = _world.QueryBuilder().With<T1>().Build();
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        _queries![poolId] = _world.QueryBuilder().With<T1>().With<T2>().Build();
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        _queries![poolId] = _world.QueryBuilder().With<T1>().With<T2>().With<T3>().Build();
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        _queries![poolId] = _world.QueryBuilder().With<T1>().With<T2>().With<T3>().With<T4>().Build();
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            if (entities[i].IsAlive()) entities[i].Destruct();

        EntityCount -= entities.Length;
    }

    public Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }

    public Array PrepareSet(in int count) => count > 0 ? new Entity[count] : [];

    public void CreateEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity();
        
        EntityCount += entities.Length;
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default) where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1);
        
        EntityCount += entities.Length;
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2);
        
        EntityCount += entities.Length;
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2).Set(c3);
        
        EntityCount += entities.Length;
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i] = _world.Entity().Set(c1).Set(c2).Set(c3).Set(c4);
        
        EntityCount += entities.Length;
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default) where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i].Set(c1);
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2);
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2).Set(c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
            entities[i].Set(c1).Set(c2).Set(c3).Set(c4);
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].IsAlive() && entities[i].Has<T1>()) entities[i].Remove<T1>();
        }
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].IsAlive() && entities[i].Has<T1>()) entities[i].Remove<T1>();
            if (entities[i].IsAlive() && entities[i].Has<T2>()) entities[i].Remove<T2>();
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].IsAlive() && entities[i].Has<T1>()) entities[i].Remove<T1>();
            if (entities[i].IsAlive() && entities[i].Has<T2>()) entities[i].Remove<T2>();
            if (entities[i].IsAlive() && entities[i].Has<T3>()) entities[i].Remove<T3>();
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        var entities = (Entity[])entitySet;
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].IsAlive() && entities[i].Has<T1>()) entities[i].Remove<T1>();
            if (entities[i].IsAlive() && entities[i].Has<T2>()) entities[i].Remove<T2>();
            if (entities[i].IsAlive() && entities[i].Has<T3>()) entities[i].Remove<T3>();
            if (entities[i].IsAlive() && entities[i].Has<T4>()) entities[i].Remove<T4>();
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent
    {
        var n = 0;
        _queries![poolId].Each(_ => n++);
        return n;
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        var n = 0;
        _queries![poolId].Each(_ => n++);
        return n;
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        var n = 0;
        _queries![poolId].Each(_ => n++);
        return n;
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        var n = 0;
        _queries![poolId].Each(_ => n++);
        return n;
    }

    public bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;
        var e = (Entity)entity;
        c1 = e.GetMut<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;
        var e = (Entity)entity;

        c1 = e.GetMut<T1>();
        c2 = e.GetMut<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;
        var e = (Entity)entity;

        c1 = e.GetMut<T1>();
        c2 = e.GetMut<T2>();
        c3 = e.GetMut<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
    {
        if (entity == null) return false;
        var e = (Entity)entity;

        c1 = e.GetMut<T1>();
        c2 = e.GetMut<T2>();
        c3 = e.GetMut<T3>();
        c4 = e.GetMut<T4>();
        return true;
    }

    public void Tick(float delta)
    {
        foreach (var system in _systems!)
            system(delta);
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent
        => _systems!.Add(_ => _queries![poolId].Iter((Iter iter, Column<T1> c1) =>
        {
            foreach (var i in iter)
                method(ref c1[i]);
        }));

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent
        => _systems!.Add(_ => _queries![poolId].Iter((Iter iter, Column<T1> c1, Column<T2> c2) =>
        {
            foreach (var i in iter)
                method(ref c1[i], ref c2[i]);
        }));

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent
        => _systems!.Add(_ => _queries![poolId].Iter((Iter iter, Column<T1 >c1, Column<T2> c2, Column<T3> c3) =>
        {
            foreach (var i in iter)
                method(ref c1[i], ref c2[i], ref c3[i]);
        }));

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent where T2 : struct, IComponent, IEcsComponent where T3 : struct, IComponent, IEcsComponent where T4 : struct, IComponent, IEcsComponent
        => _systems!.Add(_ => _queries![poolId].Iter((Iter iter, Column<T1 >c1, Column<T2> c2, Column<T3> c3, Column<T4> c4) =>
        {
            foreach (var i in iter)
                method(ref c1[i], ref c2[i], ref c3[i], ref c4[i]);
        }));
}