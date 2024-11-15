using Leopotam.Ecs;

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

namespace Benchmark.LeoEcs;

public unsafe class System<T1>(delegate*<ref T1, void> method) : IEcsRunSystem
    where T1 : struct
{
    private readonly EcsFilter<T1> _filter;

    public void Run()
    {
        for (int i = 0, iMax = _filter.GetEntitiesCount(); i < iMax; i++)
            method(ref _filter.Get1(i));
    }
}

public unsafe class System<T1, T2>(delegate*<ref T1, ref T2, void> method) : IEcsRunSystem
    where T1 : struct
    where T2 : struct
{
    private readonly EcsFilter<T1, T2> _filter;

    public void Run()
    {
        for (int i = 0, iMax = _filter.GetEntitiesCount(); i < iMax; i++)
            method(ref _filter.Get1(i), ref _filter.Get2(i));
    }
}

public unsafe class System<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method) : IEcsRunSystem
    where T1 : struct
    where T2 : struct
    where T3 : struct
{
    private readonly EcsFilter<T1, T2, T3> _filter;

    public void Run()
    {
        for (int i = 0, iMax = _filter.GetEntitiesCount(); i < iMax; i++)
            method(ref _filter.Get1(i), ref _filter.Get2(i), ref _filter.Get3(i));
    }
}

public unsafe class System<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method) : IEcsRunSystem
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
{
    private readonly EcsFilter<T1, T2, T3, T4> _filter;

    public void Run()
    {
        for (int i = 0, iMax = _filter.GetEntitiesCount(); i < iMax; i++)
            method(ref _filter.Get1(i), ref _filter.Get2(i), ref _filter.Get3(i), ref _filter.Get4(i));
    }
}