using Benchmark._Context;

namespace Benchmark.Benchmarks;

public abstract class AddComponentBase<T> : BenchmarkBase<T> where T : BenchmarkContextBase, new()
{
    protected int[] entityIds;

    protected sealed override void OnSetup()
    {
        entityIds = new int[EntityCount];
        for (var i = 0; i < EntityCount; i++)
            entityIds[i] = Context.CreateEntity();
    }

    protected sealed override void OnCleanup()
    {
        Context.RemoveComponent<Component1, Component2, Component3, Component4>(entityIds);
    }
}