using Krem.AppCore.Runtime.Core.Interfaces;
using Krem.AppCore.Runtime.Implementation;
using Krem.AppCore.Runtime.Implementation.Builders;
using Krem.AppCore.Runtime.Implementation.Contracts;
using Krem.AppCore.Runtime.Implementation.Executors;
using Krem.AppCore.Runtime.Implementation.Providers;
using Krem.AppCore.Runtime.Implementation.States.AppStates;

namespace Benchmark.KremAppCore;

public class SisterContext(int entityCount = 4096)
{
    private readonly Dictionary<int, Filter>? _filters = new();
    private readonly List<Executor>? _executors = new();
    private World? _world;
    private EntityProvider? _entityProvider;
    private ExecutorProvider? _executorProvider;
    
    public bool DeletesEntityOnLastComponentDeletion => false;
    public int EntityCount => _entityProvider!.GetAll().Count;
    public void Setup()
    {
        if (AppCore.App == null)
            AppCore.Construct();
        
        _world = new World(AppCore.App!.WorldContainer);
        _world.AddProvider<EventBusProviderContract, FastEventBusProvider>();
        _executorProvider = _world.AddProvider<ExecutorProviderContract, ExecutorProvider>();
        _entityProvider = _world.AddProvider<EntityProviderContract, EntityProvider>();
    }

    public void FinishSetup()
    {
        AppCore.App.AppStateMachine
            // Производим инжект
            .AddState(new InjectServicesToServicesState())
            .AddState(new InjectServicesToWorldsState())
            .AddState(new InjectProvidersToWorldsState())
            .AddState(new InjectServicesToProvidersState())
            .AddState(new InjectProvidersToProvidersState())
            .AddState(new InjectServicesToExecutorsState())
            .AddState(new InjectProvidersToExecutorsState())
            .AddState(new InjectFiltersToExecutorsState())
            // Инициализируем
            .AddState(new InitializeServiceContainerState())
            .AddState(new InitializeWorldContainerState());

        AppCore.Initialize();
    }

    public void Warmup<T1>(in int poolId) where T1 : Component
        => _filters![poolId] = FilterBuilder.Include<T1>().Create(_world!.Container);

    public void Warmup<T1, T2>(in int poolId) where T1 : Component where T2 : Component
        => _filters![poolId] = FilterBuilder.Include<T1>().Include<T2>().Create(_world!.Container);

    public void Warmup<T1, T2, T3>(in int poolId) where T1 : Component where T2 : Component where T3 : Component
        => _filters![poolId] = FilterBuilder.Include<T1>().Include<T2>().Include<T3>().Create(_world!.Container);

    public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : Component where T2 : Component where T3 : Component where T4 : Component
        => _filters![poolId] = FilterBuilder.Include<T1>().Include<T2>().Include<T3>().Include<T4>().Create(_world!.Container);

    public void Cleanup()
    {
        _filters!.Clear();
        _executors!.Clear();
        _entityProvider!.RemoveAll();
        _entityProvider!.Dispose();
        _world!.Dispose();
    }

    public void Lock()
    {
        /* no op */
    }

    public void Commit()
    {
        /* no op */
    }

    public void DeleteEntities(in Array entitySet)
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            if (entities[i] != null) _entityProvider!.Destruct(entities[i]);
    }

    public Array Shuffle(in Array entitySet)
    {
        Random.Shared.Shuffle((Entity?[])entitySet);
        return entitySet;
    }

    public Array PrepareSet(in int count) => new Entity[count];

    public void CreateEntities(in Array entitySet)
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
            entities[i] = _entityProvider!.Create();
    }

    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default) where T1 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = _entityProvider!.Create();
            var ec1 = e.Add<T1>(); if (c1 != null) ec1.Deserialize(c1.Serialize());
            entities[i] = e;
        }
    }

    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default) where T1 : Component where T2 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = _entityProvider!.Create();
            var ec1 = e.Add<T1>(); if (c1 != null) ec1.Deserialize(c1.Serialize());
            var ec2 = e.Add<T2>(); if (c2 != null) ec2.Deserialize(c2.Serialize());
            entities[i] = e;
        }
    }

    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : Component where T2 : Component where T3 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = _entityProvider!.Create();
            var ec1 = e.Add<T1>(); if (c1 != null) ec1.Deserialize(c1.Serialize());
            var ec2 = e.Add<T2>(); if (c2 != null) ec2.Deserialize(c2.Serialize());
            var ec3 = e.Add<T3>(); if (c3 != null) ec3.Deserialize(c3.Serialize());
            entities[i] = e;
        }
    }

    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : Component where T2 : Component where T3 : Component where T4 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = _entityProvider!.Create();
            var ec1 = e.Add<T1>(); if (c1 != null) ec1.Deserialize(c1.Serialize());
            var ec2 = e.Add<T2>(); if (c2 != null) ec2.Deserialize(c2.Serialize());
            var ec3 = e.Add<T3>(); if (c3 != null) ec3.Deserialize(c3.Serialize());
            var ec4 = e.Add<T4>(); if (c3 != null) ec4.Deserialize(c4.Serialize());
            entities[i] = e;
        }
    }

    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default) where T1 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            var ec1 = e.Add<T1>(); if (c1 != null) ec1.Deserialize(c1.Serialize());
        }
    }

    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default) where T1 : Component where T2 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            var ec1 = e.Add<T1>(); if (c1 != null) ec1.Deserialize(c1.Serialize());
            var ec2 = e.Add<T2>(); if (c2 != null) ec2.Deserialize(c2.Serialize());
        }
    }

    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default) where T1 : Component where T2 : Component where T3 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            var ec1 = e.Add<T1>(); if (c1 != null) ec1.Deserialize(c1.Serialize());
            var ec2 = e.Add<T2>(); if (c2 != null) ec2.Deserialize(c2.Serialize());
            var ec3 = e.Add<T3>(); if (c3 != null) ec3.Deserialize(c3.Serialize());
        }
    }

    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default,
        in T3 c3 = default, in T4 c4 = default) where T1 : Component where T2 : Component where T3 : Component where T4 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            var ec1 = e.Add<T1>(); if (c1 != null) ec1.Deserialize(c1.Serialize());
            var ec2 = e.Add<T2>(); if (c2 != null) ec2.Deserialize(c2.Serialize());
            var ec3 = e.Add<T3>(); if (c3 != null) ec3.Deserialize(c3.Serialize());
            var ec4 = e.Add<T4>(); if (c4 != null) ec4.Deserialize(c4.Serialize());
        }
    }

    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1) where T1 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            if (e.Has<T1>()) e.Remove<T1>();
        }
    }

    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1) where T1 : Component where T2 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            if (e.Has<T1>()) e.Remove<T1>();
            if (e.Has<T2>()) e.Remove<T2>();
        }
    }

    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1) where T1 : Component where T2 : Component where T3 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            if (e.Has<T1>()) e.Remove<T1>();
            if (e.Has<T2>()) e.Remove<T2>();
            if (e.Has<T3>()) e.Remove<T3>();
        }
    }

    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1) where T1 : Component where T2 : Component where T3 : Component where T4 : Component
    {
        var entities = (Entity?[])entitySet;
        for (var i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            if (e.Has<T1>()) e.Remove<T1>();
            if (e.Has<T2>()) e.Remove<T2>();
            if (e.Has<T3>()) e.Remove<T3>();
            if (e.Has<T4>()) e.Remove<T4>();
        }
    }

    public int CountWith<T1>(in int poolId) where T1 : Component
        => _filters[poolId].Entities.Count;

    public int CountWith<T1, T2>(in int poolId) where T1 : Component where T2 : Component
        => _filters[poolId].Entities.Count;

    public int CountWith<T1, T2, T3>(in int poolId) where T1 : Component where T2 : Component where T3 : Component
        => _filters[poolId].Entities.Count;

    public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : Component where T2 : Component where T3 : Component where T4 : Component
        => _filters[poolId].Entities.Count;

    public bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1) where T1 : Component
    {
        if (entity == null) return false;

        c1 = ((Entity)entity).Get<T1>();
        return true;
    }

    public bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2) where T1 : Component where T2 : Component
    {
        if (entity == null) return false;

        c1 = ((Entity)entity).Get<T1>();
        c2 = ((Entity)entity).Get<T2>();
        return true;
    }

    public bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3) where T1 : Component where T2 : Component where T3 : Component
    {
        if (entity == null) return false;

        c1 = ((Entity)entity).Get<T1>();
        c2 = ((Entity)entity).Get<T2>();
        c3 = ((Entity)entity).Get<T3>();
        return true;
    }

    public bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) where T1 : Component where T2 : Component where T3 : Component where T4 : Component
    {
        if (entity == null) return false;

        c1 = ((Entity)entity).Get<T1>();
        c2 = ((Entity)entity).Get<T2>();
        c3 = ((Entity)entity).Get<T3>();
        c4 = ((Entity)entity).Get<T4>();
        return true;
    }

    public void Tick(float delta)
    {
        // for (int i = 0; i < _executors!.Count; i++)
        //     _executors[i].Execute();
    }

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId) where T1 : Component
    {
        var s = _executorProvider!.Add<System<T1>>() as System<T1>;
        s!.Method = method;
        _executors!.Add(s);
    }

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId) where T1 : Component where T2 : Component
    {
        var s = _executorProvider!.Add<System<T1, T2>>() as System<T1, T2>;
        s!.Method = method;
        _executors!.Add(s);
    }

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId) where T1 : Component where T2 : Component where T3 : Component
    {
        var s = _executorProvider!.Add<System<T1, T2, T3>>() as System<T1, T2, T3>;
        s!.Method = method;
        _executors!.Add(s);
    }

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId) where T1 : Component where T2 : Component where T3 : Component where T4 : Component
    {
        var s = _executorProvider!.Add<System<T1, T2, T3, T4>>() as System<T1, T2, T3, T4>;
        s!.Method = method;
        _executors!.Add(s);
    }

    public void Dispose()
    {
        _world!.Dispose();
        _entityProvider = null;
        _entityProvider = null;
        _world = null;
    }

    public unsafe class System<T1> : Executor
        where T1 : Component
    {
        public delegate*<ref T1, void> Method;
        private readonly Filter _filter = FilterBuilder.Include<T1>().Create();

        public System(ExecutorProvider container) : base(container) { }
        
        protected override bool ExecutionBody()
        {
            _filter.Entities.ForEach(e =>
            {
                var c1 = e.Get<T1>();
                Method(ref c1);
            });
            return true;
        }
    }
    
    public unsafe class System<T1, T2> : SistemExecutor
        where T1 : Component
        where T2 : Component
    {
        private readonly Filter _filter = FilterBuilder.Include<T1>().Include<T2>().Create();
        public delegate*<ref T1, ref T2, void> Method;

        public System(ExecutorProvider container) : base(container) { }
        
        protected override bool ExecutionBody()
        {
            _filter.Entities.ForEach(e =>
            {
                var c1 = e.Get<T1>();
                var c2 = e.Get<T2>();
                Method(ref c1, ref c2);
            });
            return true;
        }
    }
    
    public unsafe class System<T1, T2, T3> : SistemExecutor
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {
        private readonly Filter _filter = FilterBuilder.Include<T1>().Include<T2>().Include<T3>().Create();
        public delegate*<ref T1, ref T2, ref T3, void> Method;
        
        public System(ExecutorProvider container) : base(container) { }
        
        protected override bool ExecutionBody()
        {
            _filter.Entities.ForEach(e =>
            {
                var c1 = e.Get<T1>();
                var c2 = e.Get<T2>();
                var c3 = e.Get<T3>();
                Method(ref c1, ref c2, ref c3);
            });
            return true;
        }
    }
    
    public unsafe class System<T1, T2, T3, T4> : SistemExecutor
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
    {
        private readonly Filter _filter = FilterBuilder.Include<T1>().Include<T2>().Include<T3>().Include<T4>().Create();
        public delegate*<ref T1, ref T2, ref T3, ref T4, void> Method;

        public System(ExecutorProvider container) : base(container) { }
        
        protected override bool ExecutionBody()
        {
            _filter.Entities.ForEach(e =>
            {
                var c1 = e.Get<T1>();
                var c2 = e.Get<T2>();
                var c3 = e.Get<T3>();
                var c4 = e.Get<T4>();
                Method(ref c1, ref c2, ref c3, ref c4);
            });
            return true;
        }
    }
}