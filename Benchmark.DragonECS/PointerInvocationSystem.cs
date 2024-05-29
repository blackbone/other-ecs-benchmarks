using DCFApixels.DragonECS;

namespace Benchmark.DragonECS;

public class PointerInvocationSystem<T1> : IEcsRun
    where T1 : struct, IEcsComponent
{
    private readonly unsafe delegate*<ref T1, void> _method;
    private readonly EcsWorld? _world;

    public unsafe PointerInvocationSystem(EcsWorld world, delegate*<ref T1, void> method)
    {
        _world = world;
        _method = method;
    }

    public unsafe void Run()
    {
        foreach (var e in _world!.Where(out Aspect<T1> aspect))
            _method(ref aspect.Components1!.Get(e));
    }
}

public class PointerInvocationSystem<T1, T2> : IEcsRun
    where T1 : struct, IEcsComponent
    where T2 : struct, IEcsComponent
{
    private readonly unsafe delegate*<ref T1, ref T2, void> _method;
    private readonly EcsWorld? _world;

    public unsafe PointerInvocationSystem(EcsWorld world, delegate*<ref T1, ref T2, void> method)
    {
        _world = world;
        _method = method;
    }

    public unsafe void Run()
    {
        foreach (var e in _world!.Where(out Aspect<T1, T2> aspect))
            _method(ref aspect.Components1!.Get(e), ref aspect.Components2!.Get(e));
    }
}

public class PointerInvocationSystem<T1, T2, T3> : IEcsRun
    where T1 : struct, IEcsComponent
    where T2 : struct, IEcsComponent
    where T3 : struct, IEcsComponent
{
    private readonly unsafe delegate*<ref T1, ref T2, ref T3, void> _method;
    private readonly EcsWorld? _world;

    public unsafe PointerInvocationSystem(EcsWorld world, delegate*<ref T1, ref T2, ref T3, void> method)
    {
        _world = world;
        _method = method;
    }

    public unsafe void Run()
    {
        foreach (var e in _world!.Where(out Aspect<T1, T2, T3> aspect))
            _method(ref aspect.Components1!.Get(e), ref aspect.Components2!.Get(e), ref aspect.Components3!.Get(e));
    }
}

public class PointerInvocationSystem<T1, T2, T3, T4> : IEcsRun
    where T1 : struct, IEcsComponent
    where T2 : struct, IEcsComponent
    where T3 : struct, IEcsComponent
    where T4 : struct, IEcsComponent
{
    private readonly unsafe delegate*<ref T1, ref T2, ref T3, ref T4, void> _method;
    private readonly EcsWorld? _world;

    public unsafe PointerInvocationSystem(EcsWorld world, delegate*<ref T1, ref T2, ref T3, ref T4, void> method)
    {
        _world = world;
        _method = method;
    }

    public unsafe void Run()
    {
        foreach (var e in _world!.Where(out Aspect<T1, T2, T3, T4> aspect))
            _method(ref aspect.Components1!.Get(e), ref aspect.Components2!.Get(e), ref aspect.Components3!.Get(e),
                ref aspect.Components4!.Get(e));
    }
}