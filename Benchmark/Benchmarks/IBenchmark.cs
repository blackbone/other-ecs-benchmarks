namespace Benchmark;

public interface IBenchmark
{
    public int EntityCount { get; set; }
    public void Setup();
    public void Cleanup();
    public void Run();
}