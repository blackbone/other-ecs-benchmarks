using fennecs;

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

namespace Benchmark.Fennecs;

public interface ISystem
{
    void Run(float delta);
}

public unsafe class System<T1> : ISystem
    where T1 : struct
{
    private readonly ComponentAction<T1> _del;
    private readonly delegate*<ref T1, void> _method;
    private readonly Stream<T1> _stream;

    public System(delegate*<ref T1, void> method, Stream<T1> stream)
    {
        _del = Invoke;
        _method = method;
        _stream = stream;
    }

    public void Run(float delta)
    {
        _stream.For(_del);
    }

    private void Invoke(ref T1 c0)
    {
        _method(ref c0);
    }
}

public unsafe class System<T1, T2> : ISystem
    where T1 : struct
    where T2 : struct
{
    private readonly ComponentAction<T1, T2> _del;
    private readonly delegate*<ref T1, ref T2, void> _method;
    private readonly Stream<T1, T2> _stream;

    public System(delegate*<ref T1, ref T2, void> method, Stream<T1, T2> stream)
    {
        _del = Invoke;
        _method = method;
        _stream = stream;
    }

    public void Run(float delta)
    {
        _stream.For(_del);
    }

    private void Invoke(ref T1 c0, ref T2 c1)
    {
        _method(ref c0, ref c1);
    }
}

public unsafe class System<T1, T2, T3> : ISystem
    where T1 : struct
    where T2 : struct
    where T3 : struct
{
    private readonly ComponentAction<T1, T2, T3> _del;
    private readonly delegate*<ref T1, ref T2, ref T3, void> _method;
    private readonly Stream<T1, T2, T3> _stream;

    public System(delegate*<ref T1, ref T2, ref T3, void> method, Stream<T1, T2, T3> stream)
    {
        _del = Invoke;
        _method = method;
        _stream = stream;
    }

    public void Run(float delta)
    {
        _stream.For(_del);
    }

    private void Invoke(ref T1 c0, ref T2 c1, ref T3 c2)
    {
        _method(ref c0, ref c1, ref c2);
    }
}

public unsafe class System<T1, T2, T3, T4> : ISystem
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
{
    private readonly ComponentAction<T1, T2, T3, T4> _del;
    private readonly delegate*<ref T1, ref T2, ref T3, ref T4, void> _method;
    private readonly Stream<T1, T2, T3, T4> _stream;

    public System(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, Stream<T1, T2, T3, T4> stream)
    {
        _del = Invoke;
        _method = method;
        _stream = stream;
    }

    public void Run(float delta)
    {
        _stream.For(_del);
    }

    private void Invoke(ref T1 c0, ref T2 c1, ref T3 c2, ref T4 c3)
    {
        _method(ref c0, ref c1, ref c2, ref c3);
    }
}