using Scellecs.Morpeh;

namespace Benchmark.Morpeh;

internal class PointerInvocationDirectSystem<T1> : ISystem
    where T1 : struct, IComponent
{
    private readonly unsafe delegate*<ref T1, void> _method;
    private Filter _filter;
        
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public unsafe PointerInvocationDirectSystem(delegate*<ref T1, void> method) => _method = method;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public World World { get; set; }
    public void OnAwake() => _filter = World.Filter.With<T1>().Build();
    public void Dispose() { }
    public unsafe void OnUpdate(float delta)
    {
        foreach (var entity in _filter)
            _method(ref entity.GetComponent<T1>());
    }
}

internal class PointerInvocationDirectSystem<T1, T2> : ISystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
{
    private readonly unsafe delegate*<ref T1, ref T2, void> _method;
    private Filter _filter;
        
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public unsafe PointerInvocationDirectSystem(delegate*<ref T1, ref T2, void> method) => _method = method;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public World World { get; set; }
    public void OnAwake() => _filter = World.Filter.With<T1>().With<T2>().Build();
    public void Dispose() { }
    public unsafe void OnUpdate(float delta)
    {
        foreach (var entity in _filter)
            _method(ref entity.GetComponent<T1>(), ref entity.GetComponent<T2>());
    }
}

internal class PointerInvocationDirectSystem<T1, T2, T3> : ISystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
{
    private readonly unsafe delegate*<ref T1, ref T2, ref T3, void> _method;
    private Filter _filter;
        
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public unsafe PointerInvocationDirectSystem(delegate*<ref T1, ref T2, ref T3, void> method) => _method = method;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public World World { get; set; }
    public void OnAwake() => _filter = World.Filter.With<T1>().With<T2>().With<T3>().Build();
    public void Dispose() { }
    public unsafe void OnUpdate(float delta)
    {
        foreach (var entity in _filter)
            _method(ref entity.GetComponent<T1>(), ref entity.GetComponent<T2>(), ref entity.GetComponent<T3>());
    }
}

internal class PointerInvocationDirectSystem<T1, T2, T3, T4> : ISystem
    where T1 : struct, IComponent
    where T2 : struct, IComponent
    where T3 : struct, IComponent
    where T4 : struct, IComponent
{
    private readonly unsafe delegate*<ref T1, ref T2, ref T3, ref T4, void> _method;
    private Filter _filter;
        
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public unsafe PointerInvocationDirectSystem(delegate*<ref T1, ref T2, ref T3, ref T4, void> method) => _method = method;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public World World { get; set; }
    public void OnAwake() => _filter = World.Filter.With<T1>().With<T2>().With<T3>().With<T4>().Build();
    public void Dispose() { }
    public unsafe void OnUpdate(float delta)
    {
        foreach (var entity in _filter)
            _method(ref entity.GetComponent<T1>(), ref entity.GetComponent<T2>(), ref entity.GetComponent<T3>(), ref entity.GetComponent<T4>());
    }
}