# other-ecs-benchmarks

alternative to https://github.com/Doraku/Ecs.CSharp.Benchmark

# Latest run

updated with actions and can be found [here](https://gist.github.com/blackbone/6d254a684cf580441bf58690ad9485c3)

# What is all about?

General idea is to hide implementation of each ECS under context abstraction and work with it from benchmark
implementations.

Benchmarks design follow 2 rules which I try to balance with:
* **Strict usage** - to ensure all benchmarks are running with same flow to avoid cheating.
* **Features utilization** - to allow implementations to run in perfomant way.

General flow of any benchmark execution is divided into 3 steps:
* Preparation
  * Creating world
  * Creating initial entities if needed
  * Initialize filters and queries or other stuff which used to gain perfomance
* Benchmark call
  * Aquiring lock of world
  * Run main logic
  * Commiting changes
* Cleanup - mostly omited

> [!IMPORTANT]
> Don't search truth here. There won't be any.

# Implemented contexts

|                                                             ECS | Version                                                                                           | Implemented | Verified |
|----------------------------------------------------------------:|:--------------------------------------------------------------------------------------------------|-------------|----------|
|                         [Arch](https://github.com/genaray/Arch) | [1.2.8](https://www.nuget.org/packages/Arch/1.2.8)                                                | ✅           | ❌        |
|                                [fennecs](https://fennecs.tech/) | [0.4.2-beta](https://www.nuget.org/packages/fennecs/0.4.2-beta)                                   | ✅           | ❌        |
|                    [Morpeh](https://github.com/scellecs/morpeh) | [2023.1.0](https://www.nuget.org/packages/Scellecs.Morpeh/2023.1.0)                               | ✅           | ❌        |
|            [DragonECS](https://github.com/DCFApixels/DragonECS) | [0.8.36](https://github.com/DCFApixels/DragonECS/commit/29f656f394984e738c7fc70bacca050ffea746d8) | ✅           | ❌        |
| [LeoECS (Unoficcial NuGet)](https://github.com/scellecs/morpeh) | [1.0.1](https://www.nuget.org/packages/Leopotam.Ecs/1.0.1)                                        | ✅           | ❌        |

# Implemented benchmarks

| Benchmark                       | Description                                       |
|---------------------------------|---------------------------------------------------|
| Create Empty Entity             | Creates [EntityCount] empty entities              |
| Create Entity With N Components | Creates [EntityCount] entitites with N components |
| Add N Components                | Adds N components to [EntityCount] entities       |
| Remove N Components             | Adds N components to [EntityCount] entities       |

# Running

Just call `Benchmark.sh` from terminal.

Command line args:

| arg       |            description             | sample                                        |
|-----------|:----------------------------------:|-----------------------------------------------|
| benchmark | allow to specify benchmarks to run | `benchmarks=CreateEmtyEntities,Add1Component` |
| contexts  |  allow to specify contexts to run  | `contexts=Morpeh,Fennecs,...`                 |

> Since all comparisons is made by string contains you can simply write something like `contexts=Morpeh`
> instead of `context=MorpehContext`
> and `benchmarks=With1,With2` to launch subset of benchmarks.
> Selected benchmarks and contexts will be logged to console.

# Contribution

- Fork
- Implement
- Create PR

# Problems

1. Because of nature of BenchmarkDotNet there's sequential iteration of creating entities happening.
This leads to case where, for example we creating 100k entities in benchmark, it's properly cleared
in Setup and Cleanup but benchmark itself will be called multiple times which will lead to creating
2. 100k entities,
then another 100k and in some cases lead to millions of entities in the world which can affect perfomance of creation
and deletion on certain ECS implementations.
2. System benchmarks which uses *Padding* property produces up to 1.100.000 entities each because of logic of padding
generation. It affects runs duration but for now i'm not sure about correct way do fix that (maybe keep entire entities
count up to *EntityCount* so it'll not affect speed but it'll reduce actual entity count to about 9.9k so archetype ecs
implementation will gain significant boost).
