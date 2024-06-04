using System;
using Benchmark.AppCoreKostyl;
using Benchmark.Benchmarks.Entities.AddComponent;
using Benchmark.Benchmarks.Entities.CreateEntity;
using Benchmark.Benchmarks.Entities.DeleteEntity;
using Benchmark.Benchmarks.Entities.RemoveComponent;
using Benchmark.Benchmarks.Systems;
using Benchmark.KremAppCore;

namespace Benchmark;

public static class Sister
{
    private static readonly Type[] SisterBenchmarks = [
        // add
        typeof(Add1Component_Sister),
        typeof(Add1ComponentRandomOrder_Sister),
        typeof(Add1RandomComponent_Sister),
        typeof(Add2Components_Sister),
        typeof(Add2ComponentsRandomOrder_Sister),
        typeof(Add2RandomComponents_Sister),
        typeof(Add3Components_Sister),
        typeof(Add3ComponentsRandomOrder_Sister),
        typeof(Add3RandomComponents_Sister),
        typeof(Add4Components_Sister),
        typeof(Add4ComponentsRandomOrder_Sister),
        
        // create
        typeof(CreateEmptyEntity_Sister),
        typeof(CreateEntityWith1Component_Sister),
        typeof(CreateEntityWith1RandomComponent_Sister),
        typeof(CreateEntityWith2Components_Sister),
        typeof(CreateEntityWith2RandomComponents_Sister),
        typeof(CreateEntityWith3Components_Sister),
        typeof(CreateEntityWith3RandomComponents_Sister),
        typeof(CreateEntityWith4Components_Sister),
        
        // delete
        typeof(DeleteEntity_Sister),
        
        // remove
        typeof(Remove1Component_Sister),
        typeof(Remove1ComponentRandomOrder_Sister),
        typeof(Remove2Components_Sister),
        typeof(Remove2ComponentsRandomOrder_Sister),
        typeof(Remove3Components_Sister),
        typeof(Remove3ComponentsRandomOrder_Sister),
        typeof(Remove4Components_Sister),
        typeof(Remove4ComponentsRandomOrder_Sister),
        
        // system
        typeof(SystemWith1Component_Sister),
        typeof(SystemWith1ComponentMultipleComposition_Sister),
        typeof(SystemWith2Components_Sister),
        typeof(SystemWith2ComponentsMultipleComposition_Sister),
        typeof(SystemWith3Components_Sister),
        typeof(SystemWith3ComponentsMultipleComposition_Sister)
    ];
    
    public static void InjectBenchmarks() { /* no op */}
    static Sister()
    {
        // inject ctx benches
        BenchMap.Contexts.Add(typeof(SisterContext), SisterBenchmarks);
        
        // inject bench ctx
        // add
        Add(typeof(Add1Component<>), typeof(Add1Component_Sister));
        Add(typeof(Add1ComponentRandomOrder<>), typeof(Add1ComponentRandomOrder_Sister));
        Add(typeof(Add1RandomComponent<>), typeof(Add1RandomComponent_Sister));
        Add(typeof(Add2Components<>), typeof(Add2Components_Sister));
        Add(typeof(Add2ComponentsRandomOrder<>), typeof(Add2ComponentsRandomOrder_Sister));
        Add(typeof(Add2RandomComponents<>), typeof(Add2RandomComponents_Sister));
        Add(typeof(Add3Components<>), typeof(Add3Components_Sister));
        Add(typeof(Add3ComponentsRandomOrder<>), typeof(Add3ComponentsRandomOrder_Sister));
        Add(typeof(Add3RandomComponents<>), typeof(Add3RandomComponents_Sister));
        Add(typeof(Add4Components<>), typeof(Add4Components_Sister));
        Add(typeof(Add4ComponentsRandomOrder<>), typeof(Add4ComponentsRandomOrder_Sister));
        
        // create
        Add(typeof(CreateEmptyEntity<>), typeof(CreateEmptyEntity_Sister));
        Add(typeof(CreateEntityWith1Component<>), typeof(CreateEntityWith1Component_Sister));
        Add(typeof(CreateEntityWith1RandomComponent<>), typeof(CreateEntityWith1RandomComponent_Sister));
        Add(typeof(CreateEntityWith2Components<>), typeof(CreateEntityWith2Components_Sister));
        Add(typeof(CreateEntityWith2RandomComponents<>), typeof(CreateEntityWith2RandomComponents_Sister));
        Add(typeof(CreateEntityWith3Components<>), typeof(CreateEntityWith3Components_Sister));
        Add(typeof(CreateEntityWith3RandomComponents<>), typeof(CreateEntityWith3RandomComponents_Sister));
        Add(typeof(CreateEntityWith4Components<>), typeof(CreateEntityWith4Components_Sister));
        
        // delete
        Add(typeof(DeleteEntity<>), typeof(DeleteEntity_Sister));
        
        // remove
        Add(typeof(Remove1Component<>), typeof(Remove1Component_Sister));
        Add(typeof(Remove1ComponentRandomOrder<>), typeof(Remove1ComponentRandomOrder_Sister));
        Add(typeof(Remove2Components<>), typeof(Remove2Components_Sister));
        Add(typeof(Remove2ComponentsRandomOrder<>), typeof(Remove2ComponentsRandomOrder_Sister));
        Add(typeof(Remove3Components<>), typeof(Remove3Components_Sister));
        Add(typeof(Remove3ComponentsRandomOrder<>), typeof(Remove3ComponentsRandomOrder_Sister));
        Add(typeof(Remove4Components<>), typeof(Remove4Components_Sister));
        Add(typeof(Remove4ComponentsRandomOrder<>), typeof(Remove4ComponentsRandomOrder_Sister));
        
        // system
        Add(typeof(SystemWith1Component<>), typeof(SystemWith1Component_Sister));
        Add(typeof(SystemWith1ComponentMultipleComposition<>), typeof(SystemWith1ComponentMultipleComposition_Sister));
        Add(typeof(SystemWith2Components<>), typeof(SystemWith2Components_Sister));
        Add(typeof(SystemWith2ComponentsMultipleComposition<>), typeof(SystemWith2ComponentsMultipleComposition_Sister));
        Add(typeof(SystemWith3Components<>), typeof(SystemWith3Components_Sister));
        Add(typeof(SystemWith3ComponentsMultipleComposition<>), typeof(SystemWith3ComponentsMultipleComposition_Sister));
    }

    private static void Add(Type bench, Type impl) => BenchMap.Runs[bench] = BenchMap.Runs[bench].With(impl);
}