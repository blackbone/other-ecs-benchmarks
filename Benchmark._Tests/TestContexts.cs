using System.Collections.Generic;
using System.Linq;
using Benchmark;
using Benchmark.Context;

namespace Benchmark.Tests;

// [TestFixture]
public class TestContexts {
    [TearDown]
    public void Cleanup() {
        _context.Cleanup();
        _context.Dispose();
        _context = null;
    }

    private IBenchmarkContext _context;

    private static IEnumerable<IBenchmarkContext> GetContexts() {
        foreach (var contextType in BenchMap.Contexts.Keys) {
            var ctor = contextType.GetConstructors().First();
            yield return ctor.Invoke([]) as IBenchmarkContext;
        }
    }

    // [Test]
    // [TestCaseSource(nameof(GetContexts))]
    public void Empty<T, TE>(T context) where T : IBenchmarkContext {
        var ctx = context as IBenchmarkContext<TE>;

        Assert.NotNull(ctx);
        ctx.Setup();
        ctx.FinishSetup();

        var set = ctx.PrepareSet(1);
        ctx.CreateEntities(set);

        Assert.That(ctx.NumberOfLivingEntities, Is.EqualTo(1));

        ctx.DeleteEntities(set);

        Assert.That(ctx.NumberOfLivingEntities, Is.EqualTo(0));

        _context = ctx; // set to variable so TearDown will hook it and clean
    }

    // [Test]
    // [TestCaseSource(nameof(GetContexts))]
    public void With1Component<T, TE>(T context) where T : IBenchmarkContext<TE> {
        Assert.NotNull(context);

        context.Setup();
        context.Warmup<Component1>(0);
        unsafe {
            context.AddSystem<Component1>(&Update, 0);
        }

        context.FinishSetup();

        var set = context.PrepareSet(1);
        context.CreateEntities(set, 0, new Component1 { Value = 1 });

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));

        Component1 c1 = default;
        context.GetSingle(set[0], 0, ref c1);
        Assert.That(c1.Value, Is.EqualTo(1));

        for (var i = 0; i < 1000; i++) {
            context.Tick(0.1f);
        }

        context.GetSingle(set[0], 0, ref c1);
        Assert.That(c1.Value, Is.EqualTo(1001));

        context.RemoveComponent<Component1>(set, 0);

        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));

        if (!context.DeletesEntityOnLastComponentDeletion) {
            Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
            context.DeleteEntities(set);
        }

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(0));

        _context = context; // set to variable so TearDown will hook it and clean
        return;

        static void Update(ref Component1 c1) {
            c1.Value++;
        }
    }

    // [Test]
    // [TestCaseSource(nameof(GetContexts))]
    public void With2Components<T, TE>(T context) where T : IBenchmarkContext<TE> {
        Assert.NotNull(context);

        context.Setup();
        context.Warmup<Component1>(0);
        context.Warmup<Component2>(1);
        context.Warmup<Component1, Component2>(2);
        unsafe {
            context.AddSystem<Component1, Component2>(&Update, 2);
        }

        context.FinishSetup();

        var set = context.PrepareSet(1);
        context.CreateEntities(set, 2, new Component1 { Value = 1 }, new Component2 { Value = 1 });

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2>(2), Is.EqualTo(1));

        Component1 c1 = default;
        Component2 c2 = default;
        context.GetSingle(set[0], 2, ref c1, ref c2);
        Assert.That(c1.Value, Is.EqualTo(1));
        Assert.That(c2.Value, Is.EqualTo(1));

        for (var i = 0; i < 1000; i++) {
            context.Tick(0.1f);
        }

        context.GetSingle(set[0], 2, ref c1, ref c2);
        Assert.That(c1.Value, Is.EqualTo(1001));
        Assert.That(c2.Value, Is.EqualTo(1));

        context.RemoveComponent<Component2>(set, 1);

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2>(2), Is.EqualTo(0));

        context.RemoveComponent<Component1>(set, 0);

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(context.DeletesEntityOnLastComponentDeletion ? 0 : 1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2>(2), Is.EqualTo(0));

        if (!context.DeletesEntityOnLastComponentDeletion) {
            Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));

            context.DeleteEntities(set);
        }

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(0));

        _context = context; // set to variable so TearDown will hook it and clean
        return;

        static void Update(ref Component1 c1, ref Component2 c2) {
            c1.Value += c2.Value;
        }
    }

    // [Test]
    // [TestCaseSource(nameof(GetContexts))]
    public void With3Components<T, TE>(T context) where T : IBenchmarkContext<TE> {
        Assert.NotNull(context);

        context.Setup();
        context.Warmup<Component1>(0);
        context.Warmup<Component2>(1);
        context.Warmup<Component3>(2);
        context.Warmup<Component1, Component2>(3);
        context.Warmup<Component2, Component3>(4);
        context.Warmup<Component3, Component1>(5);
        context.Warmup<Component1, Component2, Component3>(6);
        unsafe {
            context.AddSystem<Component1, Component2, Component3>(&Update, 6);
        }

        context.FinishSetup();

        var set = context.PrepareSet(1);
        context.CreateEntities(set, 6, new Component1 { Value = 1 }, new Component2 { Value = 1 },
            new Component3 { Value = 1 });

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
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
        context.GetSingle(set[0], 6, ref c1, ref c2, ref c3);
        Assert.That(c1.Value, Is.EqualTo(1));
        Assert.That(c2.Value, Is.EqualTo(1));
        Assert.That(c3.Value, Is.EqualTo(1));

        for (var i = 0; i < 1000; i++) {
            context.Tick(0.1f);
        }

        context.RemoveComponent<Component3>(set, 2);

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2>(3), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component3>(4), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3, Component1>(5), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3>(6), Is.EqualTo(0));

        context.RemoveComponent<Component2>(set, 1);

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2>(3), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3>(4), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3, Component1>(5), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3>(6), Is.EqualTo(0));

        context.RemoveComponent<Component1>(set, 0);

        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2>(3), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3>(4), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3, Component1>(5), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3>(6), Is.EqualTo(0));

        if (!context.DeletesEntityOnLastComponentDeletion) {
            Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));

            context.DeleteEntities(set);
        }

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(0));

        _context = context; // set to variable so TearDown will hook it and clean
        return;

        static void Update(ref Component1 c1, ref Component2 c2, ref Component3 c3) {
            c1.Value += c2.Value + c3.Value;
        }
    }

    // [Test]
    // [TestCaseSource(nameof(GetContexts))]
    public void With4Components<T, TE>(T context) where T : IBenchmarkContext<TE> {
        Assert.NotNull(context);

        context.Setup();
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
        unsafe {
            context.AddSystem<Component1, Component2, Component3, Component4>(&Update, 14);
        }

        context.FinishSetup();

        var set = context.PrepareSet(1);
        context.CreateEntities(set, 14, new Component1 { Value = 1 }, new Component2 { Value = 1 },
            new Component3 { Value = 1 }, new Component4 { Value = 1 });

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
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
        context.GetSingle(set[0], 14, ref c1, ref c2, ref c3, ref c4);
        Assert.That(c1.Value, Is.EqualTo(1));
        Assert.That(c2.Value, Is.EqualTo(1));
        Assert.That(c3.Value, Is.EqualTo(1));
        Assert.That(c4.Value, Is.EqualTo(1));

        for (var i = 0; i < 1001; i++) {
            context.Tick(0.1f);
        }

        context.RemoveComponent<Component4>(set, 3);

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(1));
        Assert.That(context.CountWith<Component4>(3), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2>(4), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component3>(5), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component4>(6), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3>(7), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2, Component4>(8), Is.EqualTo(0));
        Assert.That(context.CountWith<Component3, Component4>(9), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3>(10), Is.EqualTo(1));
        Assert.That(context.CountWith<Component1, Component2, Component4>(11), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component3, Component4>(12), Is.EqualTo(0));
        Assert.That(context.CountWith<Component2, Component3, Component4>(13), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2, Component3, Component4>(14), Is.EqualTo(0));

        context.RemoveComponent<Component3>(set, 2);

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
        Assert.That(context.CountWith<Component2>(1), Is.EqualTo(1));
        Assert.That(context.CountWith<Component3>(2), Is.EqualTo(0));
        Assert.That(context.CountWith<Component4>(3), Is.EqualTo(0));
        Assert.That(context.CountWith<Component1, Component2>(4), Is.EqualTo(1));
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

        context.RemoveComponent<Component2>(set, 1);

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));
        Assert.That(context.CountWith<Component1>(0), Is.EqualTo(1));
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

        context.RemoveComponent<Component1>(set, 0);

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

        if (!context.DeletesEntityOnLastComponentDeletion) {
            Assert.That(context.NumberOfLivingEntities, Is.EqualTo(1));

            context.DeleteEntities(set);
        }

        Assert.That(context.NumberOfLivingEntities, Is.EqualTo(0));

        _context = context; // set to variable so TearDown will hook it and clean
        return;

        static void Update(ref Component1 c1, ref Component2 c2, ref Component3 c3, ref Component4 c4) {
            c1.Value += c2.Value + c3.Value + c4.Value;
        }
    }
}
