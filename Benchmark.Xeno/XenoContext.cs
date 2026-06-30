using System;
using System.Runtime.CompilerServices;
using Benchmark;
using Benchmark.Context;
using DCFApixels.DragonECS;
using Xeno;

namespace Benchmark.Xeno;

public enum WorldKind
{
    Entity,
    System1,
    System2,
    System3,
    MultiSystems,
    FilterMismatch,
}

public sealed class XenoContext : IBenchmarkContext<Entity>
{
    private WorldKind _worldKind;
    private World _world;

    private bool _hasSystem1C1;
    private bool _hasSystem1C2;
    private bool _hasSystem2C1C2;
    private bool _hasSystem3C1C2C3;
    private bool _isFilterMismatch;
    private int _systemCount;

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int NumberOfLivingEntities => _world == null ? 0 : (int)_world.EntityCount;

    public void Setup()
    {
        _worldKind = WorldKind.Entity;
        _world = null;
        _hasSystem1C1 = false;
        _hasSystem1C2 = false;
        _hasSystem2C1C2 = false;
        _hasSystem3C1C2C3 = false;
        _isFilterMismatch = false;
        _systemCount = 0;
    }

    public void FinishSetup()
    {
        _world = CreateWorld();
        _world.Start();
    }

    public void Cleanup()
    {
        _world?.Stop();
    }

    public void Dispose()
    {
        _world?.Dispose();
        _world = null;
    }

    public void Warmup<T1>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    { }

    public void Warmup<T1, T2>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    { }

    public void Warmup<T1, T2, T3>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    { }

    public void Warmup<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    { }

    public void DeleteEntities(in Entity[] entitySet)
    {
        _world.DestroyEntities_NoLock(entitySet);
    }
    public Entity[] PrepareSet(in int count) => new Entity[count];

    public void CreateEntities(in Entity[] entitySet)
    {
        for (var i = 0; i < entitySet.Length; i++)
            entitySet[i] = _world.CreateEntity_NoLock();
    }

    public void CreateEntities<T1>(in Entity[] entitySet, in int poolId, in T1 c1)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            entitySet[i] = CreateEntitySingle(poolId, c1);
    }

    public void CreateEntities<T1, T2>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            entitySet[i] = CreateEntityPair(poolId, c1, c2);
    }

    public void CreateEntities<T1, T2, T3>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            entitySet[i] = CreateEntityTriple(poolId, c1, c2, c3);
    }

    public void CreateEntities<T1, T2, T3, T4>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3, in T4 c4)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            entitySet[i] = CreateEntityQuad(c1, c2, c3, c4);
    }

    public void AddComponent<T1>(in Entity[] entitySet, in int poolId, in T1 c1)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            AddSingle(entitySet[i], poolId, c1);
    }

    public void AddComponent<T1, T2>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            AddPair(entitySet[i], poolId, c1, c2);
    }

    public void AddComponent<T1, T2, T3>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            AddTriple(entitySet[i], poolId, c1, c2, c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3, in T4 c4)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            AddQuad(entitySet[i], c1, c2, c3, c4);
    }

    public void RemoveComponent<T1>(in Entity[] entitySet, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            RemoveSingle(entitySet[i], poolId);
    }

    public void RemoveComponent<T1, T2>(in Entity[] entitySet, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            RemovePair(entitySet[i], poolId);
    }

    public void RemoveComponent<T1, T2, T3>(in Entity[] entitySet, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            RemoveTriple(entitySet[i], poolId);
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Entity[] entitySet, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entitySet.Length; i++)
            RemoveQuad(entitySet[i]);
    }

    public int CountWith<T1>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => throw new NotSupportedException();

    public int CountWith<T1, T2>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => throw new NotSupportedException();

    public int CountWith<T1, T2, T3>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => throw new NotSupportedException();

    public int CountWith<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => throw new NotSupportedException();

    public bool GetSingle<T1>(in Entity entity, in int poolId, ref T1 c1)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => throw new NotSupportedException();

    public bool GetSingle<T1, T2>(in Entity entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => throw new NotSupportedException();

    public bool GetSingle<T1, T2, T3>(in Entity entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => throw new NotSupportedException();

    public bool GetSingle<T1, T2, T3, T4>(in Entity entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => throw new NotSupportedException();

    public void Tick(float delta) => _world.Tick(delta);

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        XenoSystemHolder1<T1>.Method = method;
        _systemCount++;
        if (typeof(T1) == typeof(Component1))
            _hasSystem1C1 = true;
        else if (typeof(T1) == typeof(Component2))
            _hasSystem1C2 = true;
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        XenoSystemHolder2<T1, T2>.Method = method;
        _systemCount++;

        if (typeof(T1) == typeof(Component1) && typeof(T2) == typeof(Component2)) {
            _hasSystem2C1C2 = true;
            if (poolId != 0 && poolId != 2)
                _isFilterMismatch = true;
        }
        else {
            _isFilterMismatch = true;
        }
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        XenoSystemHolder3<T1, T2, T3>.Method = method;
        _systemCount++;
        if (typeof(T1) == typeof(Component1) && typeof(T2) == typeof(Component2) && typeof(T3) == typeof(Component3))
            _hasSystem3C1C2C3 = true;
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => throw new NotSupportedException();

    private World CreateWorld()
    {
        _worldKind = DetectWorldKind();
        var name = $"xeno_{DateTimeOffset.UtcNow.Ticks}";
        return _worldKind switch {
            WorldKind.System1 => new XenoSystem1World(name),
            WorldKind.System2 => new XenoSystem2World(name),
            WorldKind.System3 => new XenoSystem3World(name),
            WorldKind.MultiSystems => new XenoMultiSystemsWorld(name),
            WorldKind.FilterMismatch => new XenoFilterMismatchWorld(name),
            _ => new XenoEntityWorld(name),
        };
    }

    private WorldKind DetectWorldKind()
    {
        if (_isFilterMismatch)
            return WorldKind.FilterMismatch;
        if (_hasSystem1C1 && _hasSystem1C2 && _hasSystem2C1C2 && _systemCount == 3)
            return WorldKind.MultiSystems;
        if (_hasSystem3C1C2C3 && _systemCount == 1)
            return WorldKind.System3;
        if (_hasSystem2C1C2 && _systemCount == 1)
            return WorldKind.System2;
        if (_hasSystem1C1 && _systemCount == 1)
            return WorldKind.System1;
        return WorldKind.Entity;
    }

    private Entity CreateEntitySingle<T1>(int poolId, T1 c1) where T1 : struct
    {
        if (_worldKind == WorldKind.FilterMismatch)
            return CreateFilterEntity(poolId, c1);

        return _worldKind switch {
            WorldKind.System1 => poolId switch {
                0 => ((XenoSystem1World)_world).CreateEntity_NoLock(Cast<T1, Component1>(in c1)),
                1 => ((XenoSystem1World)_world).CreateEntity_NoLock(Cast<T1, Padding1>(in c1)),
                _ => throw new NotSupportedException(),
            },
            WorldKind.System2 => poolId switch {
                1 => ((XenoSystem2World)_world).CreateEntity_NoLock(Cast<T1, Component1>(in c1)),
                2 => ((XenoSystem2World)_world).CreateEntity_NoLock(Cast<T1, Component2>(in c1)),
                _ => throw new NotSupportedException(),
            },
            WorldKind.System3 => poolId switch {
                1 => ((XenoSystem3World)_world).CreateEntity_NoLock(Cast<T1, Component1>(in c1)),
                2 => ((XenoSystem3World)_world).CreateEntity_NoLock(Cast<T1, Component2>(in c1)),
                3 => ((XenoSystem3World)_world).CreateEntity_NoLock(Cast<T1, Component3>(in c1)),
                _ => throw new NotSupportedException(),
            },
            WorldKind.MultiSystems => poolId switch {
                0 => ((XenoMultiSystemsWorld)_world).CreateEntity_NoLock(Cast<T1, Component1>(in c1)),
                1 => ((XenoMultiSystemsWorld)_world).CreateEntity_NoLock(Cast<T1, Component2>(in c1)),
                _ => throw new NotSupportedException(),
            },
            _ => poolId switch {
                0 => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component1>(in c1)),
                1 => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component2>(in c1)),
                2 => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component3>(in c1)),
                3 => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component4>(in c1)),
                _ => throw new NotSupportedException(),
            },
        };
    }

    private Entity CreateFilterEntity<T1>(int poolId, T1 c1) where T1 : struct
    {
        var world = (XenoFilterMismatchWorld)_world;
        return poolId switch {
            200 => world.CreateEntity_NoLock(Cast<T1, Padding1>(in c1)),
            100 => world.CreateEntity_NoLock(Cast<T1, Component1>(in c1)),
            101 => world.CreateEntity_NoLock(Cast<T1, Component2>(in c1)),
            102 => world.CreateEntity_NoLock(Cast<T1, Component3>(in c1)),
            103 => world.CreateEntity_NoLock(Cast<T1, Component4>(in c1)),
            104 => world.CreateEntity_NoLock(Cast<T1, Component5>(in c1)),
            105 => world.CreateEntity_NoLock(Cast<T1, Component6>(in c1)),
            106 => world.CreateEntity_NoLock(Cast<T1, Component7>(in c1)),
            107 => world.CreateEntity_NoLock(Cast<T1, Component8>(in c1)),
            108 => world.CreateEntity_NoLock(Cast<T1, Component9>(in c1)),
            109 => world.CreateEntity_NoLock(Cast<T1, Component10>(in c1)),
            110 => world.CreateEntity_NoLock(Cast<T1, Component11>(in c1)),
            111 => world.CreateEntity_NoLock(Cast<T1, Component12>(in c1)),
            112 => world.CreateEntity_NoLock(Cast<T1, Component13>(in c1)),
            113 => world.CreateEntity_NoLock(Cast<T1, Component14>(in c1)),
            114 => world.CreateEntity_NoLock(Cast<T1, Component15>(in c1)),
            115 => world.CreateEntity_NoLock(Cast<T1, Component16>(in c1)),
            116 => world.CreateEntity_NoLock(Cast<T1, Component17>(in c1)),
            117 => world.CreateEntity_NoLock(Cast<T1, Component18>(in c1)),
            118 => world.CreateEntity_NoLock(Cast<T1, Component19>(in c1)),
            119 => world.CreateEntity_NoLock(Cast<T1, Component20>(in c1)),
            120 => world.CreateEntity_NoLock(Cast<T1, Component21>(in c1)),
            121 => world.CreateEntity_NoLock(Cast<T1, Component22>(in c1)),
            122 => world.CreateEntity_NoLock(Cast<T1, Component23>(in c1)),
            123 => world.CreateEntity_NoLock(Cast<T1, Component24>(in c1)),
            124 => world.CreateEntity_NoLock(Cast<T1, Component25>(in c1)),
            125 => world.CreateEntity_NoLock(Cast<T1, Component26>(in c1)),
            126 => world.CreateEntity_NoLock(Cast<T1, Component27>(in c1)),
            127 => world.CreateEntity_NoLock(Cast<T1, Component28>(in c1)),
            128 => world.CreateEntity_NoLock(Cast<T1, Component29>(in c1)),
            129 => world.CreateEntity_NoLock(Cast<T1, Component30>(in c1)),
            130 => world.CreateEntity_NoLock(Cast<T1, Component31>(in c1)),
            131 => world.CreateEntity_NoLock(Cast<T1, Component32>(in c1)),
            132 => world.CreateEntity_NoLock(Cast<T1, Component33>(in c1)),
            133 => world.CreateEntity_NoLock(Cast<T1, Component34>(in c1)),
            134 => world.CreateEntity_NoLock(Cast<T1, Component35>(in c1)),
            135 => world.CreateEntity_NoLock(Cast<T1, Component36>(in c1)),
            136 => world.CreateEntity_NoLock(Cast<T1, Component37>(in c1)),
            137 => world.CreateEntity_NoLock(Cast<T1, Component38>(in c1)),
            138 => world.CreateEntity_NoLock(Cast<T1, Component39>(in c1)),
            139 => world.CreateEntity_NoLock(Cast<T1, Component40>(in c1)),
            140 => world.CreateEntity_NoLock(Cast<T1, Component41>(in c1)),
            141 => world.CreateEntity_NoLock(Cast<T1, Component42>(in c1)),
            142 => world.CreateEntity_NoLock(Cast<T1, Component43>(in c1)),
            143 => world.CreateEntity_NoLock(Cast<T1, Component44>(in c1)),
            144 => world.CreateEntity_NoLock(Cast<T1, Component45>(in c1)),
            145 => world.CreateEntity_NoLock(Cast<T1, Component46>(in c1)),
            146 => world.CreateEntity_NoLock(Cast<T1, Component47>(in c1)),
            147 => world.CreateEntity_NoLock(Cast<T1, Component48>(in c1)),
            148 => world.CreateEntity_NoLock(Cast<T1, Component49>(in c1)),
            149 => world.CreateEntity_NoLock(Cast<T1, Component50>(in c1)),
            150 => world.CreateEntity_NoLock(Cast<T1, Component51>(in c1)),
            151 => world.CreateEntity_NoLock(Cast<T1, Component52>(in c1)),
            152 => world.CreateEntity_NoLock(Cast<T1, Component53>(in c1)),
            153 => world.CreateEntity_NoLock(Cast<T1, Component54>(in c1)),
            154 => world.CreateEntity_NoLock(Cast<T1, Component55>(in c1)),
            155 => world.CreateEntity_NoLock(Cast<T1, Component56>(in c1)),
            156 => world.CreateEntity_NoLock(Cast<T1, Component57>(in c1)),
            157 => world.CreateEntity_NoLock(Cast<T1, Component58>(in c1)),
            158 => world.CreateEntity_NoLock(Cast<T1, Component59>(in c1)),
            159 => world.CreateEntity_NoLock(Cast<T1, Component60>(in c1)),
            160 => world.CreateEntity_NoLock(Cast<T1, Component61>(in c1)),
            161 => world.CreateEntity_NoLock(Cast<T1, Component62>(in c1)),
            162 => world.CreateEntity_NoLock(Cast<T1, Component63>(in c1)),
            163 => world.CreateEntity_NoLock(Cast<T1, Component64>(in c1)),
            164 => world.CreateEntity_NoLock(Cast<T1, Component65>(in c1)),
            165 => world.CreateEntity_NoLock(Cast<T1, Component66>(in c1)),
            166 => world.CreateEntity_NoLock(Cast<T1, Component67>(in c1)),
            167 => world.CreateEntity_NoLock(Cast<T1, Component68>(in c1)),
            168 => world.CreateEntity_NoLock(Cast<T1, Component69>(in c1)),
            169 => world.CreateEntity_NoLock(Cast<T1, Component70>(in c1)),
            170 => world.CreateEntity_NoLock(Cast<T1, Component71>(in c1)),
            171 => world.CreateEntity_NoLock(Cast<T1, Component72>(in c1)),
            172 => world.CreateEntity_NoLock(Cast<T1, Component73>(in c1)),
            173 => world.CreateEntity_NoLock(Cast<T1, Component74>(in c1)),
            174 => world.CreateEntity_NoLock(Cast<T1, Component75>(in c1)),
            175 => world.CreateEntity_NoLock(Cast<T1, Component76>(in c1)),
            176 => world.CreateEntity_NoLock(Cast<T1, Component77>(in c1)),
            177 => world.CreateEntity_NoLock(Cast<T1, Component78>(in c1)),
            178 => world.CreateEntity_NoLock(Cast<T1, Component79>(in c1)),
            179 => world.CreateEntity_NoLock(Cast<T1, Component80>(in c1)),
            180 => world.CreateEntity_NoLock(Cast<T1, Component81>(in c1)),
            181 => world.CreateEntity_NoLock(Cast<T1, Component82>(in c1)),
            182 => world.CreateEntity_NoLock(Cast<T1, Component83>(in c1)),
            183 => world.CreateEntity_NoLock(Cast<T1, Component84>(in c1)),
            184 => world.CreateEntity_NoLock(Cast<T1, Component85>(in c1)),
            185 => world.CreateEntity_NoLock(Cast<T1, Component86>(in c1)),
            186 => world.CreateEntity_NoLock(Cast<T1, Component87>(in c1)),
            187 => world.CreateEntity_NoLock(Cast<T1, Component88>(in c1)),
            188 => world.CreateEntity_NoLock(Cast<T1, Component89>(in c1)),
            189 => world.CreateEntity_NoLock(Cast<T1, Component90>(in c1)),
            190 => world.CreateEntity_NoLock(Cast<T1, Component91>(in c1)),
            191 => world.CreateEntity_NoLock(Cast<T1, Component92>(in c1)),
            192 => world.CreateEntity_NoLock(Cast<T1, Component93>(in c1)),
            193 => world.CreateEntity_NoLock(Cast<T1, Component94>(in c1)),
            194 => world.CreateEntity_NoLock(Cast<T1, Component95>(in c1)),
            195 => world.CreateEntity_NoLock(Cast<T1, Component96>(in c1)),
            196 => world.CreateEntity_NoLock(Cast<T1, Component97>(in c1)),
            197 => world.CreateEntity_NoLock(Cast<T1, Component98>(in c1)),
            198 => world.CreateEntity_NoLock(Cast<T1, Component99>(in c1)),
            199 => world.CreateEntity_NoLock(Cast<T1, Component100>(in c1)),
            _ => throw new NotSupportedException(),
        };
    }

    private Entity CreateEntityPair<T1, T2>(int poolId, T1 c1, T2 c2)
        where T1 : struct where T2 : struct
    {
        return (_worldKind, poolId) switch {
            (WorldKind.System2, _) => ((XenoSystem2World)_world).CreateEntity_NoLock(Cast<T1, Component1>(in c1), Cast<T2, Component2>(in c2)),
            (WorldKind.MultiSystems, _) => ((XenoMultiSystemsWorld)_world).CreateEntity_NoLock(Cast<T1, Component1>(in c1), Cast<T2, Component2>(in c2)),
            (_, 0) => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component1>(in c1), Cast<T2, Component2>(in c2)),
            (_, 1) => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component2>(in c1), Cast<T2, Component3>(in c2)),
            (_, 2) => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component3>(in c1), Cast<T2, Component4>(in c2)),
            (_, 3) => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component4>(in c1), Cast<T2, Component1>(in c2)),
            _ => throw new NotSupportedException(),
        };
    }

    private Entity CreateEntityTriple<T1, T2, T3>(int poolId, T1 c1, T2 c2, T3 c3)
        where T1 : struct where T2 : struct where T3 : struct
    {
        return (_worldKind, poolId) switch {
            (WorldKind.System3, _) => ((XenoSystem3World)_world).CreateEntity_NoLock(Cast<T1, Component1>(in c1), Cast<T2, Component2>(in c2), Cast<T3, Component3>(in c3)),
            (_, 0) => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component1>(in c1), Cast<T2, Component2>(in c2), Cast<T3, Component3>(in c3)),
            (_, 1) => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component2>(in c1), Cast<T2, Component3>(in c2), Cast<T3, Component4>(in c3)),
            (_, 2) => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component3>(in c1), Cast<T2, Component4>(in c2), Cast<T3, Component1>(in c3)),
            (_, 3) => ((XenoEntityWorld)_world).CreateEntity_NoLock(Cast<T1, Component4>(in c1), Cast<T2, Component1>(in c2), Cast<T3, Component2>(in c3)),
            _ => throw new NotSupportedException(),
        };
    }

    private Entity CreateEntityQuad<T1, T2, T3, T4>(T1 c1, T2 c2, T3 c3, T4 c4)
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct
    {
        return ((XenoEntityWorld)_world).CreateEntity_NoLock(
            Cast<T1, Component1>(in c1),
            Cast<T2, Component2>(in c2),
            Cast<T3, Component3>(in c3),
            Cast<T4, Component4>(in c4));
    }

    private void AddSingle<T1>(Entity entity, int poolId, T1 c1) where T1 : struct
    {
        var world = (XenoEntityWorld)_world;
        switch (poolId) {
            case 0:
                world.Add_NoLock(entity, Cast<T1, Component1>(in c1));
                return;
            case 1:
                world.Add_NoLock(entity, Cast<T1, Component2>(in c1));
                return;
            case 2:
                world.Add_NoLock(entity, Cast<T1, Component3>(in c1));
                return;
            case 3:
                world.Add_NoLock(entity, Cast<T1, Component4>(in c1));
                return;
            default:
                throw new NotSupportedException();
        }
    }

    private void AddPair<T1, T2>(Entity entity, int poolId, T1 c1, T2 c2)
        where T1 : struct where T2 : struct
    {
        var world = (XenoEntityWorld)_world;
        switch (poolId) {
            case 0:
                world.Add_NoLock(entity, Cast<T1, Component1>(in c1), Cast<T2, Component2>(in c2));
                return;
            case 1:
                world.Add_NoLock(entity, Cast<T1, Component2>(in c1), Cast<T2, Component3>(in c2));
                return;
            case 2:
                world.Add_NoLock(entity, Cast<T1, Component3>(in c1), Cast<T2, Component4>(in c2));
                return;
            case 3:
                world.Add_NoLock(entity, Cast<T1, Component4>(in c1), Cast<T2, Component1>(in c2));
                return;
            default:
                throw new NotSupportedException();
        }
    }

    private void AddTriple<T1, T2, T3>(Entity entity, int poolId, T1 c1, T2 c2, T3 c3)
        where T1 : struct where T2 : struct where T3 : struct
    {
        var world = (XenoEntityWorld)_world;
        switch (poolId) {
            case 0:
                world.Add_NoLock(entity, Cast<T1, Component1>(in c1), Cast<T2, Component2>(in c2), Cast<T3, Component3>(in c3));
                return;
            case 1:
                world.Add_NoLock(entity, Cast<T1, Component2>(in c1), Cast<T2, Component3>(in c2), Cast<T3, Component4>(in c3));
                return;
            case 2:
                world.Add_NoLock(entity, Cast<T1, Component3>(in c1), Cast<T2, Component4>(in c2), Cast<T3, Component1>(in c3));
                return;
            case 3:
                world.Add_NoLock(entity, Cast<T1, Component4>(in c1), Cast<T2, Component1>(in c2), Cast<T3, Component2>(in c3));
                return;
            default:
                throw new NotSupportedException();
        }
    }

    private void AddQuad<T1, T2, T3, T4>(Entity entity, T1 c1, T2 c2, T3 c3, T4 c4)
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct
    {
        ((XenoEntityWorld)_world).Add_NoLock(
            entity,
            Cast<T1, Component1>(in c1),
            Cast<T2, Component2>(in c2),
            Cast<T3, Component3>(in c3),
            Cast<T4, Component4>(in c4));
    }

    private void RemoveSingle(Entity entity, int poolId)
    {
        var world = (XenoEntityWorld)_world;
        switch (poolId) {
            case 0: world.RemoveComponent1_NoLock(entity); return;
            case 1: world.RemoveComponent2_NoLock(entity); return;
            case 2: world.RemoveComponent3_NoLock(entity); return;
            case 3: world.RemoveComponent4_NoLock(entity); return;
            default: throw new NotSupportedException();
        }
    }

    private void RemovePair(Entity entity, int poolId)
    {
        var world = (XenoEntityWorld)_world;
        switch (poolId) {
            case 0:
                world.RemoveComponent1AndComponent2_NoLock(entity);
                return;
            case 1:
                world.RemoveComponent2AndComponent3_NoLock(entity);
                return;
            case 2:
                world.RemoveComponent3AndComponent4_NoLock(entity);
                return;
            case 3:
                world.RemoveComponent4AndComponent1_NoLock(entity);
                return;
            default:
                throw new NotSupportedException();
        }
    }

    private void RemoveTriple(Entity entity, int poolId)
    {
        var world = (XenoEntityWorld)_world;
        switch (poolId) {
            case 0:
                world.RemoveComponent1AndComponent2AndComponent3_NoLock(entity);
                return;
            case 1:
                world.RemoveComponent2AndComponent3AndComponent4_NoLock(entity);
                return;
            case 2:
                world.RemoveComponent3AndComponent4AndComponent1_NoLock(entity);
                return;
            case 3:
                world.RemoveComponent4AndComponent1AndComponent2_NoLock(entity);
                return;
            default:
                throw new NotSupportedException();
        }
    }

    private void RemoveQuad(Entity entity)
    {
        ((XenoEntityWorld)_world).RemoveComponent1AndComponent2AndComponent3AndComponent4_NoLock(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TTo Cast<TFrom, TTo>(in TFrom value)
        where TFrom : struct
        where TTo : struct
    {
        var source = Unsafe.AsRef(in value);
        return Unsafe.As<TFrom, TTo>(ref source);
    }
}
