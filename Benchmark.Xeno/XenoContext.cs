using Benchmark._Context;
using Xeno;
using World = Xeno.World;
using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using XenoComponent = Xeno.IComponent;

namespace Benchmark.Xeno;

public class XenoContext(int entityCount = 4096) : IBenchmarkContext
{
    private World? _world;

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int EntityCount => (int)_world!.EntityCount;

    public void Dispose()
    {
    }

    public void Setup()
    {
        _world = Worlds.Create($"xeno_world_{DateTimeOffset.UtcNow.Ticks}");
        _world!.EnsureCapacity(EntityCount);
    }

    public void FinishSetup()
    {
        _world!.Start();
    }

    public void Warmup<T1>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        _world!.EnsureCapacity<T1>(entityCount);
    }

    public void Warmup<T1, T2>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        _world!.EnsureCapacity<T1>(entityCount);
        _world!.EnsureCapacity<T2>(entityCount);
    }

    public void Warmup<T1, T2, T3>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        _world!.EnsureCapacity<T1>(entityCount);
        _world!.EnsureCapacity<T2>(entityCount);
        _world!.EnsureCapacity<T3>(entityCount);
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        _world!.EnsureCapacity<T1>(entityCount);
        _world!.EnsureCapacity<T2>(entityCount);
        _world!.EnsureCapacity<T3>(entityCount);
        _world!.EnsureCapacity<T4>(entityCount);
    }

    public void Cleanup()
    {
        _world!.Dispose();
        _world = null;
    }

    public void Lock()
    {
    }

    public void Commit()
    {
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            _world!.DeleteEntity(entities[i]);
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

    public void CreateEntities(in Array entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity(c1);
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity(c1, c2);
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity(c1, c2, c3);
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.CreateEntity(c1, c2, c3, c4);
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponent(c1);
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponents(c1, c2);
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponents(c1, c2, c3);
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].AddComponents(c1, c2, c3, c4);
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].RemoveComponent<T1>(out _);
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].RemoveComponents<T1, T2>(out _, out _);
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].RemoveComponents<T1, T2, T3>(out _, out _, out _);
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].RemoveComponents<T1, T2, T3, T4>(out _, out _, out _, out _);
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        return _world!.Count<T1>();
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        return _world!.Count<T1, T2>();
    }

    public int CountWith<T1, T2, T3>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        return _world!.Count<T1, T2, T3>();
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        return _world!.Count<T1, T2, T3, T4>();
    }

    public bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.AccessComponent<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.AccessComponent<T1>();
        c2 = e.AccessComponent<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.AccessComponent<T1>();
        c2 = e.AccessComponent<T2>();
        c3 = e.AccessComponent<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3,
        ref T4 c4) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        if (entity == null) return false;

        var e = (Entity)entity;
        c1 = e.AccessComponent<T1>();
        c2 = e.AccessComponent<T2>();
        c3 = e.AccessComponent<T3>();
        c4 = e.AccessComponent<T4>();
        return true;
    }

    public void Tick(float delta)
    {
        _world!.Tick(delta);
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        _world!.AddSystem(new System1<T1>(method));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        _world!.AddSystem(new System2<T1, T2>(method));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        _world!.AddSystem(new System3<T1, T2, T3>(method));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent
    {
        _world!.AddSystem(new System4<T1, T2, T3, T4>(method));
    }
}

[System]
public unsafe partial class System1<C1>
    where C1 : struct, XenoComponent
{
    private readonly delegate*<ref C1, void> _method;
    public System1(delegate*<ref C1, void> method) : this() => _method = method;
    
    [SystemMethod(SystemMethodType.Update)]
    private void Update(ref C1 c1) => _method(ref c1);
}

[System]
public unsafe partial class System2<C1, C2>
    where C1 : struct, XenoComponent
    where C2 : struct, XenoComponent
{
    private readonly delegate*<ref C1, ref C2, void> _method;
    public System2(delegate*<ref C1, ref C2, void> method) : this() => _method = method;
    [SystemMethod(SystemMethodType.Update)]
    private void Update(ref C1 c1, ref C2 c2) => _method(ref c1, ref c2);
}

[System]
public unsafe partial class System3<C1, C2, C3>
    where C1 : struct, XenoComponent
    where C2 : struct, XenoComponent
    where C3 : struct, XenoComponent
{
    private readonly delegate*<ref C1, ref C2, ref C3, void> _method;
    public System3(delegate*<ref C1, ref C2, ref C3, void> method) : this() => _method = method;
    
    [SystemMethod(SystemMethodType.Update)]
    private void Update(ref C1 c1, ref C2 c2, ref C3 c3) => _method(ref c1, ref c2, ref c3);
}

[System]
public unsafe partial class System4<C1, C2, C3, C4>
    where C1 : struct, XenoComponent
    where C2 : struct, XenoComponent
    where C3 : struct, XenoComponent
    where C4 : struct, XenoComponent
{
    private readonly delegate*<ref C1, ref C2, ref C3, ref C4, void> _method;
    public System4(delegate*<ref C1, ref C2, ref C3, ref C4, void> method) : this() => _method = method;
    
    [SystemMethod(SystemMethodType.Update)]
    private void Update(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4) => _method(ref c1, ref c2, ref c3, ref c4);
}