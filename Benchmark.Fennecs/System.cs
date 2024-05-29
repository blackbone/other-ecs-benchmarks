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
    private readonly RefAction<T1> _del;
    private readonly delegate*<ref T1, void> _method;
    private readonly Query<T1> _query;

    public System(delegate*<ref T1, void> method, Query<T1> query)
    {
        _del = Invoke;
        _method = method;
        _query = query;
    }

    public void Run(float delta)
    {
        _query.For(_del);
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
    private readonly RefAction<T1, T2> _del;
    private readonly delegate*<ref T1, ref T2, void> _method;
    private readonly Query<T1, T2> _query;

    public System(delegate*<ref T1, ref T2, void> method, Query<T1, T2> query)
    {
        _del = Invoke;
        _method = method;
        _query = query;
    }

    public void Run(float delta)
    {
        _query.For(_del);
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
    private readonly RefAction<T1, T2, T3> _del;
    private readonly delegate*<ref T1, ref T2, ref T3, void> _method;
    private readonly Query<T1, T2, T3> _query;

    public System(delegate*<ref T1, ref T2, ref T3, void> method, Query<T1, T2, T3> query)
    {
        _del = Invoke;
        _method = method;
        _query = query;
    }

    public void Run(float delta)
    {
        _query.For(_del);
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
    private readonly RefAction<T1, T2, T3, T4> _del;
    private readonly delegate*<ref T1, ref T2, ref T3, ref T4, void> _method;
    private readonly Query<T1, T2, T3, T4> _query;

    public System(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, Query<T1, T2, T3, T4> query)
    {
        _del = Invoke;
        _method = method;
        _query = query;
    }

    public void Run(float delta)
    {
        _query.For(_del);
    }

    private void Invoke(ref T1 c0, ref T2 c1, ref T3 c2, ref T4 c3)
    {
        _method(ref c0, ref c1, ref c2, ref c3);
    }
}