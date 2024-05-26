using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.RemoveComponent;

public abstract class RemoveComponentBase<T> : EntitiesBenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    [Params(true, false)] public bool Random { get; set; }
}