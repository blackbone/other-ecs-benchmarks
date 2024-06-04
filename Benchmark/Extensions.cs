using System.Linq;

namespace Benchmark;

public static class ArrayExtensions
{
    public static T[] With<T>(this T[] array, T value)
    {
        var list = array.ToList();
        list.Add(value);
        return list.ToArray();
    }
}