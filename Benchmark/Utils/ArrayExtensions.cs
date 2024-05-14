using System;

namespace Benchmark.Utils;

public static class ArrayExtensions
{
    private static readonly Random Rnd = new(1562485251);

    public static void Shuffle<T>(this T[] array)
    {
        var n = array.Length;
        while (n > 1)
        {
            var k = Rnd.Next(n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}