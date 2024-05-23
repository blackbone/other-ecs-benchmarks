using Benchmark;

namespace Bentchmark.Tests;

public class TestBenchmarks
{
    [Test]
    [TestCaseSource(nameof(GetBenchmarks))]
    public void CheckBenchmark<T>(T benchmark) where T : BenchmarkBase, new()
    {
        Assert.NotNull(benchmark);
        
        benchmark.Setup();
        benchmark.Run();
        benchmark.Cleanup();
    }

    public static IEnumerable<BenchmarkBase?> GetBenchmarks()
    {
        foreach (var contextType in Helper.GetContextTypes())
        foreach (var baseBenchmarkType in Helper.GetBenchmarkTypes())
        {
            var benchmark = Activator.CreateInstance(baseBenchmarkType.MakeGenericType(contextType)) as BenchmarkBase;
            if (benchmark == null)
            {
                yield return null;
                continue;
            }

            Helper.InjectParameters(benchmark);
            yield return benchmark;
        }
    }

    
}