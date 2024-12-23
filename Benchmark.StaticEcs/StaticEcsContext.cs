using Benchmark._Context;
using FFS.Libraries.StaticEcs;
using StaticEcsComponent = FFS.Libraries.StaticEcs.IComponent;
using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using XenoComponent = Xeno.IComponent;
using FrifloComponent = Friflo.Engine.ECS.IComponent;

namespace Benchmark.StaticEcs;

public abstract class Ecs : Ecs<Default> {}
public abstract class World : Ecs<Default>.World {}
public abstract class Systems : Systems<DefaultSystemsId> {}

public sealed class StaticEcsContext : IBenchmarkContext
{
    public bool DeletesEntityOnLastComponentDeletion => true;

    public int EntityCount => World.EntitiesCount();

    public void Setup()
    {
        Ecs.Create(EcsConfig.Default());
        Ecs.Initialize();
        Systems.Create();
    }

    public void FinishSetup()
    {
        Systems.Initialize();
    }

    public void Cleanup()
    {
        Systems.Destroy();
    }

    public void Dispose()
    {
        Ecs.Destroy();
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent { }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent { }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent { }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent { }

    public void CreateEntities(in Array entitySet)
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = Ecs.Entity.New();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = Ecs.Entity.New(c1);
        }
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = Ecs.Entity.New(c1, c2);

        }
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = Ecs.Entity.New(c1, c2, c3);

        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = Ecs.Entity.New(c1, c2, c3, c4);

        }
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Destroy();
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1);
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Add(c1, c2);
        }
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Add(c1, c2, c3);
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default,
        in T2 c2 = default, in T3 c3 = default, in T4 c4 = default) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Add(c1, c2, c3, c4);
        }
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Delete<T1>();
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Delete<T1, T2>();
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Delete<T1, T2, T3>();
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        var entities = (Ecs.Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Delete<T1, T2, T3, T4>();
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        return World.QueryEntities.For<All<T1>>().EntitiesCount();
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        return World.QueryEntities.For<All<T1, T2>>().EntitiesCount();
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        return World.QueryEntities.For<All<T1, T2, T3>>().EntitiesCount();
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        return World.QueryEntities.For<All<T1, T2, T3, T4>>().EntitiesCount();
    }

    public bool GetSingle<T1>(in object entity, in int poolId, ref T1 c1) where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        if (entity == null) return false;

        var e = (Ecs.Entity)entity;
        c1 = e.Ref<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in object entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent 
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        if (entity == null) return false;

        var e = (Ecs.Entity)entity;
        c1 = e.Ref<T1>();
        c2 = e.Ref<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        if (entity == null) return false;

        var e = (Ecs.Entity)entity;
        c1 = e.Ref<T1>();
        c2 = e.Ref<T2>();
        c3 = e.Ref<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        if (entity == null) return false;

        var e = (Ecs.Entity)entity;
        c1 = e.Ref<T1>();
        c2 = e.Ref<T2>();
        c3 = e.Ref<T3>();
        c4 = e.Ref<T4>();
        return true;
    }

    public void Tick(float delta)
    {
        Systems.Update();
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        Ecs.Context.Value.Replace(new DelegateHolder<T1> { method = method });
        Systems.AddUpdateSystem<System<T1>>();
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent 
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        Ecs.Context.Value.Replace(new DelegateHolder<T1, T2> { method = method });
        Systems.AddUpdateSystem<System<T1, T2>>();
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        Ecs.Context.Value.Replace(new DelegateHolder<T1, T2, T3> { method = method });
        Systems.AddUpdateSystem<System<T1, T2, T3>>();
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
    {
        Ecs.Context.Value.Replace(new DelegateHolder<T1, T2, T3, T4> { method = method });
        Systems.AddUpdateSystem<System<T1, T2, T3, T4>>();
    }

    public Array PrepareSet(in int count)
    {
        return count > 0 ? new Ecs.Entity[count] : [];
    }
}

public unsafe struct System<T1> : IUpdateSystem
    where T1 : struct, StaticEcsComponent
{

    public void Update() {
        foreach (ref var c1 in World.QueryComponents.For<T1>()) {
            Ecs.Context.Value.Get<DelegateHolder<T1>>().method(ref c1);
        }
    }
}

public unsafe struct System<T1, T2> : IUpdateSystem
    where T1 : struct, StaticEcsComponent
    where T2 : struct, StaticEcsComponent
{

    public void Update() 
    {
        World.QueryComponents.For(static (Ecs.Entity _, ref T1 c1, ref T2 c2) => {
            Ecs.Context.Value.Get<DelegateHolder<T1, T2>>().method(ref c1, ref c2);
        });
    }
}

public unsafe struct System<T1, T2, T3> : IUpdateSystem
    where T1 : struct, StaticEcsComponent
    where T2 : struct, StaticEcsComponent
    where T3 : struct, StaticEcsComponent
{
    public void Update() 
    {
        World.QueryComponents.For(static (Ecs.Entity _, ref T1 c1, ref T2 c2, ref T3 c3) => {
            Ecs.Context.Value.Get<DelegateHolder<T1, T2, T3>>().method(ref c1, ref c2, ref c3);
        });
    }
}

public unsafe struct System<T1, T2, T3, T4> : IUpdateSystem
    where T1 : struct, StaticEcsComponent
    where T2 : struct, StaticEcsComponent
    where T3 : struct, StaticEcsComponent
    where T4 : struct, StaticEcsComponent {

    public void Update() 
    {
        World.QueryComponents.For(static (Ecs.Entity _, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) => {
            Ecs.Context.Value.Get<DelegateHolder<T1, T2, T3, T4>>().method(ref c1, ref c2, ref c3, ref c4);
        });
    }

}

public struct DelegateHolder<T1>
    where T1 : struct, StaticEcsComponent {
    public unsafe delegate*<ref T1, void> method;
}
    
public struct DelegateHolder<T1, T2>
    where T1 : struct, StaticEcsComponent
    where T2 : struct, StaticEcsComponent {
    public unsafe delegate*<ref T1, ref T2, void> method;
}
    
public struct DelegateHolder<T1, T2, T3>
    where T1 : struct, StaticEcsComponent
    where T2 : struct, StaticEcsComponent
    where T3 : struct, StaticEcsComponent {
    public unsafe delegate*<ref T1, ref T2, ref T3, void> method;
}

public struct DelegateHolder<T1, T2, T3, T4>
    where T1 : struct, StaticEcsComponent
    where T2 : struct, StaticEcsComponent
    where T3 : struct, StaticEcsComponent
    where T4 : struct, StaticEcsComponent {
    public unsafe delegate*<ref T1, ref T2, ref T3, ref T4, void> method;
}