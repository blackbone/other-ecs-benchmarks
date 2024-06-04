
using Benchmark.AppCoreKostyl;

var bench = new SystemWith1Component_Sister();

bench.EntityCount = 100_000;
bench.Padding = 10;
bench.Iterations = 10;

bench.Setup();
bench.Run();
bench.Cleanup();