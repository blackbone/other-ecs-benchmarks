using Massive;

namespace Benchmark.MassiveECS;

public struct PointerInvocationEntityAction<T1> : IEntityAction<T1>
{
	public unsafe delegate*<ref T1, void> Method;

	public unsafe bool Apply(int id, ref T1 a)
	{
		Method(ref a);
		return true;
	}
}

public struct PointerInvocationEntityAction<T1, T2> : IEntityAction<T1, T2>
{
	public unsafe delegate*<ref T1, ref T2, void> Method;

	public unsafe bool Apply(int id, ref T1 a, ref T2 b)
	{
		Method(ref a, ref b);
		return true;
	}
}

public struct PointerInvocationEntityAction<T1, T2, T3> : IEntityAction<T1, T2, T3>
{
	public unsafe delegate*<ref T1, ref T2, ref T3, void> Method;

	public unsafe bool Apply(int id, ref T1 a, ref T2 b, ref T3 c)
	{
		Method(ref a, ref b, ref c);
		return true;
	}
}

public struct PointerInvocationEntityAction<T1, T2, T3, T4> : IEntityAction<T1, T2, T3, T4>
{
	public unsafe delegate*<ref T1, ref T2, ref T3, ref T4, void> Method;

	public unsafe bool Apply(int id, ref T1 a, ref T2 b, ref T3 c, ref T4 d)
	{
		Method(ref a, ref b, ref c, ref d);
		return true;
	}
}
