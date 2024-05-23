using Benchmark;
using Benchmark._Context;

namespace Bentchmark.Tests;

public class TestContexts
{
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void CreateEmptyEntity<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        context.Setup(1);
        
        context.Lock();
        context.CreateEntities(context.PrepareSet(1));
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        
        context.Cleanup();
        context.Dispose();
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void CreateEntityWith1Component<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        
        context.Setup(1);
        context.Warmup<Component1>(0);
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities<Component1>(set, 0);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        
        context.Cleanup();
        context.Dispose();
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void CreateEntityWith2Components<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        
        context.Setup(1);
        context.Warmup<Component1>(0);
        context.Warmup<Component2>(1);
        context.Warmup<Component1, Component2>(2);
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities<Component1, Component2>(set, 2);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(2), Is.EqualTo(1));
        
        context.Cleanup();
        context.Dispose();
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void CreateEntityWith3Components<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        
        context.Setup(1);
        context.Warmup<Component1>(0);
        context.Warmup<Component2>(1);
        context.Warmup<Component3>(2);
        context.Warmup<Component1, Component2>(3);
        context.Warmup<Component2, Component3>(4);
        context.Warmup<Component3, Component1>(5);
        context.Warmup<Component1, Component2, Component3>(6);
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities<Component1, Component2, Component3>(set, 6);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(3), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component3>(4), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3, Component1>(5), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component3>(6), Is.EqualTo(1));
        
        context.Cleanup();
        context.Dispose();
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void CreateEntityWith4Components<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        
        context.Setup(1);
        context.Warmup<Component1>(0);
        context.Warmup<Component2>(1);
        context.Warmup<Component3>(2);
        context.Warmup<Component4>(3);
        context.Warmup<Component1, Component2>(4);
        context.Warmup<Component1, Component3>(5);
        context.Warmup<Component1, Component4>(6);
        context.Warmup<Component2, Component3>(7);
        context.Warmup<Component2, Component4>(8);
        context.Warmup<Component3, Component4>(9);
        context.Warmup<Component1, Component2, Component3>(10);
        context.Warmup<Component1, Component2, Component4>(11);
        context.Warmup<Component1, Component3, Component4>(12);
        context.Warmup<Component2, Component3, Component4>(13);
        context.Warmup<Component1, Component2, Component3, Component4>(14);
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities<Component1, Component2, Component3, Component4>(set, 14);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(1));
        Assert.That(context.CountWith<Component4>(3), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(4), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component3>(5), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component4>(6), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component3>(7), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component4>(8), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3, Component4>(9), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component3>(10), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component4>(11), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component3, Component4>(12), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component3, Component4>(13), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component3, Component4>(14), Is.EqualTo(1));
        
        context.Cleanup();
        context.Dispose();
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void Add1Components<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        
        context.Setup(1);
        context.Warmup<Component1>(0);
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities(set);
        context.AddComponent<Component1>(set, 0);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        
        context.Cleanup();
        context.Dispose();
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void Add2Components<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        
        context.Setup(1);
        context.Warmup<Component1>(0);
        context.Warmup<Component2>(1);
        context.Warmup<Component1, Component2>(2);
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities(set);
        context.AddComponent<Component1, Component2>(set, 2);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(2), Is.EqualTo(1));
        
        context.Cleanup();
        context.Dispose();
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void Add3Components<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        
        context.Setup(1);
        context.Warmup<Component1>(0);
        context.Warmup<Component2>(1);
        context.Warmup<Component3>(2);
        context.Warmup<Component1, Component2>(3);
        context.Warmup<Component2, Component3>(4);
        context.Warmup<Component3, Component1>(5);
        context.Warmup<Component1, Component2, Component3>(6);
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities(set);
        context.AddComponent<Component1, Component2, Component3>(set, 6);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(3), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component3>(4), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3, Component1>(5), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component3>(6), Is.EqualTo(1));
        
        context.Cleanup();
        context.Dispose();
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void Add4Components<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        
        context.Setup(1);
        context.Warmup<Component1>(0);
        context.Warmup<Component2>(1);
        context.Warmup<Component3>(2);
        context.Warmup<Component4>(3);
        context.Warmup<Component1, Component2>(4);
        context.Warmup<Component1, Component3>(5);
        context.Warmup<Component1, Component4>(6);
        context.Warmup<Component2, Component3>(7);
        context.Warmup<Component2, Component4>(8);
        context.Warmup<Component3, Component4>(9);
        context.Warmup<Component1, Component2, Component3>(10);
        context.Warmup<Component1, Component2, Component4>(11);
        context.Warmup<Component1, Component3, Component4>(12);
        context.Warmup<Component2, Component3, Component4>(13);
        context.Warmup<Component1, Component2, Component3, Component4>(14);
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities(set);
        context.AddComponent<Component1, Component2, Component3, Component4>(set, 14);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(1));
        Assert.That(context.CountWith<Component4>(3), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(4), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component3>(5), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component4>(6), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component3>(7), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component4>(8), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3, Component4>(9), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component3>(10), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component4>(11), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component3, Component4>(12), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component3, Component4>(13), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component3, Component4>(14), Is.EqualTo(1));
        
        context.Cleanup();
        context.Dispose();
    }
    
    private static IEnumerable<BenchmarkContextBase?> GetContexts()
    {
        foreach (var contextType in Helper.GetContextTypes())
        {
            var benchmark = Activator.CreateInstance(contextType) as BenchmarkContextBase;
            if (benchmark == null)
            {
                yield return null;
                continue;
            }

            yield return benchmark;
        }
    }
}