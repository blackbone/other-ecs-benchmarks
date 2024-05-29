using System.Reflection;
using Benchmark;
using BenchmarkDotNet.Attributes;

namespace Bentchmark.Tests;

public static class Helper
{
    public static void InjectParameters(IBenchmark benchmark)
    {
        benchmark.EntityCount = Constants.SmallEntityCount;

        var properties = benchmark.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.Name != nameof(IBenchmark.EntityCount))
            .Select(p => (p, p.GetCustomAttribute<ParamsAttribute>()))
            .Where(kv => kv.Item2 != null)
            .ToArray();
        foreach (var (property, attribute) in properties)
            property.SetValue(benchmark, attribute!.Values[0]);
    }
}