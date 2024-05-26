using Benchmark._Context;
using fennecs;

namespace Benchmark.Fennecs;

public class FennecsContext : BenchmarkContextBase
{
    private World? _world;
    private World.WorldLock _lock;
    private Dictionary<int, Query>? _queries;
    private List<Action<float>>? _systems;
    
    public override int EntityCount => _world!.Count;

    public override void Setup(int entityCount)
    {
        _world = new World(entityCount);
        _queries = new Dictionary<int, Query>();
        _systems = new List<Action<float>>();
    }

    public override void Warmup<T1>(int poolId) => _queries![poolId] = _world!.Query<T1>().Build().Warmup();

    public override void Warmup<T1, T2>(int poolId) => _queries![poolId] = _world!.Query<T1, T2>().Build().Warmup();

    public override void Warmup<T1, T2, T3>(int poolId) => _queries![poolId] = _world!.Query<T1, T2, T3>().Build().Warmup();

    public override void Warmup<T1, T2, T3, T4>(int poolId) => _queries![poolId] = _world!.Query<T1, T2, T3, T4>().Build().Warmup();

    public override void Cleanup()
    {
        _systems!.Clear();
        _systems = null;
        
        _queries!.Clear();
        _queries = null;
        
        _world!.GC();
        _world.Dispose();
        _world = null;
    }

    public override void Lock() => _lock = _world!.Lock;

    public override void Commit() => _lock.Dispose();

    public override void CreateEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn();
    }

    public override void CreateEntities<T1>(in object entitySet, in int poolId = -1, in T1 c1 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1);
    }

    public override void CreateEntities<T1, T2>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1).Add(c2);
    }

    public override void CreateEntities<T1, T2, T3>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1).Add(c2).Add(c3);
    }

    public override void CreateEntities<T1, T2, T3, T4>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add(c1).Add(c2).Add(c3).Add(c4);
    }

    public override void DeleteEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        // TODO perform use overload which utilizes ReadOnlySpan<Identity>
        for (var i = 0; i < entities.Length; i++)
            _world!.Despawn(entities[i].Id);
    }

    public override void AddComponent<T1>(in object entitySet, in int poolId = -1, in T1 c1 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1);
    }

    public override void AddComponent<T1, T2>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1).Add(c2);
    }

    public override void AddComponent<T1, T2, T3>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1).Add(c2).Add(c3);
    }

    public override void AddComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add(c1).Add(c2).Add(c3).Add(c4);
    }

    public override void RemoveComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Remove<T1>();
    }

    public override void RemoveComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Remove<T1>().Remove<T2>();
    }

    public override void RemoveComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Remove<T1>().Remove<T2>().Remove<T3>();
    }

    public override void RemoveComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Remove<T1>().Remove<T2>().Remove<T3>().Remove<T4>();
    }

    public override int CountWith<T1>(int poolId) => _world!.Query<T1>().Build().Count;

    public override int CountWith<T1, T2>(int poolId) => _world!.Query<T1, T2>().Build().Count;

    public override int CountWith<T1, T2, T3>(int poolId) => _world!.Query<T1, T2, T3>().Build().Count;

    public override int CountWith<T1, T2, T3, T4>(int poolId) => _world!.Query<T1, T2, T3, T4>().Build().Count;

    public override void Tick(float delta)
    {
        for (int i = 0; i < _systems!.Count; i++)
            _systems[i](delta);
    } 
    
    public override unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
    {
        _systems!.Add(f => ((Query<T1>)_queries![poolId]).For(Invoke));
        return;

        void Invoke(ref T1 c0) => method(ref c0);
    }

    public override unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
    {
        _systems!.Add(f => ((Query<T1, T2>)_queries![poolId]).For(Invoke));
        return;

        void Invoke(ref T1 c0, ref T2 c1) => method(ref c0, ref c1);
    }

    public override unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
    {
        _systems!.Add(f => ((Query<T1, T2, T3>)_queries![poolId]).For(Invoke));
        return;

        void Invoke(ref T1 c0, ref T2 c1, ref T3 c2) => method(ref c0, ref c1, ref c2);
    }

    public override unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
    {
        _systems!.Add(f => ((Query<T1, T2, T3, T4>)_queries![poolId]).For(Invoke));
        return;

        void Invoke(ref T1 c0, ref T2 c1, ref T3 c2, ref T4 c3) => method(ref c0, ref c1, ref c2, ref c3);
    }

    public override object Shuffle(in object entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }
    
    public override object PrepareSet(in int count) => new Entity[count];
}