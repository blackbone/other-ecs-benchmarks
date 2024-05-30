using DefaultEcs;
using DefaultEcs.System;

namespace Benchmark.DefaultECS;

public sealed partial class System<T1> : AEntitySetSystem<float>
{
    private readonly unsafe delegate*<ref T1, void> _method;

    public unsafe System(World world, delegate*<ref T1, void> method) : base(world)
        => _method = method;

    [Update]
    private unsafe void Update(ref T1 component) => _method(ref component);
}

public sealed partial class System<T1, T2> : AEntitySetSystem<float>
{
    private readonly unsafe delegate*<ref T1, ref T2, void> _method;

    public unsafe System(World world, delegate*<ref T1, ref T2, void> method) : base(world)
        => _method = method;

    [Update]
    private unsafe void Update(ref T1 c1, ref T2 c2) => _method(ref c1, ref c2);
}

public sealed partial class System<T1, T2, T3> : AEntitySetSystem<float>
{
    private readonly unsafe delegate*<ref T1, ref T2, ref T3, void> _method;

    public unsafe System(World world, delegate*<ref T1, ref T2, ref T3, void> method) : base(world)
        => _method = method;

    [Update]
    private unsafe void Update(ref T1 c1, ref T2 c2, ref T3 c3) => _method(ref c1, ref c2, ref c3);
}

public sealed partial class System<T1, T2, T3, T4> : AEntitySetSystem<float>
{
    private readonly unsafe delegate*<ref T1, ref T2, ref T3, ref T4, void> _method;

    public unsafe System(World world, delegate*<ref T1, ref T2, ref T3, ref T4, void> method) : base(world)
        => _method = method;

    [Update]
    private unsafe void Update(ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) => _method(ref c1, ref c2, ref c3, ref c4);
}