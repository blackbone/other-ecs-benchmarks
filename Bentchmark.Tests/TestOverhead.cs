using Benchmark;

namespace Bentchmark.Tests;

public class TestOverhead {
    [Test]
    [TestCaseSource(nameof(GetBenchmarks))]
    public void _<T>(T benchmark) where T : IBenchmark, new() {
        Assert.NotNull(benchmark);

        benchmark.GlobalSetup();

        // because of repetative logic we need to check bench will clear and reuse correctly
        var i = 3;
        while (i-- > 0) {
            benchmark.IterationSetup();
            benchmark.IterationCleanup();
        }

        benchmark.GlobalCleanup();
    }

    public static IEnumerable<IBenchmark> GetBenchmarks() {
        foreach (var benchmarkType in BenchMap.Runs.Values.SelectMany(v => v)) {
            if (Activator.CreateInstance(benchmarkType) is not IBenchmark benchmark) {
                yield return null;
                continue;
            }

            Helper.InjectParameters(benchmark);
            yield return benchmark;
        }
    }
}