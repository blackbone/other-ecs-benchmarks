
using Benchmark.AppCoreKostyl;
using Benchmark.Generated;

var bench = new SystemWith3Components_DefaultECSContext();

bench.EntityCount = 1_000;
bench.Padding = 10;
bench.Iterations = 10;

bench.Setup();
bench.Run();
bench.Cleanup();