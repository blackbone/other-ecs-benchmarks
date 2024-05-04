using Arch.Core;
using Benchmark._Context;

namespace Benchmark.Arch;

public class ArchContext : BenchmarkContextBase
{
    private World world;
    private Entity[] entities;
    private int n = -1;

    public override void Setup(int entityCount)
    {
        world = World.Create();
        entities = new Entity[entityCount];
        n = -1;
    }

    public override void Cleanup()
    {
        world?.Clear();
        world?.Dispose();
        world = null;
    }

    public override void Commit() { /* no op */ }

    public override int CreateEntity()
    {
        n++;
        var entity = world.Create();
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1>()
    {
        n++;
        var entity = world.Create<T1>();
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1, T2>()
    {
        n++;
        var entity = world.Create<T1, T2>();
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1, T2, T3>()
    {
        n++;
        var entity = world.Create<T1, T2, T3>();
        entities[n] = entity;
        return n;
    }

    public override int CreateEntity<T1, T2, T3, T4>()
    {
        n++;
        var entity = world.Create<T1, T2, T3, T4>();
        entities[n] = entity;
        return n;
    }

    public override void AddComponent<T1>(in int id) => world.Add<T1>(entities[id]);

    public override void AddComponent<T1, T2>(in int id) => world.Add<T1, T2>(entities[id]);

    public override void AddComponent<T1, T2, T3>(in int id) => world.Add<T1, T2, T3>(entities[id]);

    public override void AddComponent<T1, T2, T3, T4>(in int id) => world.Add<T1, T2, T3, T4>(entities[id]);

    public override void RemoveComponent<T1>(in int id) => world.Remove<T1>(entities[id]);

    public override void RemoveComponent<T1, T2>(in int id) => world.Remove<T1>(entities[id]);

    public override void RemoveComponent<T1, T2, T3>(in int id) => world.Remove<T1>(entities[id]);

    public override void RemoveComponent<T1, T2, T3, T4>(in int id) => world.Remove<T1>(entities[id]);
}