using System;
using Benchmark.Context;
using BenchmarkDotNet.Attributes;
using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using FrifloComponent = Friflo.Engine.ECS.IComponent;
using StaticEcsComponent = FFS.Libraries.StaticEcs.IComponent;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(RareFilterMismatchSystems<T, TE>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class RareFilterMismatchSystems<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
{
    private const float Delta = 0.1f;
    private const int SystemCount = 100;
    private const int PartialMatchesPerSystem = 2;
    private const int PartialPoolOffset = 100;
    private const int NoisePoolId = 200;

    [Params(Constants.SystemEntityCount)]
    public int EntityCount { get; set; }

    public T Context { get; set; }

    private readonly TE[][] _partialSets = new TE[100][];
    private readonly TE[][] _noiseSets = new TE[100][];

    [GlobalSetup]
    public void GlobalSetup()
    {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();

        Context.Warmup<Padding1>(NoisePoolId);
        for (var slotIndex = 0; slotIndex < SystemCount; slotIndex++)
            ConfigureSlot(slotIndex);

        var totalPartialEntities = Math.Min(EntityCount, SystemCount * PartialMatchesPerSystem);
        var partialBaseCount = totalPartialEntities / SystemCount;
        var partialRemainder = totalPartialEntities % SystemCount;

        var totalNoiseEntities = EntityCount - totalPartialEntities;
        var noiseBaseCount = totalNoiseEntities / SystemCount;
        var noiseRemainder = totalNoiseEntities % SystemCount;

        for (var slotIndex = 0; slotIndex < SystemCount; slotIndex++)
        {
            _partialSets[slotIndex] = Context.PrepareSet(partialBaseCount + (slotIndex < partialRemainder ? 1 : 0));
            _noiseSets[slotIndex] = Context.PrepareSet(noiseBaseCount + (slotIndex < noiseRemainder ? 1 : 0));
        }

        Context.FinishSetup();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        for (var slotIndex = 0; slotIndex < SystemCount; slotIndex++)
        {
            if (_noiseSets[slotIndex].Length > 0)
                Context.CreateEntities<Padding1>(_noiseSets[slotIndex], NoisePoolId, default(Padding1));

            if (_partialSets[slotIndex].Length > 0)
                CreatePartialSet(slotIndex);
        }
    }

    [Benchmark]
    public void Run()
    {
        Context.Tick(Delta);
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        for (var slotIndex = 0; slotIndex < SystemCount; slotIndex++)
        {
            Context.DeleteEntities(_partialSets[slotIndex]);
            Context.DeleteEntities(_noiseSets[slotIndex]);
        }
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        Context.Cleanup();
        Context.Dispose();
        Context = default(T)!;
    }

    private void ConfigureSlot(int index)
    {
        switch (index)
        {
            case 0:
                Context.Warmup<Component1>(0 + 100);
                Context.Warmup<Component1, Component2>(0);
                unsafe {
                    Context.AddSystem<Component1, Component2>(&Update<Component1, Component2>, 0);
                }
                return;
            case 1:
                Context.Warmup<Component2>(1 + 100);
                Context.Warmup<Component2, Component3>(1);
                unsafe {
                    Context.AddSystem<Component2, Component3>(&Update<Component2, Component3>, 1);
                }
                return;
            case 2:
                Context.Warmup<Component3>(2 + 100);
                Context.Warmup<Component3, Component4>(2);
                unsafe {
                    Context.AddSystem<Component3, Component4>(&Update<Component3, Component4>, 2);
                }
                return;
            case 3:
                Context.Warmup<Component4>(3 + 100);
                Context.Warmup<Component4, Component5>(3);
                unsafe {
                    Context.AddSystem<Component4, Component5>(&Update<Component4, Component5>, 3);
                }
                return;
            case 4:
                Context.Warmup<Component5>(4 + 100);
                Context.Warmup<Component5, Component6>(4);
                unsafe {
                    Context.AddSystem<Component5, Component6>(&Update<Component5, Component6>, 4);
                }
                return;
            case 5:
                Context.Warmup<Component6>(5 + 100);
                Context.Warmup<Component6, Component7>(5);
                unsafe {
                    Context.AddSystem<Component6, Component7>(&Update<Component6, Component7>, 5);
                }
                return;
            case 6:
                Context.Warmup<Component7>(6 + 100);
                Context.Warmup<Component7, Component8>(6);
                unsafe {
                    Context.AddSystem<Component7, Component8>(&Update<Component7, Component8>, 6);
                }
                return;
            case 7:
                Context.Warmup<Component8>(7 + 100);
                Context.Warmup<Component8, Component9>(7);
                unsafe {
                    Context.AddSystem<Component8, Component9>(&Update<Component8, Component9>, 7);
                }
                return;
            case 8:
                Context.Warmup<Component9>(8 + 100);
                Context.Warmup<Component9, Component10>(8);
                unsafe {
                    Context.AddSystem<Component9, Component10>(&Update<Component9, Component10>, 8);
                }
                return;
            case 9:
                Context.Warmup<Component10>(9 + 100);
                Context.Warmup<Component10, Component11>(9);
                unsafe {
                    Context.AddSystem<Component10, Component11>(&Update<Component10, Component11>, 9);
                }
                return;
            case 10:
                Context.Warmup<Component11>(10 + 100);
                Context.Warmup<Component11, Component12>(10);
                unsafe {
                    Context.AddSystem<Component11, Component12>(&Update<Component11, Component12>, 10);
                }
                return;
            case 11:
                Context.Warmup<Component12>(11 + 100);
                Context.Warmup<Component12, Component13>(11);
                unsafe {
                    Context.AddSystem<Component12, Component13>(&Update<Component12, Component13>, 11);
                }
                return;
            case 12:
                Context.Warmup<Component13>(12 + 100);
                Context.Warmup<Component13, Component14>(12);
                unsafe {
                    Context.AddSystem<Component13, Component14>(&Update<Component13, Component14>, 12);
                }
                return;
            case 13:
                Context.Warmup<Component14>(13 + 100);
                Context.Warmup<Component14, Component15>(13);
                unsafe {
                    Context.AddSystem<Component14, Component15>(&Update<Component14, Component15>, 13);
                }
                return;
            case 14:
                Context.Warmup<Component15>(14 + 100);
                Context.Warmup<Component15, Component16>(14);
                unsafe {
                    Context.AddSystem<Component15, Component16>(&Update<Component15, Component16>, 14);
                }
                return;
            case 15:
                Context.Warmup<Component16>(15 + 100);
                Context.Warmup<Component16, Component17>(15);
                unsafe {
                    Context.AddSystem<Component16, Component17>(&Update<Component16, Component17>, 15);
                }
                return;
            case 16:
                Context.Warmup<Component17>(16 + 100);
                Context.Warmup<Component17, Component18>(16);
                unsafe {
                    Context.AddSystem<Component17, Component18>(&Update<Component17, Component18>, 16);
                }
                return;
            case 17:
                Context.Warmup<Component18>(17 + 100);
                Context.Warmup<Component18, Component19>(17);
                unsafe {
                    Context.AddSystem<Component18, Component19>(&Update<Component18, Component19>, 17);
                }
                return;
            case 18:
                Context.Warmup<Component19>(18 + 100);
                Context.Warmup<Component19, Component20>(18);
                unsafe {
                    Context.AddSystem<Component19, Component20>(&Update<Component19, Component20>, 18);
                }
                return;
            case 19:
                Context.Warmup<Component20>(19 + 100);
                Context.Warmup<Component20, Component21>(19);
                unsafe {
                    Context.AddSystem<Component20, Component21>(&Update<Component20, Component21>, 19);
                }
                return;
            case 20:
                Context.Warmup<Component21>(20 + 100);
                Context.Warmup<Component21, Component22>(20);
                unsafe {
                    Context.AddSystem<Component21, Component22>(&Update<Component21, Component22>, 20);
                }
                return;
            case 21:
                Context.Warmup<Component22>(21 + 100);
                Context.Warmup<Component22, Component23>(21);
                unsafe {
                    Context.AddSystem<Component22, Component23>(&Update<Component22, Component23>, 21);
                }
                return;
            case 22:
                Context.Warmup<Component23>(22 + 100);
                Context.Warmup<Component23, Component24>(22);
                unsafe {
                    Context.AddSystem<Component23, Component24>(&Update<Component23, Component24>, 22);
                }
                return;
            case 23:
                Context.Warmup<Component24>(23 + 100);
                Context.Warmup<Component24, Component25>(23);
                unsafe {
                    Context.AddSystem<Component24, Component25>(&Update<Component24, Component25>, 23);
                }
                return;
            case 24:
                Context.Warmup<Component25>(24 + 100);
                Context.Warmup<Component25, Component26>(24);
                unsafe {
                    Context.AddSystem<Component25, Component26>(&Update<Component25, Component26>, 24);
                }
                return;
            case 25:
                Context.Warmup<Component26>(25 + 100);
                Context.Warmup<Component26, Component27>(25);
                unsafe {
                    Context.AddSystem<Component26, Component27>(&Update<Component26, Component27>, 25);
                }
                return;
            case 26:
                Context.Warmup<Component27>(26 + 100);
                Context.Warmup<Component27, Component28>(26);
                unsafe {
                    Context.AddSystem<Component27, Component28>(&Update<Component27, Component28>, 26);
                }
                return;
            case 27:
                Context.Warmup<Component28>(27 + 100);
                Context.Warmup<Component28, Component29>(27);
                unsafe {
                    Context.AddSystem<Component28, Component29>(&Update<Component28, Component29>, 27);
                }
                return;
            case 28:
                Context.Warmup<Component29>(28 + 100);
                Context.Warmup<Component29, Component30>(28);
                unsafe {
                    Context.AddSystem<Component29, Component30>(&Update<Component29, Component30>, 28);
                }
                return;
            case 29:
                Context.Warmup<Component30>(29 + 100);
                Context.Warmup<Component30, Component31>(29);
                unsafe {
                    Context.AddSystem<Component30, Component31>(&Update<Component30, Component31>, 29);
                }
                return;
            case 30:
                Context.Warmup<Component31>(30 + 100);
                Context.Warmup<Component31, Component32>(30);
                unsafe {
                    Context.AddSystem<Component31, Component32>(&Update<Component31, Component32>, 30);
                }
                return;
            case 31:
                Context.Warmup<Component32>(31 + 100);
                Context.Warmup<Component32, Component33>(31);
                unsafe {
                    Context.AddSystem<Component32, Component33>(&Update<Component32, Component33>, 31);
                }
                return;
            case 32:
                Context.Warmup<Component33>(32 + 100);
                Context.Warmup<Component33, Component34>(32);
                unsafe {
                    Context.AddSystem<Component33, Component34>(&Update<Component33, Component34>, 32);
                }
                return;
            case 33:
                Context.Warmup<Component34>(33 + 100);
                Context.Warmup<Component34, Component35>(33);
                unsafe {
                    Context.AddSystem<Component34, Component35>(&Update<Component34, Component35>, 33);
                }
                return;
            case 34:
                Context.Warmup<Component35>(34 + 100);
                Context.Warmup<Component35, Component36>(34);
                unsafe {
                    Context.AddSystem<Component35, Component36>(&Update<Component35, Component36>, 34);
                }
                return;
            case 35:
                Context.Warmup<Component36>(35 + 100);
                Context.Warmup<Component36, Component37>(35);
                unsafe {
                    Context.AddSystem<Component36, Component37>(&Update<Component36, Component37>, 35);
                }
                return;
            case 36:
                Context.Warmup<Component37>(36 + 100);
                Context.Warmup<Component37, Component38>(36);
                unsafe {
                    Context.AddSystem<Component37, Component38>(&Update<Component37, Component38>, 36);
                }
                return;
            case 37:
                Context.Warmup<Component38>(37 + 100);
                Context.Warmup<Component38, Component39>(37);
                unsafe {
                    Context.AddSystem<Component38, Component39>(&Update<Component38, Component39>, 37);
                }
                return;
            case 38:
                Context.Warmup<Component39>(38 + 100);
                Context.Warmup<Component39, Component40>(38);
                unsafe {
                    Context.AddSystem<Component39, Component40>(&Update<Component39, Component40>, 38);
                }
                return;
            case 39:
                Context.Warmup<Component40>(39 + 100);
                Context.Warmup<Component40, Component41>(39);
                unsafe {
                    Context.AddSystem<Component40, Component41>(&Update<Component40, Component41>, 39);
                }
                return;
            case 40:
                Context.Warmup<Component41>(40 + 100);
                Context.Warmup<Component41, Component42>(40);
                unsafe {
                    Context.AddSystem<Component41, Component42>(&Update<Component41, Component42>, 40);
                }
                return;
            case 41:
                Context.Warmup<Component42>(41 + 100);
                Context.Warmup<Component42, Component43>(41);
                unsafe {
                    Context.AddSystem<Component42, Component43>(&Update<Component42, Component43>, 41);
                }
                return;
            case 42:
                Context.Warmup<Component43>(42 + 100);
                Context.Warmup<Component43, Component44>(42);
                unsafe {
                    Context.AddSystem<Component43, Component44>(&Update<Component43, Component44>, 42);
                }
                return;
            case 43:
                Context.Warmup<Component44>(43 + 100);
                Context.Warmup<Component44, Component45>(43);
                unsafe {
                    Context.AddSystem<Component44, Component45>(&Update<Component44, Component45>, 43);
                }
                return;
            case 44:
                Context.Warmup<Component45>(44 + 100);
                Context.Warmup<Component45, Component46>(44);
                unsafe {
                    Context.AddSystem<Component45, Component46>(&Update<Component45, Component46>, 44);
                }
                return;
            case 45:
                Context.Warmup<Component46>(45 + 100);
                Context.Warmup<Component46, Component47>(45);
                unsafe {
                    Context.AddSystem<Component46, Component47>(&Update<Component46, Component47>, 45);
                }
                return;
            case 46:
                Context.Warmup<Component47>(46 + 100);
                Context.Warmup<Component47, Component48>(46);
                unsafe {
                    Context.AddSystem<Component47, Component48>(&Update<Component47, Component48>, 46);
                }
                return;
            case 47:
                Context.Warmup<Component48>(47 + 100);
                Context.Warmup<Component48, Component49>(47);
                unsafe {
                    Context.AddSystem<Component48, Component49>(&Update<Component48, Component49>, 47);
                }
                return;
            case 48:
                Context.Warmup<Component49>(48 + 100);
                Context.Warmup<Component49, Component50>(48);
                unsafe {
                    Context.AddSystem<Component49, Component50>(&Update<Component49, Component50>, 48);
                }
                return;
            case 49:
                Context.Warmup<Component50>(49 + 100);
                Context.Warmup<Component50, Component51>(49);
                unsafe {
                    Context.AddSystem<Component50, Component51>(&Update<Component50, Component51>, 49);
                }
                return;
            case 50:
                Context.Warmup<Component51>(50 + 100);
                Context.Warmup<Component51, Component52>(50);
                unsafe {
                    Context.AddSystem<Component51, Component52>(&Update<Component51, Component52>, 50);
                }
                return;
            case 51:
                Context.Warmup<Component52>(51 + 100);
                Context.Warmup<Component52, Component53>(51);
                unsafe {
                    Context.AddSystem<Component52, Component53>(&Update<Component52, Component53>, 51);
                }
                return;
            case 52:
                Context.Warmup<Component53>(52 + 100);
                Context.Warmup<Component53, Component54>(52);
                unsafe {
                    Context.AddSystem<Component53, Component54>(&Update<Component53, Component54>, 52);
                }
                return;
            case 53:
                Context.Warmup<Component54>(53 + 100);
                Context.Warmup<Component54, Component55>(53);
                unsafe {
                    Context.AddSystem<Component54, Component55>(&Update<Component54, Component55>, 53);
                }
                return;
            case 54:
                Context.Warmup<Component55>(54 + 100);
                Context.Warmup<Component55, Component56>(54);
                unsafe {
                    Context.AddSystem<Component55, Component56>(&Update<Component55, Component56>, 54);
                }
                return;
            case 55:
                Context.Warmup<Component56>(55 + 100);
                Context.Warmup<Component56, Component57>(55);
                unsafe {
                    Context.AddSystem<Component56, Component57>(&Update<Component56, Component57>, 55);
                }
                return;
            case 56:
                Context.Warmup<Component57>(56 + 100);
                Context.Warmup<Component57, Component58>(56);
                unsafe {
                    Context.AddSystem<Component57, Component58>(&Update<Component57, Component58>, 56);
                }
                return;
            case 57:
                Context.Warmup<Component58>(57 + 100);
                Context.Warmup<Component58, Component59>(57);
                unsafe {
                    Context.AddSystem<Component58, Component59>(&Update<Component58, Component59>, 57);
                }
                return;
            case 58:
                Context.Warmup<Component59>(58 + 100);
                Context.Warmup<Component59, Component60>(58);
                unsafe {
                    Context.AddSystem<Component59, Component60>(&Update<Component59, Component60>, 58);
                }
                return;
            case 59:
                Context.Warmup<Component60>(59 + 100);
                Context.Warmup<Component60, Component61>(59);
                unsafe {
                    Context.AddSystem<Component60, Component61>(&Update<Component60, Component61>, 59);
                }
                return;
            case 60:
                Context.Warmup<Component61>(60 + 100);
                Context.Warmup<Component61, Component62>(60);
                unsafe {
                    Context.AddSystem<Component61, Component62>(&Update<Component61, Component62>, 60);
                }
                return;
            case 61:
                Context.Warmup<Component62>(61 + 100);
                Context.Warmup<Component62, Component63>(61);
                unsafe {
                    Context.AddSystem<Component62, Component63>(&Update<Component62, Component63>, 61);
                }
                return;
            case 62:
                Context.Warmup<Component63>(62 + 100);
                Context.Warmup<Component63, Component64>(62);
                unsafe {
                    Context.AddSystem<Component63, Component64>(&Update<Component63, Component64>, 62);
                }
                return;
            case 63:
                Context.Warmup<Component64>(63 + 100);
                Context.Warmup<Component64, Component65>(63);
                unsafe {
                    Context.AddSystem<Component64, Component65>(&Update<Component64, Component65>, 63);
                }
                return;
            case 64:
                Context.Warmup<Component65>(64 + 100);
                Context.Warmup<Component65, Component66>(64);
                unsafe {
                    Context.AddSystem<Component65, Component66>(&Update<Component65, Component66>, 64);
                }
                return;
            case 65:
                Context.Warmup<Component66>(65 + 100);
                Context.Warmup<Component66, Component67>(65);
                unsafe {
                    Context.AddSystem<Component66, Component67>(&Update<Component66, Component67>, 65);
                }
                return;
            case 66:
                Context.Warmup<Component67>(66 + 100);
                Context.Warmup<Component67, Component68>(66);
                unsafe {
                    Context.AddSystem<Component67, Component68>(&Update<Component67, Component68>, 66);
                }
                return;
            case 67:
                Context.Warmup<Component68>(67 + 100);
                Context.Warmup<Component68, Component69>(67);
                unsafe {
                    Context.AddSystem<Component68, Component69>(&Update<Component68, Component69>, 67);
                }
                return;
            case 68:
                Context.Warmup<Component69>(68 + 100);
                Context.Warmup<Component69, Component70>(68);
                unsafe {
                    Context.AddSystem<Component69, Component70>(&Update<Component69, Component70>, 68);
                }
                return;
            case 69:
                Context.Warmup<Component70>(69 + 100);
                Context.Warmup<Component70, Component71>(69);
                unsafe {
                    Context.AddSystem<Component70, Component71>(&Update<Component70, Component71>, 69);
                }
                return;
            case 70:
                Context.Warmup<Component71>(70 + 100);
                Context.Warmup<Component71, Component72>(70);
                unsafe {
                    Context.AddSystem<Component71, Component72>(&Update<Component71, Component72>, 70);
                }
                return;
            case 71:
                Context.Warmup<Component72>(71 + 100);
                Context.Warmup<Component72, Component73>(71);
                unsafe {
                    Context.AddSystem<Component72, Component73>(&Update<Component72, Component73>, 71);
                }
                return;
            case 72:
                Context.Warmup<Component73>(72 + 100);
                Context.Warmup<Component73, Component74>(72);
                unsafe {
                    Context.AddSystem<Component73, Component74>(&Update<Component73, Component74>, 72);
                }
                return;
            case 73:
                Context.Warmup<Component74>(73 + 100);
                Context.Warmup<Component74, Component75>(73);
                unsafe {
                    Context.AddSystem<Component74, Component75>(&Update<Component74, Component75>, 73);
                }
                return;
            case 74:
                Context.Warmup<Component75>(74 + 100);
                Context.Warmup<Component75, Component76>(74);
                unsafe {
                    Context.AddSystem<Component75, Component76>(&Update<Component75, Component76>, 74);
                }
                return;
            case 75:
                Context.Warmup<Component76>(75 + 100);
                Context.Warmup<Component76, Component77>(75);
                unsafe {
                    Context.AddSystem<Component76, Component77>(&Update<Component76, Component77>, 75);
                }
                return;
            case 76:
                Context.Warmup<Component77>(76 + 100);
                Context.Warmup<Component77, Component78>(76);
                unsafe {
                    Context.AddSystem<Component77, Component78>(&Update<Component77, Component78>, 76);
                }
                return;
            case 77:
                Context.Warmup<Component78>(77 + 100);
                Context.Warmup<Component78, Component79>(77);
                unsafe {
                    Context.AddSystem<Component78, Component79>(&Update<Component78, Component79>, 77);
                }
                return;
            case 78:
                Context.Warmup<Component79>(78 + 100);
                Context.Warmup<Component79, Component80>(78);
                unsafe {
                    Context.AddSystem<Component79, Component80>(&Update<Component79, Component80>, 78);
                }
                return;
            case 79:
                Context.Warmup<Component80>(79 + 100);
                Context.Warmup<Component80, Component81>(79);
                unsafe {
                    Context.AddSystem<Component80, Component81>(&Update<Component80, Component81>, 79);
                }
                return;
            case 80:
                Context.Warmup<Component81>(80 + 100);
                Context.Warmup<Component81, Component82>(80);
                unsafe {
                    Context.AddSystem<Component81, Component82>(&Update<Component81, Component82>, 80);
                }
                return;
            case 81:
                Context.Warmup<Component82>(81 + 100);
                Context.Warmup<Component82, Component83>(81);
                unsafe {
                    Context.AddSystem<Component82, Component83>(&Update<Component82, Component83>, 81);
                }
                return;
            case 82:
                Context.Warmup<Component83>(82 + 100);
                Context.Warmup<Component83, Component84>(82);
                unsafe {
                    Context.AddSystem<Component83, Component84>(&Update<Component83, Component84>, 82);
                }
                return;
            case 83:
                Context.Warmup<Component84>(83 + 100);
                Context.Warmup<Component84, Component85>(83);
                unsafe {
                    Context.AddSystem<Component84, Component85>(&Update<Component84, Component85>, 83);
                }
                return;
            case 84:
                Context.Warmup<Component85>(84 + 100);
                Context.Warmup<Component85, Component86>(84);
                unsafe {
                    Context.AddSystem<Component85, Component86>(&Update<Component85, Component86>, 84);
                }
                return;
            case 85:
                Context.Warmup<Component86>(85 + 100);
                Context.Warmup<Component86, Component87>(85);
                unsafe {
                    Context.AddSystem<Component86, Component87>(&Update<Component86, Component87>, 85);
                }
                return;
            case 86:
                Context.Warmup<Component87>(86 + 100);
                Context.Warmup<Component87, Component88>(86);
                unsafe {
                    Context.AddSystem<Component87, Component88>(&Update<Component87, Component88>, 86);
                }
                return;
            case 87:
                Context.Warmup<Component88>(87 + 100);
                Context.Warmup<Component88, Component89>(87);
                unsafe {
                    Context.AddSystem<Component88, Component89>(&Update<Component88, Component89>, 87);
                }
                return;
            case 88:
                Context.Warmup<Component89>(88 + 100);
                Context.Warmup<Component89, Component90>(88);
                unsafe {
                    Context.AddSystem<Component89, Component90>(&Update<Component89, Component90>, 88);
                }
                return;
            case 89:
                Context.Warmup<Component90>(89 + 100);
                Context.Warmup<Component90, Component91>(89);
                unsafe {
                    Context.AddSystem<Component90, Component91>(&Update<Component90, Component91>, 89);
                }
                return;
            case 90:
                Context.Warmup<Component91>(90 + 100);
                Context.Warmup<Component91, Component92>(90);
                unsafe {
                    Context.AddSystem<Component91, Component92>(&Update<Component91, Component92>, 90);
                }
                return;
            case 91:
                Context.Warmup<Component92>(91 + 100);
                Context.Warmup<Component92, Component93>(91);
                unsafe {
                    Context.AddSystem<Component92, Component93>(&Update<Component92, Component93>, 91);
                }
                return;
            case 92:
                Context.Warmup<Component93>(92 + 100);
                Context.Warmup<Component93, Component94>(92);
                unsafe {
                    Context.AddSystem<Component93, Component94>(&Update<Component93, Component94>, 92);
                }
                return;
            case 93:
                Context.Warmup<Component94>(93 + 100);
                Context.Warmup<Component94, Component95>(93);
                unsafe {
                    Context.AddSystem<Component94, Component95>(&Update<Component94, Component95>, 93);
                }
                return;
            case 94:
                Context.Warmup<Component95>(94 + 100);
                Context.Warmup<Component95, Component96>(94);
                unsafe {
                    Context.AddSystem<Component95, Component96>(&Update<Component95, Component96>, 94);
                }
                return;
            case 95:
                Context.Warmup<Component96>(95 + 100);
                Context.Warmup<Component96, Component97>(95);
                unsafe {
                    Context.AddSystem<Component96, Component97>(&Update<Component96, Component97>, 95);
                }
                return;
            case 96:
                Context.Warmup<Component97>(96 + 100);
                Context.Warmup<Component97, Component98>(96);
                unsafe {
                    Context.AddSystem<Component97, Component98>(&Update<Component97, Component98>, 96);
                }
                return;
            case 97:
                Context.Warmup<Component98>(97 + 100);
                Context.Warmup<Component98, Component99>(97);
                unsafe {
                    Context.AddSystem<Component98, Component99>(&Update<Component98, Component99>, 97);
                }
                return;
            case 98:
                Context.Warmup<Component99>(98 + 100);
                Context.Warmup<Component99, Component100>(98);
                unsafe {
                    Context.AddSystem<Component99, Component100>(&Update<Component99, Component100>, 98);
                }
                return;
            case 99:
                Context.Warmup<Component100>(99 + 100);
                Context.Warmup<Component100, Component1>(99);
                unsafe {
                    Context.AddSystem<Component100, Component1>(&Update<Component100, Component1>, 99);
                }
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), index, null);
        }
    }

    private void CreatePartialSet(int index)
    {
        switch (index)
        {
            case 0:
                Context.CreateEntities<Component1>(_partialSets[0], 0 + 100, default(Component1));
                return;
            case 1:
                Context.CreateEntities<Component2>(_partialSets[1], 1 + 100, default(Component2));
                return;
            case 2:
                Context.CreateEntities<Component3>(_partialSets[2], 2 + 100, default(Component3));
                return;
            case 3:
                Context.CreateEntities<Component4>(_partialSets[3], 3 + 100, default(Component4));
                return;
            case 4:
                Context.CreateEntities<Component5>(_partialSets[4], 4 + 100, default(Component5));
                return;
            case 5:
                Context.CreateEntities<Component6>(_partialSets[5], 5 + 100, default(Component6));
                return;
            case 6:
                Context.CreateEntities<Component7>(_partialSets[6], 6 + 100, default(Component7));
                return;
            case 7:
                Context.CreateEntities<Component8>(_partialSets[7], 7 + 100, default(Component8));
                return;
            case 8:
                Context.CreateEntities<Component9>(_partialSets[8], 8 + 100, default(Component9));
                return;
            case 9:
                Context.CreateEntities<Component10>(_partialSets[9], 9 + 100, default(Component10));
                return;
            case 10:
                Context.CreateEntities<Component11>(_partialSets[10], 10 + 100, default(Component11));
                return;
            case 11:
                Context.CreateEntities<Component12>(_partialSets[11], 11 + 100, default(Component12));
                return;
            case 12:
                Context.CreateEntities<Component13>(_partialSets[12], 12 + 100, default(Component13));
                return;
            case 13:
                Context.CreateEntities<Component14>(_partialSets[13], 13 + 100, default(Component14));
                return;
            case 14:
                Context.CreateEntities<Component15>(_partialSets[14], 14 + 100, default(Component15));
                return;
            case 15:
                Context.CreateEntities<Component16>(_partialSets[15], 15 + 100, default(Component16));
                return;
            case 16:
                Context.CreateEntities<Component17>(_partialSets[16], 16 + 100, default(Component17));
                return;
            case 17:
                Context.CreateEntities<Component18>(_partialSets[17], 17 + 100, default(Component18));
                return;
            case 18:
                Context.CreateEntities<Component19>(_partialSets[18], 18 + 100, default(Component19));
                return;
            case 19:
                Context.CreateEntities<Component20>(_partialSets[19], 19 + 100, default(Component20));
                return;
            case 20:
                Context.CreateEntities<Component21>(_partialSets[20], 20 + 100, default(Component21));
                return;
            case 21:
                Context.CreateEntities<Component22>(_partialSets[21], 21 + 100, default(Component22));
                return;
            case 22:
                Context.CreateEntities<Component23>(_partialSets[22], 22 + 100, default(Component23));
                return;
            case 23:
                Context.CreateEntities<Component24>(_partialSets[23], 23 + 100, default(Component24));
                return;
            case 24:
                Context.CreateEntities<Component25>(_partialSets[24], 24 + 100, default(Component25));
                return;
            case 25:
                Context.CreateEntities<Component26>(_partialSets[25], 25 + 100, default(Component26));
                return;
            case 26:
                Context.CreateEntities<Component27>(_partialSets[26], 26 + 100, default(Component27));
                return;
            case 27:
                Context.CreateEntities<Component28>(_partialSets[27], 27 + 100, default(Component28));
                return;
            case 28:
                Context.CreateEntities<Component29>(_partialSets[28], 28 + 100, default(Component29));
                return;
            case 29:
                Context.CreateEntities<Component30>(_partialSets[29], 29 + 100, default(Component30));
                return;
            case 30:
                Context.CreateEntities<Component31>(_partialSets[30], 30 + 100, default(Component31));
                return;
            case 31:
                Context.CreateEntities<Component32>(_partialSets[31], 31 + 100, default(Component32));
                return;
            case 32:
                Context.CreateEntities<Component33>(_partialSets[32], 32 + 100, default(Component33));
                return;
            case 33:
                Context.CreateEntities<Component34>(_partialSets[33], 33 + 100, default(Component34));
                return;
            case 34:
                Context.CreateEntities<Component35>(_partialSets[34], 34 + 100, default(Component35));
                return;
            case 35:
                Context.CreateEntities<Component36>(_partialSets[35], 35 + 100, default(Component36));
                return;
            case 36:
                Context.CreateEntities<Component37>(_partialSets[36], 36 + 100, default(Component37));
                return;
            case 37:
                Context.CreateEntities<Component38>(_partialSets[37], 37 + 100, default(Component38));
                return;
            case 38:
                Context.CreateEntities<Component39>(_partialSets[38], 38 + 100, default(Component39));
                return;
            case 39:
                Context.CreateEntities<Component40>(_partialSets[39], 39 + 100, default(Component40));
                return;
            case 40:
                Context.CreateEntities<Component41>(_partialSets[40], 40 + 100, default(Component41));
                return;
            case 41:
                Context.CreateEntities<Component42>(_partialSets[41], 41 + 100, default(Component42));
                return;
            case 42:
                Context.CreateEntities<Component43>(_partialSets[42], 42 + 100, default(Component43));
                return;
            case 43:
                Context.CreateEntities<Component44>(_partialSets[43], 43 + 100, default(Component44));
                return;
            case 44:
                Context.CreateEntities<Component45>(_partialSets[44], 44 + 100, default(Component45));
                return;
            case 45:
                Context.CreateEntities<Component46>(_partialSets[45], 45 + 100, default(Component46));
                return;
            case 46:
                Context.CreateEntities<Component47>(_partialSets[46], 46 + 100, default(Component47));
                return;
            case 47:
                Context.CreateEntities<Component48>(_partialSets[47], 47 + 100, default(Component48));
                return;
            case 48:
                Context.CreateEntities<Component49>(_partialSets[48], 48 + 100, default(Component49));
                return;
            case 49:
                Context.CreateEntities<Component50>(_partialSets[49], 49 + 100, default(Component50));
                return;
            case 50:
                Context.CreateEntities<Component51>(_partialSets[50], 50 + 100, default(Component51));
                return;
            case 51:
                Context.CreateEntities<Component52>(_partialSets[51], 51 + 100, default(Component52));
                return;
            case 52:
                Context.CreateEntities<Component53>(_partialSets[52], 52 + 100, default(Component53));
                return;
            case 53:
                Context.CreateEntities<Component54>(_partialSets[53], 53 + 100, default(Component54));
                return;
            case 54:
                Context.CreateEntities<Component55>(_partialSets[54], 54 + 100, default(Component55));
                return;
            case 55:
                Context.CreateEntities<Component56>(_partialSets[55], 55 + 100, default(Component56));
                return;
            case 56:
                Context.CreateEntities<Component57>(_partialSets[56], 56 + 100, default(Component57));
                return;
            case 57:
                Context.CreateEntities<Component58>(_partialSets[57], 57 + 100, default(Component58));
                return;
            case 58:
                Context.CreateEntities<Component59>(_partialSets[58], 58 + 100, default(Component59));
                return;
            case 59:
                Context.CreateEntities<Component60>(_partialSets[59], 59 + 100, default(Component60));
                return;
            case 60:
                Context.CreateEntities<Component61>(_partialSets[60], 60 + 100, default(Component61));
                return;
            case 61:
                Context.CreateEntities<Component62>(_partialSets[61], 61 + 100, default(Component62));
                return;
            case 62:
                Context.CreateEntities<Component63>(_partialSets[62], 62 + 100, default(Component63));
                return;
            case 63:
                Context.CreateEntities<Component64>(_partialSets[63], 63 + 100, default(Component64));
                return;
            case 64:
                Context.CreateEntities<Component65>(_partialSets[64], 64 + 100, default(Component65));
                return;
            case 65:
                Context.CreateEntities<Component66>(_partialSets[65], 65 + 100, default(Component66));
                return;
            case 66:
                Context.CreateEntities<Component67>(_partialSets[66], 66 + 100, default(Component67));
                return;
            case 67:
                Context.CreateEntities<Component68>(_partialSets[67], 67 + 100, default(Component68));
                return;
            case 68:
                Context.CreateEntities<Component69>(_partialSets[68], 68 + 100, default(Component69));
                return;
            case 69:
                Context.CreateEntities<Component70>(_partialSets[69], 69 + 100, default(Component70));
                return;
            case 70:
                Context.CreateEntities<Component71>(_partialSets[70], 70 + 100, default(Component71));
                return;
            case 71:
                Context.CreateEntities<Component72>(_partialSets[71], 71 + 100, default(Component72));
                return;
            case 72:
                Context.CreateEntities<Component73>(_partialSets[72], 72 + 100, default(Component73));
                return;
            case 73:
                Context.CreateEntities<Component74>(_partialSets[73], 73 + 100, default(Component74));
                return;
            case 74:
                Context.CreateEntities<Component75>(_partialSets[74], 74 + 100, default(Component75));
                return;
            case 75:
                Context.CreateEntities<Component76>(_partialSets[75], 75 + 100, default(Component76));
                return;
            case 76:
                Context.CreateEntities<Component77>(_partialSets[76], 76 + 100, default(Component77));
                return;
            case 77:
                Context.CreateEntities<Component78>(_partialSets[77], 77 + 100, default(Component78));
                return;
            case 78:
                Context.CreateEntities<Component79>(_partialSets[78], 78 + 100, default(Component79));
                return;
            case 79:
                Context.CreateEntities<Component80>(_partialSets[79], 79 + 100, default(Component80));
                return;
            case 80:
                Context.CreateEntities<Component81>(_partialSets[80], 80 + 100, default(Component81));
                return;
            case 81:
                Context.CreateEntities<Component82>(_partialSets[81], 81 + 100, default(Component82));
                return;
            case 82:
                Context.CreateEntities<Component83>(_partialSets[82], 82 + 100, default(Component83));
                return;
            case 83:
                Context.CreateEntities<Component84>(_partialSets[83], 83 + 100, default(Component84));
                return;
            case 84:
                Context.CreateEntities<Component85>(_partialSets[84], 84 + 100, default(Component85));
                return;
            case 85:
                Context.CreateEntities<Component86>(_partialSets[85], 85 + 100, default(Component86));
                return;
            case 86:
                Context.CreateEntities<Component87>(_partialSets[86], 86 + 100, default(Component87));
                return;
            case 87:
                Context.CreateEntities<Component88>(_partialSets[87], 87 + 100, default(Component88));
                return;
            case 88:
                Context.CreateEntities<Component89>(_partialSets[88], 88 + 100, default(Component89));
                return;
            case 89:
                Context.CreateEntities<Component90>(_partialSets[89], 89 + 100, default(Component90));
                return;
            case 90:
                Context.CreateEntities<Component91>(_partialSets[90], 90 + 100, default(Component91));
                return;
            case 91:
                Context.CreateEntities<Component92>(_partialSets[91], 91 + 100, default(Component92));
                return;
            case 92:
                Context.CreateEntities<Component93>(_partialSets[92], 92 + 100, default(Component93));
                return;
            case 93:
                Context.CreateEntities<Component94>(_partialSets[93], 93 + 100, default(Component94));
                return;
            case 94:
                Context.CreateEntities<Component95>(_partialSets[94], 94 + 100, default(Component95));
                return;
            case 95:
                Context.CreateEntities<Component96>(_partialSets[95], 95 + 100, default(Component96));
                return;
            case 96:
                Context.CreateEntities<Component97>(_partialSets[96], 96 + 100, default(Component97));
                return;
            case 97:
                Context.CreateEntities<Component98>(_partialSets[97], 97 + 100, default(Component98));
                return;
            case 98:
                Context.CreateEntities<Component99>(_partialSets[98], 98 + 100, default(Component99));
                return;
            case 99:
                Context.CreateEntities<Component100>(_partialSets[99], 99 + 100, default(Component100));
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), index, null);
        }
    }

    private static void Update<T1, T2>(ref T1 c1, ref T2 c2) where T1 : struct where T2 : struct { }
}
