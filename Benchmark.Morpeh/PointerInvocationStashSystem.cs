using Scellecs.Morpeh;

namespace Benchmark.Morpeh;

public class PointerInvocationStashSystem<T1> : ISystem
    where T1 : struct, IComponent
{
    private readonly unsafe delegate*<ref T1, void> _method;
    private Filter _filter;
    private Stash<T1> _stash1;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public unsafe PointerInvocationStashSystem(delegate*<ref T1, void> method)
    {
        _method = method;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public World World { get; set; }

    public void OnAwake()
    {
        _stash1 = World.GetStash<T1>();
        _filter = World.Filter.With<T1>().Build();
    }

    public void Dispose()
    {
    }

    public unsafe void OnUpdate(float delta)
    {
        foreach (var entity in _filter)
            _method(ref _stash1.Get(entity));
    }
}

public class PointerInvocationStashSystem<T1, T2> : ISystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
{
    private readonly unsafe delegate*<ref T1, ref T2, void> _method;
    private Filter _filter;
    private Stash<T1> _stash1;
    private Stash<T2> _stash2;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public unsafe PointerInvocationStashSystem(delegate*<ref T1, ref T2, void> method)
    {
        _method = method;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public World World { get; set; }

    public void OnAwake()
    {
        _stash1 = World.GetStash<T1>();
        _stash2 = World.GetStash<T2>();
        _filter = World.Filter.With<T1>().With<T2>().Build();
    }

    public void Dispose()
    {
    }

    public unsafe void OnUpdate(float delta)
    {
        foreach (var entity in _filter)
            _method(ref _stash1.Get(entity), ref _stash2.Get(entity));
    }
}

public class PointerInvocationStashSystem<T1, T2, T3> : ISystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
{
    private readonly unsafe delegate*<ref T1, ref T2, ref T3, void> _method;
    private Filter _filter;
    private Stash<T1> _stash1;
    private Stash<T2> _stash2;
    private Stash<T3> _stash3;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public unsafe PointerInvocationStashSystem(delegate*<ref T1, ref T2, ref T3, void> method)
    {
        _method = method;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public World World { get; set; }

    public unsafe void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
            _method(ref _stash1.Get(entity), ref _stash2.Get(entity), ref _stash3.Get(entity));
    }

    public void Dispose()
    {
    }

    public void OnAwake()
    {
        _stash1 = World.GetStash<T1>();
        _stash2 = World.GetStash<T2>();
        _stash3 = World.GetStash<T3>();
        _filter = World.Filter.With<T1>().With<T2>().With<T3>().Build();
    }
}

public class PointerInvocationStashSystem<T1, T2, T3, T4> : ISystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
    where T4 : struct, IComponent
{
    private readonly unsafe delegate*<ref T1, ref T2, ref T3, ref T4, void> _method;
    private Filter _filter;
    private Stash<T1> _stash1;
    private Stash<T2> _stash2;
    private Stash<T3> _stash3;
    private Stash<T4> _stash4;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public unsafe PointerInvocationStashSystem(delegate*<ref T1, ref T2, ref T3, ref T4, void> method)
    {
        _method = method;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public World World { get; set; }

    public void OnAwake()
    {
        _stash1 = World.GetStash<T1>();
        _stash2 = World.GetStash<T2>();
        _stash3 = World.GetStash<T3>();
        _stash4 = World.GetStash<T4>();
        _filter = World.Filter.With<T1>().With<T2>().With<T3>().With<T4>().Build();
    }

    public void Dispose()
    {
    }

    public unsafe void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
            _method(ref _stash1.Get(entity), ref _stash2.Get(entity), ref _stash3.Get(entity), ref _stash4.Get(entity));
    }
}
