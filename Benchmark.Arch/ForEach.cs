using System.Runtime.CompilerServices;
using Arch.Core;

namespace Benchmark.Arch;

public struct ForEach1<T1> : IForEach<T1>
{
    private readonly unsafe delegate*<ref T1, void> _method;

    public unsafe ForEach1(delegate*<ref T1, void> method) => _method = method;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Update(ref T1 t0) => _method(ref t0);
}

public struct ForEach2<T1, T2> : IForEach<T1, T2>
{
    private readonly unsafe delegate*<ref T1, ref T2, void> _method;

    public unsafe ForEach2(delegate*<ref T1, ref T2, void> method) => _method = method;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Update(ref T1 t1, ref T2 t2) => _method(ref t1, ref t2);
}

public struct ForEach3<T1, T2, T3> : IForEach<T1, T2, T3>
{
    private readonly unsafe delegate*<ref T1, ref T2, ref T3, void> _method;

    public unsafe ForEach3(delegate*<ref T1, ref T2, ref T3, void> method) => _method = method;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Update(ref T1 t1, ref T2 t2, ref T3 t3) => _method(ref t1, ref t2, ref t3);
}

public struct ForEach4<T1, T2, T3, T4> : IForEach<T1, T2, T3, T4>
{
    private readonly unsafe delegate*<ref T1, ref T2, ref T3, ref T4, void> _method;

    public unsafe ForEach4(delegate*<ref T1, ref T2, ref T3, ref T4, void> method) => _method = method;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Update(ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4) => _method(ref t1, ref t2, ref t3, ref t4);
}