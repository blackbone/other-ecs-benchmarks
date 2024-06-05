using DefaultEcs;
using DefaultEcs.System;

namespace Benchmark.DefaultECS;

public sealed unsafe class System<T1>(EntityQueryBuilder query, delegate*<ref T1, void> method) : ISystem<float>
{
    public bool IsEnabled { get; set; } = true;
    public void Dispose() { }

    public void Update(float state)
    {
        var entities = query.AsSet().GetEntities();
        for (int i = 0; i < entities.Length; i++)
            method(ref entities[i].Get<T1>());
    }
}

public sealed unsafe class System<T1, T2>(EntityQueryBuilder query, delegate*<ref T1, ref T2, void> method) : ISystem<float>
{
    public bool IsEnabled { get; set; } = true;
    public void Dispose() { }

    public void Update(float state)
    {
        var entities = query.AsSet().GetEntities();
        for (int i = 0; i < entities.Length; i++)
            method(ref entities[i].Get<T1>(), ref entities[i].Get<T2>());
    }
}

public sealed unsafe class System<T1, T2, T3>(EntityQueryBuilder query, delegate*<ref T1, ref T2, ref T3, void> method) : ISystem<float>
{
    public bool IsEnabled { get; set; } = true;
    public void Dispose() { }

    public void Update(float state)
    {
        var entities = query.AsSet().GetEntities();
        for (int i = 0; i < entities.Length; i++)
            method(ref entities[i].Get<T1>(), ref entities[i].Get<T2>(), ref entities[i].Get<T3>());
    }
}

public sealed unsafe class System<T1, T2, T3, T4>(EntityQueryBuilder query, delegate*<ref T1, ref T2, ref T3, ref T4, void> method) : ISystem<float>
{
    public bool IsEnabled { get; set; } = true;
    public void Dispose() { }

    public void Update(float state)
    {
        var entities = query.AsSet().GetEntities();
        for (int i = 0; i < entities.Length; i++)
            method(ref entities[i].Get<T1>(), ref entities[i].Get<T2>(), ref entities[i].Get<T3>(), ref entities[i].Get<T4>());
    }
}