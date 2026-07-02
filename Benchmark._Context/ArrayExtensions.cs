using System;
using System.Runtime.InteropServices;

namespace Benchmark;

public static class ArrayExtensions
{
    private const int Seed = 2052513957;

    public static Random Rnd = new(Seed);

    public static void ResetRandom()
    {
        Rnd = new Random(Seed);
    }

    public static void Shuffle<T>(this T[] array)
    {
        var n = array.Length;
        while (n > 1)
        {
            var k = Rnd.Next(n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }

    public static void Shuffle(this Array array)
    {
        var n = array.Length;
        while (n > 1)
        {
            var k = Rnd.Next(n--);
            var tmp = array.GetValue(n);
            array.SetValue(array.GetValue(k), n);
            array.SetValue(tmp, k);
        }
    }
}
