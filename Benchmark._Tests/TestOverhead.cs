using System;
using System.Collections.Generic;
using System.Linq;
using Helper = Bentchmark.Tests.Helper;

namespace Benchmark.Tests;

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
            if (!benchmarkType.IsAssignableTo(typeof(IBenchmark))) {
                continue;
            }

            foreach (var injection in Helper.GetInjections(benchmarkType)) {
                var benchmark = (IBenchmark)Activator.CreateInstance(benchmarkType);
                injection(benchmark);
                yield return benchmark;
            }
        }
    }
}
