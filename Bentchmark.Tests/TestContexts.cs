using Benchmark;
using Benchmark._Context;

namespace Bentchmark.Tests;

[TestFixture]
public partial class TestContexts
{
    private BenchmarkContextBase? _context;
    
    [TearDown]
    public void Cleanup()
    {
        _context?.Cleanup();
        _context?.Dispose();
        _context = null;
    }
    
    private static IEnumerable<BenchmarkContextBase?> GetContexts()
    {
        foreach (var contextType in Helper.GetContextTypes())
        {
            if (Activator.CreateInstance(contextType) is not BenchmarkContextBase benchmark)
            {
                Assert.Fail($"Context of type {contextType.FullName} is not {nameof(BenchmarkContextBase)} !!!");
                continue;
            }

            yield return benchmark;
        }
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void Empty<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        context.Setup(1);
        context.FinishSetup();
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities(set);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        
        context.Lock();
        context.DeleteEntities(set);
        context.Commit();
        Assert.That(context.EntityCount, Is.EqualTo(0));
        
        _context = context; // set to variable so TearDown will hook it and clean
    }

    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void With1Component<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        
        context.Setup(1);
        context.Warmup<Component1>(0);
        context.FinishSetup();
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities(set, 0, new Component1 { Value = 1 });
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        
        Component1 c1 = default;
        context.GetSingle(set.GetValue(0), 0, ref c1);
        Assert.That(c1.Value, Is.EqualTo(1));
        
        context.Lock();
        context.RemoveComponent<Component1>(set, 0);
        context.Commit();

        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));

        if (!context.DeletesEntityOnLastComponentDeletion)
        {
            Assert.That(context.EntityCount, Is.EqualTo(1));
            context.Lock();
            context.DeleteEntities(set);
            context.Commit();
        }
        
        Assert.That(context.EntityCount, Is.EqualTo(0));
        
        _context = context; // set to variable so TearDown will hook it and clean
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void With2Components<T>(T context) where T : BenchmarkContextBase, new()
    {
        Assert.NotNull(context);
        
        context.Setup(1);
        context.Warmup<Component1>(0);
        context.Warmup<Component2>(1);
        context.Warmup<Component1, Component2>(2);
        context.FinishSetup();
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities(set, 2, new Component1 { Value = 1 }, new Component2 { Value = 1 });
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(2), Is.EqualTo(1));
        
        Component1 c1 = default;
        Component2 c2 = default;
        context.GetSingle(set.GetValue(0), 2, ref c1, ref c2);
        Assert.That(c1.Value, Is.EqualTo(1));
        Assert.That(c2.Value, Is.EqualTo(1));
        
        context.Lock();
        context.RemoveComponent<Component1>(set, 0);
        context.Commit();

        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(2), Is.EqualTo(0));
        
        context.Lock();
        context.RemoveComponent<Component2>(set, 1);
        context.Commit();

        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2>(2), Is.EqualTo(0));
        
        if (!context.DeletesEntityOnLastComponentDeletion)
        {
            Assert.That(context.EntityCount, Is.EqualTo(1));
            context.Lock();
            context.DeleteEntities(set);
            context.Commit();
        }
        
        Assert.That(context.EntityCount, Is.EqualTo(0));
        
        _context = context; // set to variable so TearDown will hook it and clean
    }

    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void With3Components<T>(T context) where T : BenchmarkContextBase, new()
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
        context.FinishSetup();
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities(set, 6, new Component1 { Value = 1 }, new Component2 { Value = 1 }, new Component3 { Value = 1 });
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(3), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component3>(4), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3, Component1>(5), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component3>(6), Is.EqualTo(1));
        
        Component1 c1 = default;
        Component2 c2 = default;
        Component3 c3 = default;
        context.GetSingle(set.GetValue(0), 6, ref c1, ref c2, ref c3);
        Assert.That(c1.Value, Is.EqualTo(1));
        Assert.That(c2.Value, Is.EqualTo(1));
        Assert.That(c3.Value, Is.EqualTo(1));
        
        context.Lock();
        context.RemoveComponent<Component1>(set, 0);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(3), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3>(4), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3, Component1>(5), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3>(6), Is.EqualTo(0));
        
        context.Lock();
        context.RemoveComponent<Component2>(set, 1);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(3), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3>(4), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3, Component1>(5), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3>(6), Is.EqualTo(0));
        
        context.Lock();
        context.RemoveComponent<Component3>(set, 2);
        context.Commit();
        
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2>(3), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3>(4), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3, Component1>(5), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3>(6), Is.EqualTo(0));
        
        if (!context.DeletesEntityOnLastComponentDeletion)
        {
            Assert.That(context.EntityCount, Is.EqualTo(1));
            context.Lock();
            context.DeleteEntities(set);
            context.Commit();
        }
        
        Assert.That(context.EntityCount, Is.EqualTo(0));
        
        _context = context; // set to variable so TearDown will hook it and clean
    }
    
    [Test]
    [TestCaseSource(nameof(GetContexts))]
    public void With4Components<T>(T context) where T : BenchmarkContextBase, new()
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
        context.FinishSetup();
        
        context.Lock();
        var set = context.PrepareSet(1);
        context.CreateEntities(set, 14, new Component1 { Value = 1 }, new Component2 { Value = 1 }, new Component3 { Value = 1 }, new Component4 { Value = 1 });
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
        
        Component1 c1 = default;
        Component2 c2 = default;
        Component3 c3 = default;
        Component4 c4 = default;
        context.GetSingle(set.GetValue(0), 14, ref c1, ref c2, ref c3, ref c4);
        Assert.That(c1.Value, Is.EqualTo(1));
        Assert.That(c2.Value, Is.EqualTo(1));
        Assert.That(c3.Value, Is.EqualTo(1));
        Assert.That(c4.Value, Is.EqualTo(1));
        
        context.Lock();
        context.RemoveComponent<Component1>(set, 0);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(1));
        Assert.That(context.CountWith<Component4>(3), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(4), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component3>(5), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component4>(6), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3>(7), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component4>(8), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3, Component4>(9), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component3>(10), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component4>(11), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component3, Component4>(12), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3, Component4>(13), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component3, Component4>(14), Is.EqualTo(0));
        
        context.Lock();
        context.RemoveComponent<Component2>(set, 1);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(1));
        Assert.That(context.CountWith<Component4>(3), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(4), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component3>(5), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component4>(6), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3>(7), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component4>(8), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3, Component4>(9), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component3>(10), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component4>(11), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component3, Component4>(12), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3, Component4>(13), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3, Component4>(14), Is.EqualTo(0));
        
        context.Lock();
        context.RemoveComponent<Component3>(set, 2);
        context.Commit();
        
        Assert.That(context.EntityCount, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(0));
        Assert.That(context.CountWith<Component4>(3), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(4), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component3>(5), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component4>(6), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3>(7), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component4>(8), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3, Component4>(9), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3>(10), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component4>(11), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component3, Component4>(12), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3, Component4>(13), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3, Component4>(14), Is.EqualTo(0));
        
        context.Lock();
        context.RemoveComponent<Component4>(set, 3);
        context.Commit();
        
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(0));
        Assert.That(context.CountWith<Component4>(3), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2>(4), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component3>(5), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component4>(6), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3>(7), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component4>(8), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3, Component4>(9), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3>(10), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component4>(11), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component3, Component4>(12), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3, Component4>(13), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3, Component4>(14), Is.EqualTo(0));
        
        if (!context.DeletesEntityOnLastComponentDeletion)
        {
            Assert.That(context.EntityCount, Is.EqualTo(1));
            context.Lock();
            context.DeleteEntities(set);
            context.Commit();
        }
        
        Assert.That(context.EntityCount, Is.EqualTo(0));
        
        _context = context; // set to variable so TearDown will hook it and clean
    }
}