using Arch.Core;

namespace Benchmark.Arch;

public unsafe class System<T>(delegate*<ref T, void> method)
{
    private readonly QueryDescription _query = new()
    {
        All = [typeof(T)],
        Any = [],
        None = [],
        Exclusive = []
    };
    public void ForEachQuery(World world) => world.ParallelQuery(_query, (ref T t0) => method(ref t0));
}

public unsafe partial class System<T1, T2>(delegate*<ref T1, ref T2, void> method)
{
    private readonly QueryDescription _query = new()
    {
        All = [typeof(T1), typeof(T2)],
        Any = [],
        None = [],
        Exclusive = []
    };
    
    public void ForEachQuery(World world) => world.ParallelQuery(_query, (ref T1 t1, ref T2 t2) => method(ref t1, ref t2));
}

public unsafe partial class System<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method)
{
    private readonly QueryDescription _query = new()
    {
        All = [typeof(T1), typeof(T2), typeof(T3)],
        Any = [],
        None = [],
        Exclusive = []
    };
    
    public void ForEachQuery(World world) => world.ParallelQuery(_query, (ref T1 t1, ref T2 t2, ref T3 t3) => method(ref t1, ref t2, ref t3));
}

public unsafe class System<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method)
{    private readonly QueryDescription _query = new()
    {
        All = [typeof(T1), typeof(T2), typeof(T3), typeof(T4)],
        Any = [],
        None = [],
        Exclusive = []
    };
    
    public void ForEachQuery(World world) => world.ParallelQuery(_query, (ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4) => method(ref t1, ref t2, ref t3, ref t4));
}