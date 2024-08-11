> [!WARNING]
> Benchmark is under active development all API, integrations and set of benchmarks is subject to change!
>

# Latest run

[![Test](https://github.com/blackbone/other-ecs-benchmarks/actions/workflows/test.yml/badge.svg)](https://github.com/blackbone/other-ecs-benchmarks/actions/workflows/test.yml)
[![Run parallel benchmarks](https://github.com/blackbone/other-ecs-benchmarks/actions/workflows/benchmark.yml/badge.svg)](https://github.com/blackbone/other-ecs-benchmarks/actions/workflows/benchmark.yml)

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

|                                                                 ECS | Version                                                                                           |  Implemented  |  Verified  |       Notes       |
|--------------------------------------------------------------------:|:--------------------------------------------------------------------------------------------------|:-------------:|:----------:|:-----------------:|
|                             [Arch](https://github.com/genaray/Arch) | [1.2.8.2-alpha](https://www.nuget.org/packages/Arch/1.2.8.2-alpha)                                |       ✅       |     ❌      |        N/A        |
|                                    [fennecs](https://fennecs.tech/) | [0.5.10-beta](https://www.nuget.org/packages/fennecs/0.5.10-beta)                                 |       ✅       |     ❌      |        N/A        |
|                        [Morpeh](https://github.com/scellecs/morpeh) | [2024.1.0-rc49](https://github.com/scellecs/morpeh/releases/tag/2024.1.0-rc49)                    |       ✅       |     ❌      |        N/A        |
|                [DragonECS](https://github.com/DCFApixels/DragonECS) | [0.8.41](https://github.com/DCFApixels/DragonECS/commit/dc05867f929aa86bae998f65aa4d11df2848c2fd) |       ✅       |     ❌      |        N/A        |
|                           [LeoECS](https://github.com/Leopotam/ecs) | [2023.6.22](https://github.com/Leopotam/ecs/releases/tag/2023.6.22)                               |       ✅       |     ❌      |        N/A        |
|                   [LeoECSLite](https://github.com/Leopotam/ecslite) | [2024.5.22](https://github.com/Leopotam/ecslite/releases/tag/2024.5.22)                           |       ✅       |     ❌      |        N/A        |
|                  [DefaultECS](https://github.com/Doraku/DefaultEcs) | [0.18.0-beta01](https://github.com/Doraku/DefaultEcs/releases/tag/0.18.0-beta01)                  |       ✅       |     ❌      |  Analyzer 0.17.8  |
|          [FlecsNET](https://github.com/BeanCheeseBurrito/Flecs.NET) | [4.0.0](https://www.nuget.org/packages/Flecs.NET.Release/4.0.0)                                   |       ✅       |     ❌      |        N/A        |
|                 [TinyEcs](https://github.com/andreakarasho/TinyEcs) | [1.3.0](https://www.nuget.org/packages/TinyEcs.Main/1.3.0)                                        |       ✅       |     ❌      |        N/A        |
|                           [Xeno](https://github.com/blackbone/xeno) | [0.1.1](https://github.com/blackbone/xeno/commit/04ba1abe218868a38d55123386a387aa95fc27af)        |       ✅       |     ✅      |        N/A        |
|               [FriFlo](https://github.com/friflo/Friflo.Engine.ECS) | [3.0.0-preview.11](https://www.nuget.org/packages/Friflo.Engine.ECS/3.0.0-preview.11)             |       ✅       |     ❌      |        N/A        |

# Implemented benchmarks

| Benchmark                                     | Description                                                             |
|-----------------------------------------------|-------------------------------------------------------------------------|
| Create Empty Entity                           | Creates [EntityCount] empty entities                                    |
| Create Entity With N Components               | Creates [EntityCount] entitites with N components                       |
| Add N Components                              | Adds N components to [EntityCount] entities                             |
| Remove N Components                           | Adds N components to [EntityCount] entities                             |
| System with N Components                      | Performs simple operations on entities (numbers sum)                    |
| System with N Components Multiple Composition | Same as *System with N Components* but with mixture of other components |

# Running

Just call `Benchmark.sh` from terminal.

Command line args:

| arg        |               description                | sample                                         |
|------------|:----------------------------------------:|------------------------------------------------|
| benchmark  | allow to specify single benchmark to run | `benchmarks=CreateEmptyEntities,Add1Component` |
| benchmarks |    allow to specify benchmarks to run    | `benchmark=CreateEmptyEntities`                |
| contexts   |     allow to specify contexts to run     | `contexts=Morpeh,Fennecs,...`                  |
| --list     |        prints all benchmarks name        | `--list`                                       |

> Since all comparisons is made by string contains you can simply write something like `contexts=Morpeh`
> instead of `context=MorpehContext`
> and `benchmarks=With1,With2` to launch subset of benchmarks.
> Selected benchmarks and contexts will be logged to console.
> BUT benchmark arg requires exact name match with those printed with `--list`

# Contribution

- Fork
- Implement
- Create PR

# Problems

1. Because of nature of BenchmarkDotNet there's sequential iteration of creating entities happening.
   This leads to case where, for example we creating 100k entities in benchmark, it's properly cleared
   in Setup and Cleanup but benchmark itself will be called multiple times which will lead to creating 100k entities,
   then another 100k and in some cases lead to millions of entities in the world which can affect perfomance of creation
   and deletion on certain ECS implementations.
2. System benchmarks which uses *Padding* property produces up to 1.100.000 entities each because of logic of padding
   generation. It affects runs duration but for now i'm not sure about correct way do fix that (maybe keep entire
   entities
   count up to *EntityCount* so it'll not affect speed but it'll reduce actual entity count to about 9.9k so archetype
   ecs
   implementation will gain significant boost).
3. Because some framework deleting entity on delete last components there are differences in behaviours in tests and
   benchmarks.
   For example **RemoveComponent** benchmark will work faster with Arch and FennECS because they're not deleting entity.
   Because of that special property called `DeletesEntityOnLastComponentDeletion` is required to be implemented in each
   context.
4. **TinyECS** - Deleting entities during lock causing stackoverflow on merge so get rid of lock during deletions.
