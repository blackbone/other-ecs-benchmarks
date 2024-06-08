using DefaultEcs;
using DefaultEcs.System;

namespace Benchmark.DefaultECS;

public sealed unsafe class System<T1>(EntityQueryBuilder query, delegate*<ref T1, void> method) : ISystem<float>
{
    public bool IsEnabled { get; set; } = true;
    public void Dispose() { }

    public void Update(float state)
    {
        foreach (var entity in query.AsEnumerable())
            method(ref entity.Get<T1>());
    }
}

public sealed unsafe class System<T1, T2>(EntityQueryBuilder query, delegate*<ref T1, ref T2, void> method) : ISystem<float>
{
    public bool IsEnabled { get; set; } = true;
    public void Dispose() { }

    public void Update(float state)
    {
        foreach (var entity in query.AsEnumerable())
            method(ref entity.Get<T1>(), ref entity.Get<T2>());
    }
}

public sealed unsafe class System<T1, T2, T3>(EntityQueryBuilder query, delegate*<ref T1, ref T2, ref T3, void> method) : ISystem<float>
{
    public bool IsEnabled { get; set; } = true;
    public void Dispose() { }

    public void Update(float state)
    {
        foreach (var entity in query.AsEnumerable())
            method(ref entity.Get<T1>(), ref entity.Get<T2>(), ref entity.Get<T3>());
    }
}

public sealed unsafe class System<T1, T2, T3, T4>(EntityQueryBuilder query, delegate*<ref T1, ref T2, ref T3, ref T4, void> method) : ISystem<float>
{
    public bool IsEnabled { get; set; } = true;
    public void Dispose() { }

    public void Update(float state)
    {
        foreach (var entity in query.AsEnumerable())
            method(ref entity.Get<T1>(), ref entity.Get<T2>(), ref entity.Get<T3>(), ref entity.Get<T4>());
    }
}