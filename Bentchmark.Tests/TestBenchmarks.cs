using System.Reflection;
using Benchmark;
using BenchmarkDotNet.Attributes;

namespace Bentchmark.Tests;

public class TestBenchmarks
{
    [Test]
    [TestCaseSource(nameof(GetBenchmarks))]
    public void _<T>(T benchmark) where T : IBenchmark, new()
    {
        Assert.NotNull(benchmark);

        var isGlobalSetup = typeof(T).GetMethod(nameof(IBenchmark.IterationSetup))?.GetCustomAttribute(typeof(GlobalSetupAttribute)) != null;
        var isGlobalCleanup = typeof(T).GetMethod(nameof(IBenchmark.IterationCleanup))?.GetCustomAttribute(typeof(GlobalCleanupAttribute)) != null;

        if (isGlobalSetup) benchmark.IterationSetup();
        
        // because of repetative logic we need to check bench will clear and reuse correctly
        var i = 3;
        while (i-- > 0)
        {
            if (!isGlobalSetup) benchmark.IterationSetup();
            benchmark.Run();
            if (!isGlobalCleanup) benchmark.IterationCleanup();
        }
        
        if (isGlobalCleanup) benchmark.IterationCleanup();
    }

    public static IEnumerable<IBenchmark?> GetBenchmarks()
    {
        foreach (var benchmarkType in BenchMap.Runs.Values.SelectMany(v => v))
        {
            if (Activator.CreateInstance(benchmarkType) is not IBenchmark benchmark)
            {
                yield return null;
                continue;
            }

            Helper.InjectParameters(benchmark);
            yield return benchmark;
        }
    }
}