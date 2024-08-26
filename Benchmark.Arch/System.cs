using System.Runtime.CompilerServices;
using Arch.Core;

namespace Benchmark.Arch;

public unsafe class System<T>(delegate*<ref T, void> method)
{
    private readonly struct ForEach(delegate*<ref T, void> method) : IForEach<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(ref T t0) => method(ref t0);
    }

    private ForEach _forEach = new(method);
    private readonly QueryDescription _query = new([typeof(T)]);

    public void ForEachQuery(World world) => world.InlineParallelQuery<ForEach, T>(_query, ref _forEach);
}

public unsafe class System<T1, T2>(delegate*<ref T1, ref T2, void> method)
{
    private readonly struct ForEach(delegate*<ref T1, ref T2, void> method) : IForEach<T1, T2>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(ref T1 t1, ref T2 t2) => method(ref t1, ref t2);
    }

    private ForEach _forEach = new(method);
    private readonly QueryDescription _query = new([typeof(T1), typeof(T2)]);
    
    public void ForEachQuery(World world) => world.InlineParallelQuery<ForEach, T1, T2>(_query, ref _forEach);
}

public unsafe class System<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method)
{
    private readonly struct ForEach(delegate*<ref T1, ref T2, ref T3, void> method) : IForEach<T1, T2, T3>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(ref T1 t1, ref T2 t2, ref T3 t3) => method(ref t1, ref t2, ref t3);
    }

    private ForEach _forEach = new(method);

    private readonly QueryDescription _query = new([typeof(T1), typeof(T2), typeof(T3)]);

    public void ForEachQuery(World world) => world.InlineParallelQuery<ForEach, T1, T2, T3>(_query, ref _forEach);
}

public unsafe class System<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method)
{
    private readonly struct ForEach(delegate*<ref T1, ref T2, ref T3, ref T4, void> method) : IForEach<T1, T2, T3, T4>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4) => method(ref t1, ref t2, ref t3, ref t4);
    }

    private ForEach _forEach = new(method);

    private readonly QueryDescription _query = new([typeof(T1), typeof(T2), typeof(T3), typeof(T4)]);
    
    public void ForEachQuery(World world) => world.InlineParallelQuery<ForEach, T1, T2, T3, T4>(_query, ref _forEach);
}