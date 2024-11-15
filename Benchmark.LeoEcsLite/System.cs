using Leopotam.EcsLite;

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

namespace Benchmark.LeoEcsLite;

public unsafe class System<T1>(delegate*<ref T1, void> method) : IEcsInitSystem, IEcsRunSystem
    where T1 : struct
{
    private EcsFilter _filter;
    private EcsPool<T1> _p1;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<T1>().End();
        _p1 = world.GetPool<T1>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter!)
            method(ref _p1!.Get(entity));
    }
}

public unsafe class System<T1, T2>(delegate*<ref T1, ref T2, void> method) : IEcsInitSystem, IEcsRunSystem
    where T1 : struct
    where T2 : struct
{
    private EcsFilter _filter;
    private EcsPool<T1> _p1;
    private EcsPool<T2> _p2;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<T1>().End();
        _p1 = world.GetPool<T1>();
        _p2 = world.GetPool<T2>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter!)
            method(ref _p1!.Get(entity), ref _p2!.Get(entity));
    }
}

public unsafe class System<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method) : IEcsInitSystem, IEcsRunSystem
    where T1 : struct
    where T2 : struct
    where T3 : struct
{
    private EcsFilter _filter;
    private EcsPool<T1> _p1;
    private EcsPool<T2> _p2;
    private EcsPool<T3> _p3;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<T1>().End();
        _p1 = world.GetPool<T1>();
        _p2 = world.GetPool<T2>();
        _p3 = world.GetPool<T3>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter!)
            method(ref _p1!.Get(entity), ref _p2!.Get(entity), ref _p3!.Get(entity));
    }
}

public unsafe class System<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method)
    : IEcsInitSystem, IEcsRunSystem
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
{
    private EcsFilter _filter;
    private EcsPool<T1> _p1;
    private EcsPool<T2> _p2;
    private EcsPool<T3> _p3;
    private EcsPool<T4> _p4;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<T1>().End();
        _p1 = world.GetPool<T1>();
        _p2 = world.GetPool<T2>();
        _p3 = world.GetPool<T3>();
        _p4 = world.GetPool<T4>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter!)
            method(ref _p1!.Get(entity), ref _p2!.Get(entity), ref _p3!.Get(entity), ref _p4!.Get(entity));
    }
}