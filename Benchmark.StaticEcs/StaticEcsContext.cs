using System.Runtime.CompilerServices;
using Benchmark.Context;
using DCFApixels.DragonECS;
using FFS.Libraries.StaticEcs;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace Benchmark.StaticEcs;

public readonly struct Default : IWorldType;
public readonly struct DefaultSystemsId : ISystemsType;

public abstract class W : World<Default> {}
public abstract class Systems : W.Systems<DefaultSystemsId> {}

public readonly struct EntityType1 : IEntityType { public static readonly byte Id = 1; }
public readonly struct EntityType2 : IEntityType { public static readonly byte Id = 2; }
public readonly struct EntityType3 : IEntityType { public static readonly byte Id = 3; }
public readonly struct EntityType4 : IEntityType { public static readonly byte Id = 4; }

public sealed class StaticEcsContext : IBenchmarkContext<W.Entity>
{
    public bool DeletesEntityOnLastComponentDeletion => false;

    public int NumberOfLivingEntities => (int) W.CalculateEntitiesCount();

    public void Setup()
    {
        W.Create(WorldConfig.Default());
        W.Types()
         .EntityType<EntityType1>(EntityType1.Id)
         .EntityType<EntityType2>(EntityType2.Id)
         .EntityType<EntityType3>(EntityType3.Id)
         .EntityType<EntityType4>(EntityType4.Id);
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
        if (W.Components<T>.Instance.IsRegistered) {
            return;
        }
        
        W.Types().Component(new ComponentTypeConfig<T>(noDataLifecycle: true));
    }

    [MethodImpl(AggressiveInlining)]
    public byte ResolveEntityType(int poolId) => (byte)(poolId > 4 ? 0 : poolId); 
 
    public void CreateEntities(in W.Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i] = W.NewEntity<FFS.Libraries.StaticEcs.Default>();
    }

    public void CreateEntities<T1>(in W.Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        var entType = ResolveEntityType(poolId);
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = W.NewEntity(entType).Set(c1);
        }
    }

    public void CreateEntities<T1, T2>(in W.Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        var entType = ResolveEntityType(poolId);
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = W.NewEntity(entType).Set(c1, c2);
        }
    }

    public void CreateEntities<T1, T2, T3>(in W.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        var entType = ResolveEntityType(poolId);
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = W.NewEntity(entType).Set(c1, c2, c3);
        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in W.Entity[] entities, in int poolId, in T1 c1,
        in T2 c2, in T3 c3, in T4 c4) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        var entType = ResolveEntityType(poolId);
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i] = W.NewEntity(entType).Set(c1, c2, c3, c4);
        }
    }

    public void DeleteEntities(in W.Entity[] entities)
    {
        for (var i = 0; i < entities.Length; i++) {
            var entity = entities[i];
            if (entity.IsNotDestroyed) {
                entity.Destroy();
            }
        }
    }

    public void AddComponent<T1>(in W.Entity[] entities, in int poolId, in T1 c1)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
            entities[i].Set(c1);
    }

    public void AddComponent<T1, T2>(in W.Entity[] entities, in int poolId, in T1 c1, in T2 c2)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Set(c1, c2);
        }
    }

    public void AddComponent<T1, T2, T3>(in W.Entity[] entities, in int poolId, in T1 c1, in T2 c2,
        in T3 c3) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        for (var i = 0; i < entities.Length; i++)
        {
            entities[i].Set(c1, c2, c3);
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
            entities[i].Set(c1, c2, c3, c4);
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
        return W.Query<All<T1>>().EntitiesCount();
    }

    public int CountWith<T1, T2>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        return W.Query<All<T1, T2>>().EntitiesCount();
    }

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        return W.Query<All<T1, T2, T3>>().EntitiesCount();
    }

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        return W.Query<All<T1, T2, T3, T4>>().EntitiesCount();
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
        Systems.Add(new System<T1> {
            method = @delegate
        });
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> @delegate, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        Systems.Add(new System<T1, T2> {
            method = @delegate
        });
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> @delegate, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        Systems.Add(new System<T1, T2, T3> {
            method = @delegate
        });
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> @delegate, int poolId)
        where T1 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T3 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
        where T4 : struct, Scellecs.Morpeh.IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, IComponent
    {
        Systems.Add(new System<T1, T2, T3, T4> {
            method = @delegate
        });
    }

    public W.Entity[] PrepareSet(in int count)
    {
        return count > 0 ? new W.Entity[count] : [];
    }
}

public unsafe struct System<T1> : ISystem
    where T1 : struct, IComponent
{
    public delegate*<ref T1, void> method;
    
    [MethodImpl(AggressiveInlining)]
    public void Update() {
        W.Query().For(method);
    }
}

public unsafe struct System<T1, T2> : ISystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
{
    public delegate*<ref T1, ref T2, void> method;
    
    [MethodImpl(AggressiveInlining)]
    public void Update() {
        W.Query().For(method);
    }
}

public unsafe struct System<T1, T2, T3> : ISystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
{
    public delegate*<ref T1, ref T2, ref T3, void> method;
    
    [MethodImpl(AggressiveInlining)]
    public void Update() {
        W.Query().For(method);
    }
}

public unsafe struct System<T1, T2, T3, T4> : ISystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
    where T4 : struct, IComponent
{
    public delegate*<ref T1, ref T2, ref T3, ref T4, void> method;
    
    [MethodImpl(AggressiveInlining)]
    public void Update() {
        W.Query().For(method);
    }
}
