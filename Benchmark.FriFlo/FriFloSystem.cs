using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;

namespace Benchmark.FriFlo;

public unsafe class FriFloSystem<T1>(delegate*<ref T1, void> method) : QuerySystem<T1>
    where T1 : struct, IComponent
{
    protected override void OnUpdate()
    {
        var job = Query.ForEach(Job);
        job.Run();
    }

    private void Job(Chunk<T1> c1, ChunkEntities entities)
    {
        for (var i = 0; i < c1.Length; i++) {
            method(ref c1[i]);
        }
    }
}

public unsafe class FriFloSystem<T1, T2>(delegate*<ref T1, ref T2, void> method) : QuerySystem<T1, T2>
    where T1 : struct, IComponent
    where T2 : struct, IComponent
{
    protected override void OnUpdate()
    {
        var job = Query.ForEach(Job);
        job.Run();
    }

    private void Job(Chunk<T1> c1, Chunk<T2> c2, ChunkEntities entities)
    {
        for (var i = 0; i < entities.Length; i++) {
            method(ref c1[i], ref c2[i]);
        }
    }
}

public unsafe class FriFloSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method) : QuerySystem<T1, T2, T3>
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
{
    protected override void OnUpdate()
    {
        var job = Query.ForEach(Job);
        job.Run();
    }

    private void Job(Chunk<T1> c1, Chunk<T2> c2, Chunk<T3> c3, ChunkEntities entities)
    {
        for (var i = 0; i < entities.Length; i++) {
            method(ref c1[i], ref c2[i], ref c3[i]);
        }
    }
}

public unsafe class FriFloSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method) : QuerySystem<T1, T2, T3, T4>
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
    where T4 : struct, IComponent
{
    protected override void OnUpdate()
    {
        var job = Query.ForEach(Job);
        job.Run();
    }

    private void Job(Chunk<T1> c1, Chunk<T2> c2, Chunk<T3> c3, Chunk<T4> c4, ChunkEntities entities)
    {
        for (var i = 0; i < entities.Length; i++) {
            method(ref c1[i], ref c2[i], ref c3[i], ref c4[i]);
        }
    }
}