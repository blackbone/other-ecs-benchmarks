using System.Runtime.CompilerServices;
using Xeno;

namespace Benchmark.Xeno;

public static unsafe class XenoSystemHolder1<T1> where T1 : struct
{
    public static delegate*<ref T1, void> Method;
}

public static unsafe class XenoSystemHolder2<T1, T2>
    where T1 : struct
    where T2 : struct
{
    public static delegate*<ref T1, ref T2, void> Method;
}

public static unsafe class XenoSystemHolder3<T1, T2, T3>
    where T1 : struct
    where T2 : struct
    where T3 : struct
{
    public static delegate*<ref T1, ref T2, ref T3, void> Method;
}

internal sealed unsafe class XenoSystem1<T1> where T1 : struct
{
    [SystemMethod(SystemMethodType.Update, 0)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Run(ref T1 c1)
    {
        if (XenoSystemHolder1<T1>.Method != null)
            XenoSystemHolder1<T1>.Method(ref c1);
    }
}

internal sealed unsafe class XenoSystem2<T1, T2>
    where T1 : struct
    where T2 : struct
{
    [SystemMethod(SystemMethodType.Update, 0)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Run(ref T1 c1, ref T2 c2)
    {
        if (XenoSystemHolder2<T1, T2>.Method != null)
            XenoSystemHolder2<T1, T2>.Method(ref c1, ref c2);
    }
}

internal sealed unsafe class XenoSystem3<T1, T2, T3>
    where T1 : struct
    where T2 : struct
    where T3 : struct
{
    [SystemMethod(SystemMethodType.Update, 0)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Run(ref T1 c1, ref T2 c2, ref T3 c3)
    {
        if (XenoSystemHolder3<T1, T2, T3>.Method != null)
            XenoSystemHolder3<T1, T2, T3>.Method(ref c1, ref c2, ref c3);
    }
}
