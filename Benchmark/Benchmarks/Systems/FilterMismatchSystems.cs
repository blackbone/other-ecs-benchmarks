using System;
using Benchmark.Context;
using BenchmarkDotNet.Attributes;
using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using XenoComponent = Xeno.IComponent;
using FrifloComponent = Friflo.Engine.ECS.IComponent;
using StaticEcsComponent = FFS.Libraries.StaticEcs.IComponent;

namespace Benchmark.Benchmarks.Systems;

[ArtifactsPath(".benchmark_results/" + nameof(FilterMismatchSystems<T, TE>))]
[MemoryDiagnoser]
#if CHECK_CACHE_MISSES
[HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.CacheMisses)]
#endif
public abstract class FilterMismatchSystems<T, TE> : IBenchmark<T, TE> where T : IBenchmarkContext<TE>
{
    private const float Delta = 0.1f;

    [Params(Constants.SystemEntityCount)]
    public int EntityCount { get; set; }

    public T Context { get; set; }

    private readonly TE[][] _sets = new TE[100][];

    [GlobalSetup]
    public void GlobalSetup() {
        Context = BenchmarkContext.Create<T>(EntityCount);
        Context.Setup();

        var portion = EntityCount / 100;
        Context.Warmup<Component1>(0 + 100);
        Context.Warmup<Component1, Component2>(0);
        unsafe {
            Context.AddSystem<Component1, Component2>(&Update<Component1, Component2>, 0);
        }
        _sets[0] = Context.PrepareSet(portion);

        Context.Warmup<Component2>(1 + 100);
        Context.Warmup<Component2, Component3>(1);
        unsafe {
            Context.AddSystem<Component2, Component3>(&Update<Component2, Component3>, 1);
        }
        _sets[1] = Context.PrepareSet(portion);

        Context.Warmup<Component3>(2 + 100);
        Context.Warmup<Component3, Component4>(2);
        unsafe {
            Context.AddSystem<Component3, Component4>(&Update<Component3, Component4>, 2);
        }
        _sets[2] = Context.PrepareSet(portion);

        Context.Warmup<Component4>(3 + 100);
        Context.Warmup<Component4, Component5>(3);
        unsafe {
            Context.AddSystem<Component4, Component5>(&Update<Component4, Component5>, 3);
        }
        _sets[3] = Context.PrepareSet(portion);

        Context.Warmup<Component5>(4 + 100);
        Context.Warmup<Component5, Component6>(4);
        unsafe {
            Context.AddSystem<Component5, Component6>(&Update<Component5, Component6>, 4);
        }
        _sets[4] = Context.PrepareSet(portion);

        Context.Warmup<Component6>(5 + 100);
        Context.Warmup<Component6, Component7>(5);
        unsafe {
            Context.AddSystem<Component6, Component7>(&Update<Component6, Component7>, 5);
        }
        _sets[5] = Context.PrepareSet(portion);

        Context.Warmup<Component7>(6 + 100);
        Context.Warmup<Component7, Component8>(6);
        unsafe {
            Context.AddSystem<Component7, Component8>(&Update<Component7, Component8>, 6);
        }
        _sets[6] = Context.PrepareSet(portion);

        Context.Warmup<Component8>(7 + 100);
        Context.Warmup<Component8, Component9>(7);
        unsafe {
            Context.AddSystem<Component8, Component9>(&Update<Component8, Component9>, 7);
        }
        _sets[7] = Context.PrepareSet(portion);

        Context.Warmup<Component9>(8 + 100);
        Context.Warmup<Component9, Component10>(8);
        unsafe {
            Context.AddSystem<Component9, Component10>(&Update<Component9, Component10>, 8);
        }
        _sets[8] = Context.PrepareSet(portion);

        Context.Warmup<Component10>(9 + 100);
        Context.Warmup<Component10, Component11>(9);
        unsafe {
            Context.AddSystem<Component10, Component11>(&Update<Component10, Component11>, 9);
        }
        _sets[9] = Context.PrepareSet(portion);

        Context.Warmup<Component11>(10 + 100);
        Context.Warmup<Component11, Component12>(10);
        unsafe {
            Context.AddSystem<Component11, Component12>(&Update<Component11, Component12>, 10);
        }
        _sets[10] = Context.PrepareSet(portion);

        Context.Warmup<Component12>(11 + 100);
        Context.Warmup<Component12, Component13>(11);
        unsafe {
            Context.AddSystem<Component12, Component13>(&Update<Component12, Component13>, 11);
        }
        _sets[11] = Context.PrepareSet(portion);

        Context.Warmup<Component13>(12 + 100);
        Context.Warmup<Component13, Component14>(12);
        unsafe {
            Context.AddSystem<Component13, Component14>(&Update<Component13, Component14>, 12);
        }
        _sets[12] = Context.PrepareSet(portion);

        Context.Warmup<Component14>(13 + 100);
        Context.Warmup<Component14, Component15>(13);
        unsafe {
            Context.AddSystem<Component14, Component15>(&Update<Component14, Component15>, 13);
        }
        _sets[13] = Context.PrepareSet(portion);

        Context.Warmup<Component15>(14 + 100);
        Context.Warmup<Component15, Component16>(14);
        unsafe {
            Context.AddSystem<Component15, Component16>(&Update<Component15, Component16>, 14);
        }
        _sets[14] = Context.PrepareSet(portion);

        Context.Warmup<Component16>(15 + 100);
        Context.Warmup<Component16, Component17>(15);
        unsafe {
            Context.AddSystem<Component16, Component17>(&Update<Component16, Component17>, 15);
        }
        _sets[15] = Context.PrepareSet(portion);

        Context.Warmup<Component17>(16 + 100);
        Context.Warmup<Component17, Component18>(16);
        unsafe {
            Context.AddSystem<Component17, Component18>(&Update<Component17, Component18>, 16);
        }
        _sets[16] = Context.PrepareSet(portion);

        Context.Warmup<Component18>(17 + 100);
        Context.Warmup<Component18, Component19>(17);
        unsafe {
            Context.AddSystem<Component18, Component19>(&Update<Component18, Component19>, 17);
        }
        _sets[17] = Context.PrepareSet(portion);

        Context.Warmup<Component19>(18 + 100);
        Context.Warmup<Component19, Component20>(18);
        unsafe {
            Context.AddSystem<Component19, Component20>(&Update<Component19, Component20>, 18);
        }
        _sets[18] = Context.PrepareSet(portion);

        Context.Warmup<Component20>(19 + 100);
        Context.Warmup<Component20, Component21>(19);
        unsafe {
            Context.AddSystem<Component20, Component21>(&Update<Component20, Component21>, 19);
        }
        _sets[19] = Context.PrepareSet(portion);

        Context.Warmup<Component21>(20 + 100);
        Context.Warmup<Component21, Component22>(20);
        unsafe {
            Context.AddSystem<Component21, Component22>(&Update<Component21, Component22>, 20);
        }
        _sets[20] = Context.PrepareSet(portion);

        Context.Warmup<Component22>(21 + 100);
        Context.Warmup<Component22, Component23>(21);
        unsafe {
            Context.AddSystem<Component22, Component23>(&Update<Component22, Component23>, 21);
        }
        _sets[21] = Context.PrepareSet(portion);

        Context.Warmup<Component23>(22 + 100);
        Context.Warmup<Component23, Component24>(22);
        unsafe {
            Context.AddSystem<Component23, Component24>(&Update<Component23, Component24>, 22);
        }
        _sets[22] = Context.PrepareSet(portion);

        Context.Warmup<Component24>(23 + 100);
        Context.Warmup<Component24, Component25>(23);
        unsafe {
            Context.AddSystem<Component24, Component25>(&Update<Component24, Component25>, 23);
        }
        _sets[23] = Context.PrepareSet(portion);

        Context.Warmup<Component25>(24 + 100);
        Context.Warmup<Component25, Component26>(24);
        unsafe {
            Context.AddSystem<Component25, Component26>(&Update<Component25, Component26>, 24);
        }
        _sets[24] = Context.PrepareSet(portion);

        Context.Warmup<Component26>(25 + 100);
        Context.Warmup<Component26, Component27>(25);
        unsafe {
            Context.AddSystem<Component26, Component27>(&Update<Component26, Component27>, 25);
        }
        _sets[25] = Context.PrepareSet(portion);

        Context.Warmup<Component27>(26 + 100);
        Context.Warmup<Component27, Component28>(26);
        unsafe {
            Context.AddSystem<Component27, Component28>(&Update<Component27, Component28>, 26);
        }
        _sets[26] = Context.PrepareSet(portion);

        Context.Warmup<Component28>(27 + 100);
        Context.Warmup<Component28, Component29>(27);
        unsafe {
            Context.AddSystem<Component28, Component29>(&Update<Component28, Component29>, 27);
        }
        _sets[27] = Context.PrepareSet(portion);

        Context.Warmup<Component29>(28 + 100);
        Context.Warmup<Component29, Component30>(28);
        unsafe {
            Context.AddSystem<Component29, Component30>(&Update<Component29, Component30>, 28);
        }
        _sets[28] = Context.PrepareSet(portion);

        Context.Warmup<Component30>(29 + 100);
        Context.Warmup<Component30, Component31>(29);
        unsafe {
            Context.AddSystem<Component30, Component31>(&Update<Component30, Component31>, 29);
        }
        _sets[29] = Context.PrepareSet(portion);

        Context.Warmup<Component31>(30 + 100);
        Context.Warmup<Component31, Component32>(30);
        unsafe {
            Context.AddSystem<Component31, Component32>(&Update<Component31, Component32>, 30);
        }
        _sets[30] = Context.PrepareSet(portion);

        Context.Warmup<Component32>(31 + 100);
        Context.Warmup<Component32, Component33>(31);
        unsafe {
            Context.AddSystem<Component32, Component33>(&Update<Component32, Component33>, 31);
        }
        _sets[31] = Context.PrepareSet(portion);

        Context.Warmup<Component33>(32 + 100);
        Context.Warmup<Component33, Component34>(32);
        unsafe {
            Context.AddSystem<Component33, Component34>(&Update<Component33, Component34>, 32);
        }
        _sets[32] = Context.PrepareSet(portion);

        Context.Warmup<Component34>(33 + 100);
        Context.Warmup<Component34, Component35>(33);
        unsafe {
            Context.AddSystem<Component34, Component35>(&Update<Component34, Component35>, 33);
        }
        _sets[33] = Context.PrepareSet(portion);

        Context.Warmup<Component35>(34 + 100);
        Context.Warmup<Component35, Component36>(34);
        unsafe {
            Context.AddSystem<Component35, Component36>(&Update<Component35, Component36>, 34);
        }
        _sets[34] = Context.PrepareSet(portion);

        Context.Warmup<Component36>(35 + 100);
        Context.Warmup<Component36, Component37>(35);
        unsafe {
            Context.AddSystem<Component36, Component37>(&Update<Component36, Component37>, 35);
        }
        _sets[35] = Context.PrepareSet(portion);

        Context.Warmup<Component37>(36 + 100);
        Context.Warmup<Component37, Component38>(36);
        unsafe {
            Context.AddSystem<Component37, Component38>(&Update<Component37, Component38>, 36);
        }
        _sets[36] = Context.PrepareSet(portion);

        Context.Warmup<Component38>(37 + 100);
        Context.Warmup<Component38, Component39>(37);
        unsafe {
            Context.AddSystem<Component38, Component39>(&Update<Component38, Component39>, 37);
        }
        _sets[37] = Context.PrepareSet(portion);

        Context.Warmup<Component39>(38 + 100);
        Context.Warmup<Component39, Component40>(38);
        unsafe {
            Context.AddSystem<Component39, Component40>(&Update<Component39, Component40>, 38);
        }
        _sets[38] = Context.PrepareSet(portion);

        Context.Warmup<Component40>(39 + 100);
        Context.Warmup<Component40, Component41>(39);
        unsafe {
            Context.AddSystem<Component40, Component41>(&Update<Component40, Component41>, 39);
        }
        _sets[39] = Context.PrepareSet(portion);

        Context.Warmup<Component41>(40 + 100);
        Context.Warmup<Component41, Component42>(40);
        unsafe {
            Context.AddSystem<Component41, Component42>(&Update<Component41, Component42>, 40);
        }
        _sets[40] = Context.PrepareSet(portion);

        Context.Warmup<Component42>(41 + 100);
        Context.Warmup<Component42, Component43>(41);
        unsafe {
            Context.AddSystem<Component42, Component43>(&Update<Component42, Component43>, 41);
        }
        _sets[41] = Context.PrepareSet(portion);

        Context.Warmup<Component43>(42 + 100);
        Context.Warmup<Component43, Component44>(42);
        unsafe {
            Context.AddSystem<Component43, Component44>(&Update<Component43, Component44>, 42);
        }
        _sets[42] = Context.PrepareSet(portion);

        Context.Warmup<Component44>(43 + 100);
        Context.Warmup<Component44, Component45>(43);
        unsafe {
            Context.AddSystem<Component44, Component45>(&Update<Component44, Component45>, 43);
        }
        _sets[43] = Context.PrepareSet(portion);

        Context.Warmup<Component45>(44 + 100);
        Context.Warmup<Component45, Component46>(44);
        unsafe {
            Context.AddSystem<Component45, Component46>(&Update<Component45, Component46>, 44);
        }
        _sets[44] = Context.PrepareSet(portion);

        Context.Warmup<Component46>(45 + 100);
        Context.Warmup<Component46, Component47>(45);
        unsafe {
            Context.AddSystem<Component46, Component47>(&Update<Component46, Component47>, 45);
        }
        _sets[45] = Context.PrepareSet(portion);

        Context.Warmup<Component47>(46 + 100);
        Context.Warmup<Component47, Component48>(46);
        unsafe {
            Context.AddSystem<Component47, Component48>(&Update<Component47, Component48>, 46);
        }
        _sets[46] = Context.PrepareSet(portion);

        Context.Warmup<Component48>(47 + 100);
        Context.Warmup<Component48, Component49>(47);
        unsafe {
            Context.AddSystem<Component48, Component49>(&Update<Component48, Component49>, 47);
        }
        _sets[47] = Context.PrepareSet(portion);

        Context.Warmup<Component49>(48 + 100);
        Context.Warmup<Component49, Component50>(48);
        unsafe {
            Context.AddSystem<Component49, Component50>(&Update<Component49, Component50>, 48);
        }
        _sets[48] = Context.PrepareSet(portion);

        Context.Warmup<Component50>(49 + 100);
        Context.Warmup<Component50, Component51>(49);
        unsafe {
            Context.AddSystem<Component50, Component51>(&Update<Component50, Component51>, 49);
        }
        _sets[49] = Context.PrepareSet(portion);

        Context.Warmup<Component51>(50 + 100);
        Context.Warmup<Component51, Component52>(50);
        unsafe {
            Context.AddSystem<Component51, Component52>(&Update<Component51, Component52>, 50);
        }
        _sets[50] = Context.PrepareSet(portion);

        Context.Warmup<Component52>(51 + 100);
        Context.Warmup<Component52, Component53>(51);
        unsafe {
            Context.AddSystem<Component52, Component53>(&Update<Component52, Component53>, 51);
        }
        _sets[51] = Context.PrepareSet(portion);

        Context.Warmup<Component53>(52 + 100);
        Context.Warmup<Component53, Component54>(52);
        unsafe {
            Context.AddSystem<Component53, Component54>(&Update<Component53, Component54>, 52);
        }
        _sets[52] = Context.PrepareSet(portion);

        Context.Warmup<Component54>(53 + 100);
        Context.Warmup<Component54, Component55>(53);
        unsafe {
            Context.AddSystem<Component54, Component55>(&Update<Component54, Component55>, 53);
        }
        _sets[53] = Context.PrepareSet(portion);

        Context.Warmup<Component55>(54 + 100);
        Context.Warmup<Component55, Component56>(54);
        unsafe {
            Context.AddSystem<Component55, Component56>(&Update<Component55, Component56>, 54);
        }
        _sets[54] = Context.PrepareSet(portion);

        Context.Warmup<Component56>(55 + 100);
        Context.Warmup<Component56, Component57>(55);
        unsafe {
            Context.AddSystem<Component56, Component57>(&Update<Component56, Component57>, 55);
        }
        _sets[55] = Context.PrepareSet(portion);

        Context.Warmup<Component57>(56 + 100);
        Context.Warmup<Component57, Component58>(56);
        unsafe {
            Context.AddSystem<Component57, Component58>(&Update<Component57, Component58>, 56);
        }
        _sets[56] = Context.PrepareSet(portion);

        Context.Warmup<Component58>(57 + 100);
        Context.Warmup<Component58, Component59>(57);
        unsafe {
            Context.AddSystem<Component58, Component59>(&Update<Component58, Component59>, 57);
        }
        _sets[57] = Context.PrepareSet(portion);

        Context.Warmup<Component59>(58 + 100);
        Context.Warmup<Component59, Component60>(58);
        unsafe {
            Context.AddSystem<Component59, Component60>(&Update<Component59, Component60>, 58);
        }
        _sets[58] = Context.PrepareSet(portion);

        Context.Warmup<Component60>(59 + 100);
        Context.Warmup<Component60, Component61>(59);
        unsafe {
            Context.AddSystem<Component60, Component61>(&Update<Component60, Component61>, 59);
        }
        _sets[59] = Context.PrepareSet(portion);

        Context.Warmup<Component61>(60 + 100);
        Context.Warmup<Component61, Component62>(60);
        unsafe {
            Context.AddSystem<Component61, Component62>(&Update<Component61, Component62>, 60);
        }
        _sets[60] = Context.PrepareSet(portion);

        Context.Warmup<Component62>(61 + 100);
        Context.Warmup<Component62, Component63>(61);
        unsafe {
            Context.AddSystem<Component62, Component63>(&Update<Component62, Component63>, 61);
        }
        _sets[61] = Context.PrepareSet(portion);

        Context.Warmup<Component63>(62 + 100);
        Context.Warmup<Component63, Component64>(62);
        unsafe {
            Context.AddSystem<Component63, Component64>(&Update<Component63, Component64>, 62);
        }
        _sets[62] = Context.PrepareSet(portion);

        Context.Warmup<Component64>(63 + 100);
        Context.Warmup<Component64, Component65>(63);
        unsafe {
            Context.AddSystem<Component64, Component65>(&Update<Component64, Component65>, 63);
        }
        _sets[63] = Context.PrepareSet(portion);

        Context.Warmup<Component65>(64 + 100);
        Context.Warmup<Component65, Component66>(64);
        unsafe {
            Context.AddSystem<Component65, Component66>(&Update<Component65, Component66>, 64);
        }
        _sets[64] = Context.PrepareSet(portion);

        Context.Warmup<Component66>(65 + 100);
        Context.Warmup<Component66, Component67>(65);
        unsafe {
            Context.AddSystem<Component66, Component67>(&Update<Component66, Component67>, 65);
        }
        _sets[65] = Context.PrepareSet(portion);

        Context.Warmup<Component67>(66 + 100);
        Context.Warmup<Component67, Component68>(66);
        unsafe {
            Context.AddSystem<Component67, Component68>(&Update<Component67, Component68>, 66);
        }
        _sets[66] = Context.PrepareSet(portion);

        Context.Warmup<Component68>(67 + 100);
        Context.Warmup<Component68, Component69>(67);
        unsafe {
            Context.AddSystem<Component68, Component69>(&Update<Component68, Component69>, 67);
        }
        _sets[67] = Context.PrepareSet(portion);

        Context.Warmup<Component69>(68 + 100);
        Context.Warmup<Component69, Component70>(68);
        unsafe {
            Context.AddSystem<Component69, Component70>(&Update<Component69, Component70>, 68);
        }
        _sets[68] = Context.PrepareSet(portion);

        Context.Warmup<Component70>(69 + 100);
        Context.Warmup<Component70, Component71>(69);
        unsafe {
            Context.AddSystem<Component70, Component71>(&Update<Component70, Component71>, 69);
        }
        _sets[69] = Context.PrepareSet(portion);

        Context.Warmup<Component71>(70 + 100);
        Context.Warmup<Component71, Component72>(70);
        unsafe {
            Context.AddSystem<Component71, Component72>(&Update<Component71, Component72>, 70);
        }
        _sets[70] = Context.PrepareSet(portion);

        Context.Warmup<Component72>(71 + 100);
        Context.Warmup<Component72, Component73>(71);
        unsafe {
            Context.AddSystem<Component72, Component73>(&Update<Component72, Component73>, 71);
        }
        _sets[71] = Context.PrepareSet(portion);

        Context.Warmup<Component73>(72 + 100);
        Context.Warmup<Component73, Component74>(72);
        unsafe {
            Context.AddSystem<Component73, Component74>(&Update<Component73, Component74>, 72);
        }
        _sets[72] = Context.PrepareSet(portion);

        Context.Warmup<Component74>(73 + 100);
        Context.Warmup<Component74, Component75>(73);
        unsafe {
            Context.AddSystem<Component74, Component75>(&Update<Component74, Component75>, 73);
        }
        _sets[73] = Context.PrepareSet(portion);

        Context.Warmup<Component75>(74 + 100);
        Context.Warmup<Component75, Component76>(74);
        unsafe {
            Context.AddSystem<Component75, Component76>(&Update<Component75, Component76>, 74);
        }
        _sets[74] = Context.PrepareSet(portion);

        Context.Warmup<Component76>(75 + 100);
        Context.Warmup<Component76, Component77>(75);
        unsafe {
            Context.AddSystem<Component76, Component77>(&Update<Component76, Component77>, 75);
        }
        _sets[75] = Context.PrepareSet(portion);

        Context.Warmup<Component77>(76 + 100);
        Context.Warmup<Component77, Component78>(76);
        unsafe {
            Context.AddSystem<Component77, Component78>(&Update<Component77, Component78>, 76);
        }
        _sets[76] = Context.PrepareSet(portion);

        Context.Warmup<Component78>(77 + 100);
        Context.Warmup<Component78, Component79>(77);
        unsafe {
            Context.AddSystem<Component78, Component79>(&Update<Component78, Component79>, 77);
        }
        _sets[77] = Context.PrepareSet(portion);

        Context.Warmup<Component79>(78 + 100);
        Context.Warmup<Component79, Component80>(78);
        unsafe {
            Context.AddSystem<Component79, Component80>(&Update<Component79, Component80>, 78);
        }
        _sets[78] = Context.PrepareSet(portion);

        Context.Warmup<Component80>(79 + 100);
        Context.Warmup<Component80, Component81>(79);
        unsafe {
            Context.AddSystem<Component80, Component81>(&Update<Component80, Component81>, 79);
        }
        _sets[79] = Context.PrepareSet(portion);

        Context.Warmup<Component81>(80 + 100);
        Context.Warmup<Component81, Component82>(80);
        unsafe {
            Context.AddSystem<Component81, Component82>(&Update<Component81, Component82>, 80);
        }
        _sets[80] = Context.PrepareSet(portion);

        Context.Warmup<Component82>(81 + 100);
        Context.Warmup<Component82, Component83>(81);
        unsafe {
            Context.AddSystem<Component82, Component83>(&Update<Component82, Component83>, 81);
        }
        _sets[81] = Context.PrepareSet(portion);

        Context.Warmup<Component83>(82 + 100);
        Context.Warmup<Component83, Component84>(82);
        unsafe {
            Context.AddSystem<Component83, Component84>(&Update<Component83, Component84>, 82);
        }
        _sets[82] = Context.PrepareSet(portion);

        Context.Warmup<Component84>(83 + 100);
        Context.Warmup<Component84, Component85>(83);
        unsafe {
            Context.AddSystem<Component84, Component85>(&Update<Component84, Component85>, 83);
        }
        _sets[83] = Context.PrepareSet(portion);

        Context.Warmup<Component85>(84 + 100);
        Context.Warmup<Component85, Component86>(84);
        unsafe {
            Context.AddSystem<Component85, Component86>(&Update<Component85, Component86>, 84);
        }
        _sets[84] = Context.PrepareSet(portion);

        Context.Warmup<Component86>(85 + 100);
        Context.Warmup<Component86, Component87>(85);
        unsafe {
            Context.AddSystem<Component86, Component87>(&Update<Component86, Component87>, 85);
        }
        _sets[85] = Context.PrepareSet(portion);

        Context.Warmup<Component87>(86 + 100);
        Context.Warmup<Component87, Component88>(86);
        unsafe {
            Context.AddSystem<Component87, Component88>(&Update<Component87, Component88>, 86);
        }
        _sets[86] = Context.PrepareSet(portion);

        Context.Warmup<Component88>(87 + 100);
        Context.Warmup<Component88, Component89>(87);
        unsafe {
            Context.AddSystem<Component88, Component89>(&Update<Component88, Component89>, 87);
        }
        _sets[87] = Context.PrepareSet(portion);

        Context.Warmup<Component89>(88 + 100);
        Context.Warmup<Component89, Component90>(88);
        unsafe {
            Context.AddSystem<Component89, Component90>(&Update<Component89, Component90>, 88);
        }
        _sets[88] = Context.PrepareSet(portion);

        Context.Warmup<Component90>(89 + 100);
        Context.Warmup<Component90, Component91>(89);
        unsafe {
            Context.AddSystem<Component90, Component91>(&Update<Component90, Component91>, 89);
        }
        _sets[89] = Context.PrepareSet(portion);

        Context.Warmup<Component91>(90 + 100);
        Context.Warmup<Component91, Component92>(90);
        unsafe {
            Context.AddSystem<Component91, Component92>(&Update<Component91, Component92>, 90);
        }
        _sets[90] = Context.PrepareSet(portion);

        Context.Warmup<Component92>(91 + 100);
        Context.Warmup<Component92, Component93>(91);
        unsafe {
            Context.AddSystem<Component92, Component93>(&Update<Component92, Component93>, 91);
        }
        _sets[91] = Context.PrepareSet(portion);

        Context.Warmup<Component93>(92 + 100);
        Context.Warmup<Component93, Component94>(92);
        unsafe {
            Context.AddSystem<Component93, Component94>(&Update<Component93, Component94>, 92);
        }
        _sets[92] = Context.PrepareSet(portion);

        Context.Warmup<Component94>(93 + 100);
        Context.Warmup<Component94, Component95>(93);
        unsafe {
            Context.AddSystem<Component94, Component95>(&Update<Component94, Component95>, 93);
        }
        _sets[93] = Context.PrepareSet(portion);

        Context.Warmup<Component95>(94 + 100);
        Context.Warmup<Component95, Component96>(94);
        unsafe {
            Context.AddSystem<Component95, Component96>(&Update<Component95, Component96>, 94);
        }
        _sets[94] = Context.PrepareSet(portion);

        Context.Warmup<Component96>(95 + 100);
        Context.Warmup<Component96, Component97>(95);
        unsafe {
            Context.AddSystem<Component96, Component97>(&Update<Component96, Component97>, 95);
        }
        _sets[95] = Context.PrepareSet(portion);

        Context.Warmup<Component97>(96 + 100);
        Context.Warmup<Component97, Component98>(96);
        unsafe {
            Context.AddSystem<Component97, Component98>(&Update<Component97, Component98>, 96);
        }
        _sets[96] = Context.PrepareSet(portion);

        Context.Warmup<Component98>(97 + 100);
        Context.Warmup<Component98, Component99>(97);
        unsafe {
            Context.AddSystem<Component98, Component99>(&Update<Component98, Component99>, 97);
        }
        _sets[97] = Context.PrepareSet(portion);

        Context.Warmup<Component99>(98 + 100);
        Context.Warmup<Component99, Component100>(98);
        unsafe {
            Context.AddSystem<Component99, Component100>(&Update<Component99, Component100>, 98);
        }
        _sets[98] = Context.PrepareSet(portion);

        Context.Warmup<Component100>(99 + 100);
        Context.Warmup<Component100, Component1>(99);
        unsafe {
            Context.AddSystem<Component100, Component1>(&Update<Component100, Component1>, 99);
        }
        _sets[99] = Context.PrepareSet(EntityCount - portion * (100 - 1));

        Context.FinishSetup();
    }

    [IterationSetup]
    public void IterationSetup() {
        Context.CreateEntities<Component1>(_sets[0], 0 + 100, default(Component1));
        Context.CreateEntities<Component2>(_sets[1], 1 + 100, default(Component2));
        Context.CreateEntities<Component3>(_sets[2], 2 + 100, default(Component3));
        Context.CreateEntities<Component4>(_sets[3], 3 + 100, default(Component4));
        Context.CreateEntities<Component5>(_sets[4], 4 + 100, default(Component5));
        Context.CreateEntities<Component6>(_sets[5], 5 + 100, default(Component6));
        Context.CreateEntities<Component7>(_sets[6], 6 + 100, default(Component7));
        Context.CreateEntities<Component8>(_sets[7], 7 + 100, default(Component8));
        Context.CreateEntities<Component9>(_sets[8], 8 + 100, default(Component9));
        Context.CreateEntities<Component10>(_sets[9], 9 + 100, default(Component10));
        Context.CreateEntities<Component11>(_sets[10], 10 + 100, default(Component11));
        Context.CreateEntities<Component12>(_sets[11], 11 + 100, default(Component12));
        Context.CreateEntities<Component13>(_sets[12], 12 + 100, default(Component13));
        Context.CreateEntities<Component14>(_sets[13], 13 + 100, default(Component14));
        Context.CreateEntities<Component15>(_sets[14], 14 + 100, default(Component15));
        Context.CreateEntities<Component16>(_sets[15], 15 + 100, default(Component16));
        Context.CreateEntities<Component17>(_sets[16], 16 + 100, default(Component17));
        Context.CreateEntities<Component18>(_sets[17], 17 + 100, default(Component18));
        Context.CreateEntities<Component19>(_sets[18], 18 + 100, default(Component19));
        Context.CreateEntities<Component20>(_sets[19], 19 + 100, default(Component20));
        Context.CreateEntities<Component21>(_sets[20], 20 + 100, default(Component21));
        Context.CreateEntities<Component22>(_sets[21], 21 + 100, default(Component22));
        Context.CreateEntities<Component23>(_sets[22], 22 + 100, default(Component23));
        Context.CreateEntities<Component24>(_sets[23], 23 + 100, default(Component24));
        Context.CreateEntities<Component25>(_sets[24], 24 + 100, default(Component25));
        Context.CreateEntities<Component26>(_sets[25], 25 + 100, default(Component26));
        Context.CreateEntities<Component27>(_sets[26], 26 + 100, default(Component27));
        Context.CreateEntities<Component28>(_sets[27], 27 + 100, default(Component28));
        Context.CreateEntities<Component29>(_sets[28], 28 + 100, default(Component29));
        Context.CreateEntities<Component30>(_sets[29], 29 + 100, default(Component30));
        Context.CreateEntities<Component31>(_sets[30], 30 + 100, default(Component31));
        Context.CreateEntities<Component32>(_sets[31], 31 + 100, default(Component32));
        Context.CreateEntities<Component33>(_sets[32], 32 + 100, default(Component33));
        Context.CreateEntities<Component34>(_sets[33], 33 + 100, default(Component34));
        Context.CreateEntities<Component35>(_sets[34], 34 + 100, default(Component35));
        Context.CreateEntities<Component36>(_sets[35], 35 + 100, default(Component36));
        Context.CreateEntities<Component37>(_sets[36], 36 + 100, default(Component37));
        Context.CreateEntities<Component38>(_sets[37], 37 + 100, default(Component38));
        Context.CreateEntities<Component39>(_sets[38], 38 + 100, default(Component39));
        Context.CreateEntities<Component40>(_sets[39], 39 + 100, default(Component40));
        Context.CreateEntities<Component41>(_sets[40], 40 + 100, default(Component41));
        Context.CreateEntities<Component42>(_sets[41], 41 + 100, default(Component42));
        Context.CreateEntities<Component43>(_sets[42], 42 + 100, default(Component43));
        Context.CreateEntities<Component44>(_sets[43], 43 + 100, default(Component44));
        Context.CreateEntities<Component45>(_sets[44], 44 + 100, default(Component45));
        Context.CreateEntities<Component46>(_sets[45], 45 + 100, default(Component46));
        Context.CreateEntities<Component47>(_sets[46], 46 + 100, default(Component47));
        Context.CreateEntities<Component48>(_sets[47], 47 + 100, default(Component48));
        Context.CreateEntities<Component49>(_sets[48], 48 + 100, default(Component49));
        Context.CreateEntities<Component50>(_sets[49], 49 + 100, default(Component50));
        Context.CreateEntities<Component51>(_sets[50], 50 + 100, default(Component51));
        Context.CreateEntities<Component52>(_sets[51], 51 + 100, default(Component52));
        Context.CreateEntities<Component53>(_sets[52], 52 + 100, default(Component53));
        Context.CreateEntities<Component54>(_sets[53], 53 + 100, default(Component54));
        Context.CreateEntities<Component55>(_sets[54], 54 + 100, default(Component55));
        Context.CreateEntities<Component56>(_sets[55], 55 + 100, default(Component56));
        Context.CreateEntities<Component57>(_sets[56], 56 + 100, default(Component57));
        Context.CreateEntities<Component58>(_sets[57], 57 + 100, default(Component58));
        Context.CreateEntities<Component59>(_sets[58], 58 + 100, default(Component59));
        Context.CreateEntities<Component60>(_sets[59], 59 + 100, default(Component60));
        Context.CreateEntities<Component61>(_sets[60], 60 + 100, default(Component61));
        Context.CreateEntities<Component62>(_sets[61], 61 + 100, default(Component62));
        Context.CreateEntities<Component63>(_sets[62], 62 + 100, default(Component63));
        Context.CreateEntities<Component64>(_sets[63], 63 + 100, default(Component64));
        Context.CreateEntities<Component65>(_sets[64], 64 + 100, default(Component65));
        Context.CreateEntities<Component66>(_sets[65], 65 + 100, default(Component66));
        Context.CreateEntities<Component67>(_sets[66], 66 + 100, default(Component67));
        Context.CreateEntities<Component68>(_sets[67], 67 + 100, default(Component68));
        Context.CreateEntities<Component69>(_sets[68], 68 + 100, default(Component69));
        Context.CreateEntities<Component70>(_sets[69], 69 + 100, default(Component70));
        Context.CreateEntities<Component71>(_sets[70], 70 + 100, default(Component71));
        Context.CreateEntities<Component72>(_sets[71], 71 + 100, default(Component72));
        Context.CreateEntities<Component73>(_sets[72], 72 + 100, default(Component73));
        Context.CreateEntities<Component74>(_sets[73], 73 + 100, default(Component74));
        Context.CreateEntities<Component75>(_sets[74], 74 + 100, default(Component75));
        Context.CreateEntities<Component76>(_sets[75], 75 + 100, default(Component76));
        Context.CreateEntities<Component77>(_sets[76], 76 + 100, default(Component77));
        Context.CreateEntities<Component78>(_sets[77], 77 + 100, default(Component78));
        Context.CreateEntities<Component79>(_sets[78], 78 + 100, default(Component79));
        Context.CreateEntities<Component80>(_sets[79], 79 + 100, default(Component80));
        Context.CreateEntities<Component81>(_sets[80], 80 + 100, default(Component81));
        Context.CreateEntities<Component82>(_sets[81], 81 + 100, default(Component82));
        Context.CreateEntities<Component83>(_sets[82], 82 + 100, default(Component83));
        Context.CreateEntities<Component84>(_sets[83], 83 + 100, default(Component84));
        Context.CreateEntities<Component85>(_sets[84], 84 + 100, default(Component85));
        Context.CreateEntities<Component86>(_sets[85], 85 + 100, default(Component86));
        Context.CreateEntities<Component87>(_sets[86], 86 + 100, default(Component87));
        Context.CreateEntities<Component88>(_sets[87], 87 + 100, default(Component88));
        Context.CreateEntities<Component89>(_sets[88], 88 + 100, default(Component89));
        Context.CreateEntities<Component90>(_sets[89], 89 + 100, default(Component90));
        Context.CreateEntities<Component91>(_sets[90], 90 + 100, default(Component91));
        Context.CreateEntities<Component92>(_sets[91], 91 + 100, default(Component92));
        Context.CreateEntities<Component93>(_sets[92], 92 + 100, default(Component93));
        Context.CreateEntities<Component94>(_sets[93], 93 + 100, default(Component94));
        Context.CreateEntities<Component95>(_sets[94], 94 + 100, default(Component95));
        Context.CreateEntities<Component96>(_sets[95], 95 + 100, default(Component96));
        Context.CreateEntities<Component97>(_sets[96], 96 + 100, default(Component97));
        Context.CreateEntities<Component98>(_sets[97], 97 + 100, default(Component98));
        Context.CreateEntities<Component99>(_sets[98], 98 + 100, default(Component99));
        Context.CreateEntities<Component100>(_sets[99], 99 + 100, default(Component100));
    }

    [Benchmark]
    public void Run() {
        Context.Tick(Delta);
    }

    [IterationCleanup]
    public void IterationCleanup() {
        Context.DeleteEntities(_sets[0]);
        Context.DeleteEntities(_sets[1]);
        Context.DeleteEntities(_sets[2]);
        Context.DeleteEntities(_sets[3]);
        Context.DeleteEntities(_sets[4]);
        Context.DeleteEntities(_sets[5]);
        Context.DeleteEntities(_sets[6]);
        Context.DeleteEntities(_sets[7]);
        Context.DeleteEntities(_sets[8]);
        Context.DeleteEntities(_sets[9]);
        Context.DeleteEntities(_sets[10]);
        Context.DeleteEntities(_sets[11]);
        Context.DeleteEntities(_sets[12]);
        Context.DeleteEntities(_sets[13]);
        Context.DeleteEntities(_sets[14]);
        Context.DeleteEntities(_sets[15]);
        Context.DeleteEntities(_sets[16]);
        Context.DeleteEntities(_sets[17]);
        Context.DeleteEntities(_sets[18]);
        Context.DeleteEntities(_sets[19]);
        Context.DeleteEntities(_sets[20]);
        Context.DeleteEntities(_sets[21]);
        Context.DeleteEntities(_sets[22]);
        Context.DeleteEntities(_sets[23]);
        Context.DeleteEntities(_sets[24]);
        Context.DeleteEntities(_sets[25]);
        Context.DeleteEntities(_sets[26]);
        Context.DeleteEntities(_sets[27]);
        Context.DeleteEntities(_sets[28]);
        Context.DeleteEntities(_sets[29]);
        Context.DeleteEntities(_sets[30]);
        Context.DeleteEntities(_sets[31]);
        Context.DeleteEntities(_sets[32]);
        Context.DeleteEntities(_sets[33]);
        Context.DeleteEntities(_sets[34]);
        Context.DeleteEntities(_sets[35]);
        Context.DeleteEntities(_sets[36]);
        Context.DeleteEntities(_sets[37]);
        Context.DeleteEntities(_sets[38]);
        Context.DeleteEntities(_sets[39]);
        Context.DeleteEntities(_sets[40]);
        Context.DeleteEntities(_sets[41]);
        Context.DeleteEntities(_sets[42]);
        Context.DeleteEntities(_sets[43]);
        Context.DeleteEntities(_sets[44]);
        Context.DeleteEntities(_sets[45]);
        Context.DeleteEntities(_sets[46]);
        Context.DeleteEntities(_sets[47]);
        Context.DeleteEntities(_sets[48]);
        Context.DeleteEntities(_sets[49]);
        Context.DeleteEntities(_sets[50]);
        Context.DeleteEntities(_sets[51]);
        Context.DeleteEntities(_sets[52]);
        Context.DeleteEntities(_sets[53]);
        Context.DeleteEntities(_sets[54]);
        Context.DeleteEntities(_sets[55]);
        Context.DeleteEntities(_sets[56]);
        Context.DeleteEntities(_sets[57]);
        Context.DeleteEntities(_sets[58]);
        Context.DeleteEntities(_sets[59]);
        Context.DeleteEntities(_sets[60]);
        Context.DeleteEntities(_sets[61]);
        Context.DeleteEntities(_sets[62]);
        Context.DeleteEntities(_sets[63]);
        Context.DeleteEntities(_sets[64]);
        Context.DeleteEntities(_sets[65]);
        Context.DeleteEntities(_sets[66]);
        Context.DeleteEntities(_sets[67]);
        Context.DeleteEntities(_sets[68]);
        Context.DeleteEntities(_sets[69]);
        Context.DeleteEntities(_sets[70]);
        Context.DeleteEntities(_sets[71]);
        Context.DeleteEntities(_sets[72]);
        Context.DeleteEntities(_sets[73]);
        Context.DeleteEntities(_sets[74]);
        Context.DeleteEntities(_sets[75]);
        Context.DeleteEntities(_sets[76]);
        Context.DeleteEntities(_sets[77]);
        Context.DeleteEntities(_sets[78]);
        Context.DeleteEntities(_sets[79]);
        Context.DeleteEntities(_sets[80]);
        Context.DeleteEntities(_sets[81]);
        Context.DeleteEntities(_sets[82]);
        Context.DeleteEntities(_sets[83]);
        Context.DeleteEntities(_sets[84]);
        Context.DeleteEntities(_sets[85]);
        Context.DeleteEntities(_sets[86]);
        Context.DeleteEntities(_sets[87]);
        Context.DeleteEntities(_sets[88]);
        Context.DeleteEntities(_sets[89]);
        Context.DeleteEntities(_sets[90]);
        Context.DeleteEntities(_sets[91]);
        Context.DeleteEntities(_sets[92]);
        Context.DeleteEntities(_sets[93]);
        Context.DeleteEntities(_sets[94]);
        Context.DeleteEntities(_sets[95]);
        Context.DeleteEntities(_sets[96]);
        Context.DeleteEntities(_sets[97]);
        Context.DeleteEntities(_sets[98]);
        Context.DeleteEntities(_sets[99]);
    }

    [GlobalCleanup]
    public void GlobalCleanup() {
        Context.Cleanup();
        Context.Dispose();
        Context = default(T)!;
    }


    private static void Update<T1, T2>(ref T1 c1, ref T2 c2) where T1 : struct where T2 : struct { }
}
