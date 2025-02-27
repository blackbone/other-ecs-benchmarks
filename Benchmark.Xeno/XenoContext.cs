using System;
using Benchmark._Context;
using DCFApixels.DragonECS;
using Xeno;

namespace Benchmark.Xeno;

public class XenoContext : IBenchmarkContext<Entity>
{
    private World _world;

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int NumberOfLivingEntities => (int)_world.EntityCount;

    public void Setup()
    {
        _world = Worlds.Create($"xeno_world_{DateTimeOffset.UtcNow.Ticks}");
        _world.EnsureCapacity(NumberOfLivingEntities);
    }

    public void Dispose()
    {
        _world?.Dispose();
        _world = null;
    }

    public void FinishSetup()
    {
        _world.Start();
    }

    public void Warmup<T1>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    { }

    public void Warmup<T1, T2>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    { }

    public void Warmup<T1, T2, T3>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    { }

    public void Warmup<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    { }

    public void Cleanup()
    {
        _world.Stop();
    }

    public void DeleteEntities(in Entity[] entities) {
        for (var i = 0; i < entities.Length; i++) {
            _world.DestroyEntity(entities[i]);
        }
    }

    public Entity[] PrepareSet(in int count) => new Entity[count];

    public void CreateEntities(in Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.CreateEntity();
    }

    public void CreateEntities<T1>(in Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++) {
            entities[i] = _world.CreateEntity(c1);
        }
    }

    public void CreateEntities<T1, T2>(in Entity[] entities, in int poolId, in T1 c1,
        in T2 c2) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.CreateEntity(c1, c2);
    }

    public void CreateEntities<T1, T2, T3>(in Entity[] entities, in int poolId, in T1 c1,
        in T2 c2,
        in T3 c3) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.CreateEntity(c1, c2, c3);
    }

    public void CreateEntities<T1, T2, T3, T4>(in Entity[] entities, in int poolId, in T1 c1,
        in T2 c2,
        in T3 c3, in T4 c4) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world.CreateEntity(c1, c2, c3, c4);
    }

    public void AddComponent<T1>(in Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponents(c1);
    }

    public void AddComponent<T1, T2>(in Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponents(c1, c2);
    }

    public void AddComponent<T1, T2, T3>(in Entity[] entities, in int poolId, in T1 c1,
        in T2 c2,
        in T3 c3) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponents(c1, c2, c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in Entity[] entities, in int poolId, in T1 c1,
        in T2 c2,
        in T3 c3, in T4 c4) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponents(c1, c2, c3, c4);
    }

    public void RemoveComponent<T1>(in Entity[] entities, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].RemoveComponents<T1>();
    }

    public void RemoveComponent<T1, T2>(in Entity[] entities, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].RemoveComponents<T1, T2>();
    }

    public void RemoveComponent<T1, T2, T3>(in Entity[] entities, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].RemoveComponents<T1, T2, T3>();
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Entity[] entities, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].RemoveComponents<T1, T2, T3, T4>();
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.Count<T1>();
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.Count<T1, T2>();
    }

    public int CountWith<T1, T2, T3>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.Count<T1, T2, T3>();
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.Count<T1, T2, T3, T4>();
    }

    public bool GetSingle<T1>(in Entity e, in int poolId, ref T1 c1)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var le = e;
        return le.RefComponents(ref c1);
    }

    public bool GetSingle<T1, T2>(in Entity e, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var le = e;
        le.RefComponents(ref c1);
        le.RefComponents(ref c2);
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        var le = e;
        le.RefComponents(ref c1);
        le.RefComponents(ref c2);
        le.RefComponents(ref c3);
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3,
        ref T4 c4) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        var le = e;
        le.RefComponents(ref c1);
        le.RefComponents(ref c2);
        le.RefComponents(ref c3);
        le.RefComponents(ref c4);
        return true;
    }

    public void Tick(float delta)
    {
        _world.Tick(delta);
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _world.AddSystem(new System1<T1>(method));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _world.AddSystem(new System2<T1, T2>(method));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _world.AddSystem(new System3<T1, T2, T3>(method));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _world.AddSystem(new System4<T1, T2, T3, T4>(method));
    }
}

[System]
public unsafe partial class System1<C1> where C1 : struct, IComponent {
    private readonly delegate*<ref C1, void> _method;
    public System1(delegate*<ref C1, void> method) : this() {
        _method = method;
    }
    [SystemMethod(SystemMethodType.Update)]
    public void Update(ref C1 c1) => _method(ref c1);
}

[System]
public unsafe partial class System2<C1, C2> where C1 : struct, IComponent
    where C2 : struct, IComponent {
    private readonly delegate*<ref C1, ref C2, void> _method;
    public System2(delegate*<ref C1, ref C2, void> method) : this() {
        _method = method;
    }

    [SystemMethod(SystemMethodType.Update)]
    public void Update(ref C1 c1, ref C2 c2) => _method(ref c1, ref c2);
}

[System]
public unsafe partial class System3<C1, C2, C3> where C1 : struct, IComponent
    where C2 : struct, IComponent
    where C3 : struct, IComponent {
    private readonly delegate*<ref C1, ref C2, ref C3, void> _method;
    public System3(delegate*<ref C1, ref C2, ref C3, void> method) : this() {
        _method = method;
    }
    [SystemMethod(SystemMethodType.Update)]
    public void Update(ref C1 c1, ref C2 c2, ref C3 c3) => _method(ref c1, ref c2, ref c3);
}

[System]
public unsafe partial class System4<C1, C2, C3, C4> where C1 : struct, IComponent
    where C2 : struct, IComponent
    where C3 : struct, IComponent
    where C4 : struct, IComponent {
    private readonly delegate*<ref C1, ref C2, ref C3, ref C4, void> _method;
    public System4(delegate*<ref C1, ref C2, ref C3, ref C4, void> method) : this() {
        _method = method;
    }
    [SystemMethod(SystemMethodType.Update)]
    public void Update(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4) => _method(ref c1, ref c2, ref c3, ref c4);
}
