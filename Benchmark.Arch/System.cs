using Arch.System;

namespace Benchmark.Arch;

public unsafe partial class System<T>(delegate*<ref T, void> method)
{
    [Query]
    public void ForEach(ref T t) => method(ref t);
}

public unsafe partial class System<T1, T2>(delegate*<ref T1, ref T2, void> method)
{
    [Query]
    public void ForEach(ref T1 t1, ref T2 t2) => method(ref t1, ref t2);
}

public unsafe partial class System<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method)
{
    [Query]
    public void ForEach(ref T1 t1, ref T2 t2, ref T3 t3) => method(ref t1, ref t2, ref t3);
}

public unsafe partial class System<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method)
{
    [Query]
    public void ForEach(ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4) => method(ref t1, ref t2, ref t3, ref t4);
}