using System;
using Benchmark.Context;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(FilterMismatchSystems<T, TE>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class FilterMismatchSystems<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE> {
    private const int SystemCount = 100;
    private const float Delta = 0.1f;

    [Params(Constants.SystemEntityCount)]
    public int EntityCount { get; set; }

    public T Context { get; set; }

    private readonly TE[][] _sets = new TE[SystemCount][];
    private readonly Action[] _create = new Action[SystemCount];
    private readonly Action[] _delete = new Action[SystemCount];

    [GlobalSetup]
    public void GlobalSetup() {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();

        var portion = EntityCount / SystemCount;
        SetupSystem<Component1, Component2>(0, portion);
        SetupSystem<Component2, Component3>(1, portion);
        SetupSystem<Component3, Component4>(2, portion);
        SetupSystem<Component4, Component5>(3, portion);
        SetupSystem<Component5, Component6>(4, portion);
        SetupSystem<Component6, Component7>(5, portion);
        SetupSystem<Component7, Component8>(6, portion);
        SetupSystem<Component8, Component9>(7, portion);
        SetupSystem<Component9, Component10>(8, portion);
        SetupSystem<Component10, Component11>(9, portion);
        SetupSystem<Component11, Component12>(10, portion);
        SetupSystem<Component12, Component13>(11, portion);
        SetupSystem<Component13, Component14>(12, portion);
        SetupSystem<Component14, Component15>(13, portion);
        SetupSystem<Component15, Component16>(14, portion);
        SetupSystem<Component16, Component17>(15, portion);
        SetupSystem<Component17, Component18>(16, portion);
        SetupSystem<Component18, Component19>(17, portion);
        SetupSystem<Component19, Component20>(18, portion);
        SetupSystem<Component20, Component21>(19, portion);
        SetupSystem<Component21, Component22>(20, portion);
        SetupSystem<Component22, Component23>(21, portion);
        SetupSystem<Component23, Component24>(22, portion);
        SetupSystem<Component24, Component25>(23, portion);
        SetupSystem<Component25, Component26>(24, portion);
        SetupSystem<Component26, Component27>(25, portion);
        SetupSystem<Component27, Component28>(26, portion);
        SetupSystem<Component28, Component29>(27, portion);
        SetupSystem<Component29, Component30>(28, portion);
        SetupSystem<Component30, Component31>(29, portion);
        SetupSystem<Component31, Component32>(30, portion);
        SetupSystem<Component32, Component33>(31, portion);
        SetupSystem<Component33, Component34>(32, portion);
        SetupSystem<Component34, Component35>(33, portion);
        SetupSystem<Component35, Component36>(34, portion);
        SetupSystem<Component36, Component37>(35, portion);
        SetupSystem<Component37, Component38>(36, portion);
        SetupSystem<Component38, Component39>(37, portion);
        SetupSystem<Component39, Component40>(38, portion);
        SetupSystem<Component40, Component41>(39, portion);
        SetupSystem<Component41, Component42>(40, portion);
        SetupSystem<Component42, Component43>(41, portion);
        SetupSystem<Component43, Component44>(42, portion);
        SetupSystem<Component44, Component45>(43, portion);
        SetupSystem<Component45, Component46>(44, portion);
        SetupSystem<Component46, Component47>(45, portion);
        SetupSystem<Component47, Component48>(46, portion);
        SetupSystem<Component48, Component49>(47, portion);
        SetupSystem<Component49, Component50>(48, portion);
        SetupSystem<Component50, Component51>(49, portion);
        SetupSystem<Component51, Component52>(50, portion);
        SetupSystem<Component52, Component53>(51, portion);
        SetupSystem<Component53, Component54>(52, portion);
        SetupSystem<Component54, Component55>(53, portion);
        SetupSystem<Component55, Component56>(54, portion);
        SetupSystem<Component56, Component57>(55, portion);
        SetupSystem<Component57, Component58>(56, portion);
        SetupSystem<Component58, Component59>(57, portion);
        SetupSystem<Component59, Component60>(58, portion);
        SetupSystem<Component60, Component61>(59, portion);
        SetupSystem<Component61, Component62>(60, portion);
        SetupSystem<Component62, Component63>(61, portion);
        SetupSystem<Component63, Component64>(62, portion);
        SetupSystem<Component64, Component65>(63, portion);
        SetupSystem<Component65, Component66>(64, portion);
        SetupSystem<Component66, Component67>(65, portion);
        SetupSystem<Component67, Component68>(66, portion);
        SetupSystem<Component68, Component69>(67, portion);
        SetupSystem<Component69, Component70>(68, portion);
        SetupSystem<Component70, Component71>(69, portion);
        SetupSystem<Component71, Component72>(70, portion);
        SetupSystem<Component72, Component73>(71, portion);
        SetupSystem<Component73, Component74>(72, portion);
        SetupSystem<Component74, Component75>(73, portion);
        SetupSystem<Component75, Component76>(74, portion);
        SetupSystem<Component76, Component77>(75, portion);
        SetupSystem<Component77, Component78>(76, portion);
        SetupSystem<Component78, Component79>(77, portion);
        SetupSystem<Component79, Component80>(78, portion);
        SetupSystem<Component80, Component81>(79, portion);
        SetupSystem<Component81, Component82>(80, portion);
        SetupSystem<Component82, Component83>(81, portion);
        SetupSystem<Component83, Component84>(82, portion);
        SetupSystem<Component84, Component85>(83, portion);
        SetupSystem<Component85, Component86>(84, portion);
        SetupSystem<Component86, Component87>(85, portion);
        SetupSystem<Component87, Component88>(86, portion);
        SetupSystem<Component88, Component89>(87, portion);
        SetupSystem<Component89, Component90>(88, portion);
        SetupSystem<Component90, Component91>(89, portion);
        SetupSystem<Component91, Component92>(90, portion);
        SetupSystem<Component92, Component93>(91, portion);
        SetupSystem<Component93, Component94>(92, portion);
        SetupSystem<Component94, Component95>(93, portion);
        SetupSystem<Component95, Component96>(94, portion);
        SetupSystem<Component96, Component97>(95, portion);
        SetupSystem<Component97, Component98>(96, portion);
        SetupSystem<Component98, Component99>(97, portion);
        SetupSystem<Component99, Component100>(98, portion);
        SetupSystem<Component100, Component1>(99, EntityCount - portion * (SystemCount - 1));

        Context.FinishSetup();
    }

    [IterationSetup]
    public void IterationSetup() {
        for (var i = 0; i < SystemCount; i++) {
            _create[i]();
        }
    }

    [Benchmark]
    public void Run() {
        Context.Tick(Delta);
    }

    [IterationCleanup]
    public void IterationCleanup() {
        for (var i = 0; i < SystemCount; i++) {
            _delete[i]();
        }
    }

    [GlobalCleanup]
    public void GlobalCleanup() {
        Context.Cleanup();
        Context.Dispose();
        Context = default!;
    }

    private void SetupSystem<T1, T2>(int index, int count)
        where T1 : struct, Scellecs.Morpeh.IComponent, DCFApixels.DragonECS.IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
        where T2 : struct, Scellecs.Morpeh.IComponent, DCFApixels.DragonECS.IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
    {
        Context.Warmup<T1>(index + SystemCount);
        Context.Warmup<T1, T2>(index);
        unsafe {
            Context.AddSystem<T1, T2>(&Update<T1, T2>, index);
        }
        _sets[index] = Context.PrepareSet(count);
        _create[index] = () => Context.CreateEntities<T1>(_sets[index], index + SystemCount, default);
        _delete[index] = () => Context.DeleteEntities(_sets[index]);
    }

    private static void Update<T1, T2>(ref T1 c1, ref T2 c2) where T1 : struct where T2 : struct { }
}
