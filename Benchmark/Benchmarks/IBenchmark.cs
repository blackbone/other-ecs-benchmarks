namespace Benchmark;

public interface IBenchmark
{
    public int EntityCount { get; set; }
    public void GlobalSetup() { }
    public void IterationSetup() { }
    public void Run();
    public void IterationCleanup() { }
    public void GlobalCleanup() { }
}