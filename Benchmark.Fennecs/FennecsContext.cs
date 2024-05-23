using Benchmark._Context;
using fennecs;

namespace Benchmark.Fennecs;

public class FennecsContext : BenchmarkContextBase
{
    private World? _world;
    private World.WorldLock _lock;
    
    public override int EntityCount => _world!.Count;

    public override void Setup(int entityCount)
    {
        _world = new World(entityCount);
    }

    public override void Warmup<T1>(int poolId) => _world!.Query<T1>().Build().Warmup();

    public override void Warmup<T1, T2>(int poolId) => _world!.Query<T1, T2>().Build().Warmup();

    public override void Warmup<T1, T2, T3>(int poolId) => _world!.Query<T1, T2, T3>().Build().Warmup();

    public override void Warmup<T1, T2, T3, T4>(int poolId) => _world!.Query<T1, T2, T3, T4>().Build().Warmup();

    public override void Cleanup()
    {
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

    public override void CreateEntities<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add<T1>();
    }

    public override void CreateEntities<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add<T1>().Add<T2>();
    }

    public override void CreateEntities<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add<T1>().Add<T2>().Add<T3>();
    }

    public override void CreateEntities<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _world!.Spawn().Add<T1>().Add<T2>().Add<T3>().Add<T4>();
    }

    public override void DeleteEntities(in object entitySet)
    {
        var entities = (Entity[])entitySet;
        // TODO perform use overload which utilizes ReadOnlySpan<Identity>
        for (var i = 0; i < entities.Length; i++)
            _world!.Despawn(entities[i].Id);
    }

    public override void AddComponent<T1>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add<T1>();
    }

    public override void AddComponent<T1, T2>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add<T1>().Add<T2>();
    }

    public override void AddComponent<T1, T2, T3>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add<T1>().Add<T2>().Add<T3>();
    }

    public override void AddComponent<T1, T2, T3, T4>(in object entitySet, in int poolId = -1)
    {
        var entities = (Entity[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i].Add<T1>().Add<T2>().Add<T3>().Add<T4>();
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

    public override object Shuffle(in object entitySet)
    {
        Random.Shared.Shuffle((Entity[])entitySet);
        return entitySet;
    }
    
    public override object PrepareSet(in int count) => new Entity[count];
}