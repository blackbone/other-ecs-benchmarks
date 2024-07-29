using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;

namespace Benchmark.FriFlo;

public unsafe class FriFloSystem<T1>(ParallelJobRunner runner, delegate*<ref T1, void> method) : QuerySystem<T1>
    where T1 : struct, IComponent
{
    protected override void OnUpdate()
    {
        var job = Query.ForEach(Job);
        job.JobRunner = runner;
        job.RunParallel();
    }

    private void Job(Chunk<T1> c1, ChunkEntities entities)
    {
        foreach (var entity in entities)
            method(ref entity.GetComponent<T1>());
    }
}

public unsafe class FriFloSystem<T1, T2>(ParallelJobRunner runner, delegate*<ref T1, ref T2, void> method) : QuerySystem<T1, T2>
    where T1 : struct, IComponent
    where T2 : struct, IComponent
{
    protected override void OnUpdate()
    {
        var job = Query.ForEach(Job);
        job.JobRunner = runner;
        job.RunParallel();
    }

    private void Job(Chunk<T1> c1, Chunk<T2> c2, ChunkEntities entities)
    {
        foreach (var entity in entities)
            method(ref entity.GetComponent<T1>(), ref entity.GetComponent<T2>());
    }
}

public unsafe class FriFloSystem<T1, T2, T3>(ParallelJobRunner runner, delegate*<ref T1, ref T2, ref T3, void> method) : QuerySystem<T1, T2, T3>
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
{
    protected override void OnUpdate()
    {
        var job = Query.ForEach(Job);
        job.JobRunner = runner;
        job.RunParallel();
    }

    private void Job(Chunk<T1> c1, Chunk<T2> c2, Chunk<T3> c3, ChunkEntities entities)
    {
        foreach (var entity in entities)
            method(ref entity.GetComponent<T1>(), ref entity.GetComponent<T2>(), ref entity.GetComponent<T3>());
    }
}

public unsafe class FriFloSystem<T1, T2, T3, T4>(ParallelJobRunner runner, delegate*<ref T1, ref T2, ref T3, ref T4, void> method) : QuerySystem<T1, T2, T3, T4>
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
    where T4 : struct, IComponent
{
    protected override void OnUpdate()
    {
        var job = Query.ForEach(Job);
        job.JobRunner = runner;
        job.RunParallel();
    }

    private void Job(Chunk<T1> c1, Chunk<T2> c2, Chunk<T3> c3, Chunk<T4> c4, ChunkEntities entities)
    {
        foreach (var entity in entities)
            method(ref entity.GetComponent<T1>(), ref entity.GetComponent<T2>(), ref entity.GetComponent<T3>(), ref entity.GetComponent<T4>());
    }
}