using Benchmark._Context;

namespace Benchmark;

public interface IBenchmark<T> : IBenchmark where T : IBenchmarkContext
{
    public T Context { get; set; }
}