using AECS;
using Benchmark.Context;
using DCFApixels.DragonECS;
using Scellecs.Morpeh;

namespace Benchmark.ArrayECS;

public class ArrayECSContext : IBenchmarkContext<ulong> {
    [Ignore] private int EntityCount;

    private ArrayWorld _world;

    public bool DeletesEntityOnLastComponentDeletion => false;
    public int NumberOfLivingEntities => _world.EntityCount;

    public void Setup() {
        _world = new ArrayWorld(EntityCount);
    }

    public void FinishSetup() { }
    public void Cleanup() {
        _world.Dispose();
        _world = null;
    }

    public void Dispose() {}

    public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        _world.Preallocate<T1>(EntityCount);
    }
    public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        _world.Preallocate<T1>(EntityCount);
        _world.Preallocate<T2>(EntityCount);
    }
    public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        _world.Preallocate<T1>(EntityCount);
        _world.Preallocate<T2>(EntityCount);
        _world.Preallocate<T3>(EntityCount);
    }
    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        _world.Preallocate<T1>(EntityCount);
        _world.Preallocate<T2>(EntityCount);
        _world.Preallocate<T3>(EntityCount);
        _world.Preallocate<T4>(EntityCount);
    }
    public void DeleteEntities(in ulong[] entitySet) {
        for (int i = 0; i < entitySet.Length; i++) {
            _world.DestroyEntity(entitySet[i]);
        }
    }

    public ulong[] PrepareSet(in int count) => new ulong[count];

    public void CreateEntities(in ulong[] entitySet) {
        for (int i = 0; i < entitySet.Length; i++) {
            entitySet[i] = _world.CreateEntity();
        }
    }

    public void CreateEntities<T1>(in ulong[] entitySet, in int poolId, in T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = _world.CreateEntity();
            entitySet[i] = e;
            _world.AddComponent(e, c1);
        }
    }
    public void CreateEntities<T1, T2>(in ulong[] entitySet, in int poolId, in T1 c1, in T2 c2) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = _world.CreateEntity();
            entitySet[i] = e;
            _world.AddComponent(e, c1);
            _world.AddComponent(e, c2);
        }
    }
    public void CreateEntities<T1, T2, T3>(in ulong[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = _world.CreateEntity();
            entitySet[i] = e;
            _world.AddComponent(e, c1);
            _world.AddComponent(e, c2);
            _world.AddComponent(e, c3);
        }
    }
    public void CreateEntities<T1, T2, T3, T4>(in ulong[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = _world.CreateEntity();
            entitySet[i] = e;
            _world.AddComponent(e, c1);
            _world.AddComponent(e, c2);
            _world.AddComponent(e, c3);
            _world.AddComponent(e, c4);
        }
    }
    public void AddComponent<T1>(in ulong[] entitySet, in int poolId, in T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = entitySet[i];
            _world.AddComponent(e, c1);
        }
    }
    public void AddComponent<T1, T2>(in ulong[] entitySet, in int poolId, in T1 c1, in T2 c2) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = entitySet[i];
            _world.AddComponent(e, c1);
            _world.AddComponent(e, c2);
        }
    }
    public void AddComponent<T1, T2, T3>(in ulong[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = entitySet[i];
            _world.AddComponent(e, c1);
            _world.AddComponent(e, c2);
            _world.AddComponent(e, c3);
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in ulong[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = entitySet[i];
            _world.AddComponent(e, c1);
            _world.AddComponent(e, c2);
            _world.AddComponent(e, c3);
            _world.AddComponent(e, c4);
        }
    }
    public void RemoveComponent<T1>(in ulong[] entitySet, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = entitySet[i];
            _world.RemoveComponent<T1>(e);
        }
    }

    public void RemoveComponent<T1, T2>(in ulong[] entitySet, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = entitySet[i];
            _world.RemoveComponent<T1>(e);
            _world.RemoveComponent<T2>(e);
        }
    }

    public void RemoveComponent<T1, T2, T3>(in ulong[] entitySet, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = entitySet[i];
            _world.RemoveComponent<T1>(e);
            _world.RemoveComponent<T2>(e);
            _world.RemoveComponent<T3>(e);
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in ulong[] entitySet, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        for (int i = 0; i < entitySet.Length; i++) {
            var e = entitySet[i];
            _world.RemoveComponent<T1>(e);
            _world.RemoveComponent<T2>(e);
            _world.RemoveComponent<T3>(e);
            _world.RemoveComponent<T4>(e);
        }
    }

    public int CountWith<T1>(in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _world.Count<T1>();

    public int CountWith<T1, T2>(in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _world.Count<T1, T2>();

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _world.Count<T1, T2, T3>();

    public int CountWith<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _world.Count<T1, T2, T3, T4>();

    public bool GetSingle<T1>(in ulong entity, in int poolId, ref T1 c1)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _world.Ref(entity, ref c1);

    public bool GetSingle<T1, T2>(in ulong entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        => _world.Ref(entity, ref c1) && _world.Ref(entity, ref c2);

    public bool GetSingle<T1, T2, T3>(in ulong entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.Ref(entity, ref c1) && _world.Ref(entity, ref c2) && _world.Ref(entity, ref c3);
    }

    public bool GetSingle<T1, T2, T3, T4>(in ulong entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        return _world.Ref(entity, ref c1) && _world.Ref(entity, ref c2) && _world.Ref(entity, ref c3) && _world.Ref(entity, ref c4);
    }

    public void Tick(float delta) => _world.Update(delta);

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent {
        _world.AddSystem(new System<T1>(method));
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _world.AddSystem(new System<T1, T2>(method));
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent

    {
        _world.AddSystem(new System<T1, T2, T3>(method));
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        _world.AddSystem(new System<T1, T2, T3, T4>(method));
    }

    private unsafe class System<T1>(delegate*<ref T1, void> method) : ArraySystem<T1> {
        protected override void OnUpdate(ref T1 c) => method(ref c);
    }

    private unsafe class System<T1, T2>(delegate*<ref T1, ref T2, void> method) : ArraySystem<T1, T2> {
        protected override void OnUpdate(ref T1 c1, ref T2 c2) => method(ref c1, ref c2);
    }

    private unsafe class System<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method) : ArraySystem<T1, T2, T3> {
        protected override void OnUpdate(ref T1 c1, ref T2 c2, ref T3 c3) => method(ref c1, ref c2, ref c3);
    }

    private unsafe class System<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method) : ArraySystem<T1, T2, T3, T4> {
        protected override void OnUpdate(ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) => method(ref c1, ref c2, ref c3, ref c4);
    }
}
