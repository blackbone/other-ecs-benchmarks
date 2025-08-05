using Benchmark.Context;
using DCFApixels.DragonECS;
using FFS.Libraries.StaticEcs;

namespace Benchmark.StaticEcs;

public readonly struct Default : IWorldType;
public readonly struct DefaultSystemsId : ISystemsType;

public abstract class W : World<Default> {}
public abstract class Systems : W.Systems<DefaultSystemsId> {}

public sealed class StaticEcsContext : IBenchmarkContext<W.Entity>
{
    public bool DeletesEntityOnLastComponentDeletion => false;

    public int NumberOfLivingEntities => (int) W.EntitiesCount();

    public void Setup()
    {
        W.Create(WorldConfig.Default());
        Systems.Create();
    }

    public void FinishSetup()
    {
        W.Initialize();
        Systems.Initialize();
    }

    public void Cleanup()
    {
        Systems.Destroy();
    }

    public void Dispose()
    {
        W.Destroy();
    }

    public void Warmup<T1>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        TryRegisterComponent<T1>();
    }

    public void Warmup<T1, T2>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        TryRegisterComponent<T1>();
        TryRegisterComponent<T2>();
    }

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        TryRegisterComponent<T1>();
        TryRegisterComponent<T2>();
        TryRegisterComponent<T3>();
    }

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        TryRegisterComponent<T1>();
        TryRegisterComponent<T2>();
        TryRegisterComponent<T3>();
        TryRegisterComponent<T4>();
    }

    private void TryRegisterComponent<T>() where T : struct, IComponent {
        if (W.Components<T>.Value.IsRegistered()) {
            return;
        }
        
        W.RegisterComponentType<T>();
    }

    public void CreateEntities(in W.Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = W.Entity.New();
    }

    public void CreateEntities<T1>(in W.Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = W.Entity.New(c1);
        }
    }

    public void CreateEntities<T1, T2>(in W.Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = W.Entity.New(c1, c2);

        }
    }

    public void CreateEntities<T1, T2, T3>(in W.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = W.Entity.New(c1, c2, c3);

        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in W.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3, in T4 c4) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = W.Entity.New(c1, c2, c3, c4);

        }
    }

    public void DeleteEntities(in W.Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Destroy();
    }

    public void AddComponent<T1>(in W.Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1);
    }

    public void AddComponent<T1, T2>(in W.Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Add(c1, c2);
        }
    }

    public void AddComponent<T1, T2, T3>(in W.Entity[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Add(c1, c2, c3);
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in W.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3, in T4 c4) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Add(c1, c2, c3, c4);
        }
    }

    public void RemoveComponent<T1>(in W.Entity[] entities, in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Delete<T1>();
    }

    public void RemoveComponent<T1, T2>(in W.Entity[] entities, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Delete<T1, T2>();
        }
    }

    public void RemoveComponent<T1, T2, T3>(in W.Entity[] entities, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Delete<T1, T2, T3>();
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in W.Entity[] entities, in int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Delete<T1, T2, T3, T4>();
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        return W.QueryEntities.For<All<T1>>().EntitiesCount();
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        return W.QueryEntities.For<All<T1, T2>>().EntitiesCount();
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        return W.QueryEntities.For<All<T1, T2, T3>>().EntitiesCount();
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        return W.QueryEntities.For<All<T1, T2, T3, T4>>().EntitiesCount();
    }

    public bool GetSingle<T1>(in W.Entity e, in int poolId, ref T1 c1) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        c1 = e.Ref<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in W.Entity e, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        c1 = e.Ref<T1>();
        c2 = e.Ref<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in W.Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        c1 = e.Ref<T1>();
        c2 = e.Ref<T2>();
        c3 = e.Ref<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in W.Entity e, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
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

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> @delegate, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        Systems.AddUpdate(new System<T1> {
            method = @delegate
        });
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> @delegate, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        Systems.AddUpdate(new System<T1, T2> {
            method = @delegate
        });
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> @delegate, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        Systems.AddUpdate(new System<T1, T2, T3> {
            method = @delegate
        });
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> @delegate, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        Systems.AddUpdate(new System<T1, T2, T3, T4> {
            method = @delegate
        });
    }

    public W.Entity[] PrepareSet(in int count)
    {
        return count > 0 ? new W.Entity[count] : [];
    }
}

public unsafe struct System<T1> : IUpdateSystem
    where T1 : struct, IComponent
{

    public delegate*<ref T1, void> method;
    
    public void Update() {
        W.QueryComponents.For(ref this, static (ref System<T1> self, ref T1 c1) => {
            self.method(ref c1);
        });
    }
}

public unsafe struct System<T1, T2> : IUpdateSystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
{
    
    public delegate*<ref T1, ref T2, void> method;

    public void Update()
    {
        W.QueryComponents.For(ref this, static (ref System<T1, T2> self, ref T1 c1, ref T2 c2) => {
            self.method(ref c1, ref c2);
        });
    }
}

public unsafe struct System<T1, T2, T3> : IUpdateSystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
{
    
    public delegate*<ref T1, ref T2, ref T3, void> method;
    
    public void Update()
    {
        W.QueryComponents.For(ref this, static (ref System<T1, T2, T3> self, ref T1 c1, ref T2 c2, ref T3 c3) => {
            self.method(ref c1, ref c2, ref c3);
        });
    }
}

public unsafe struct System<T1, T2, T3, T4> : IUpdateSystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
    where T4 : struct, IComponent {
    
    
    public delegate*<ref T1, ref T2, ref T3, ref T4, void> method;

    public void Update()
    {
        W.QueryComponents.For(ref this, static (ref System<T1, T2, T3, T4> self, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) => {
            self.method(ref c1, ref c2, ref c3, ref c4);
        });
    }

}
