using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;

namespace Benchmark._Context;

public abstract class BenchmarkContextBase : IDisposable
{
    /// <summary>
    /// Does framework deletes entity on last component deletion.
    /// E.g. is there auto despawn of empty entities.
    /// </summary>
    public abstract bool DeletesEntityOnLastComponentDeletion { get; }
    
    /// <summary>
    /// Current count of entities in ECS.
    /// </summary>
    public abstract int EntityCount { get; }

    /// <summary>
    /// Initialize world for test.
    /// </summary>
    /// <param name="entityCount"> Amount of entities will be used in benchmark. </param>
    public abstract void Setup(int entityCount);

    /// <summary>
    /// Indicates setup has been finished.
    /// </summary>
    public virtual void FinishSetup() { }

    /// <summary>
    /// Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId"> Id of cache - will be passed to <see cref="CreateEntities{T1}"/>, <see cref="AddComponent{T1}"/>, <see cref="RemoveComponent{T1}"/>. </param>
    public abstract void Warmup<T1>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId"> Id of cache - will be passed to <see cref="CreateEntities{T1,T2}"/>, <see cref="AddComponent{T1, T2}"/>, <see cref="RemoveComponent{T1, T2}"/>. </param>
    public abstract void Warmup<T1, T2>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent where T2 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId"> Id of cache - will be passed to <see cref="CreateEntities{T1,T2,T3}"/>, <see cref="AddComponent{T1, T2, T3}"/>, <see cref="RemoveComponent{T1, T2, T3}"/>. </param>
    public abstract void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId"> Id of cache - will be passed to <see cref="CreateEntities{T1,T2,T3,T4}"/>, <see cref="AddComponent{T1, T2, T3, T4}"/>, <see cref="RemoveComponent{T1, T2, T3, T4}"/>. </param>
    public abstract void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Clean up the world.
    /// </summary>
    public abstract void Cleanup();

    /// <summary>
    /// Disposition of context.
    /// </summary>
    public void Dispose()
    {
    }

    /// <summary>
    /// Acquire lock for deferred structural changes/
    /// </summary>
    public abstract void Lock();

    /// <summary>
    /// Release lock and apply structural changes.
    /// </summary>
    public abstract void Commit();

    #region Entity - Create

    /// <summary>
    /// Create empty entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    public abstract void CreateEntities(in Array entitySet);

    /// <summary>
    /// Create entity with components.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1}"/></param>
    /// <returns> Entity set identifier. Literally it can be anything, filter, query ids array, whatever. Will be used as argument in <see cref="AddComponent{T1}"/>, <see cref="RemoveComponent{T1}"/>, <see cref="DeleteEntities"/> </returns>
    public abstract void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Create entity with components.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2}"/></param>
    /// <returns> Entity set identifier. Literally it can be anything, filter, query ids array, whatever. Will be used as argument in <see cref="AddComponent{T1}"/>, <see cref="RemoveComponent{T1}"/>, <see cref="DeleteEntities"/> </returns>
    public abstract void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Create entity with components.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3}"/></param>
    /// <returns> Entity set identifier. Literally it can be anything, filter, query ids array, whatever. Will be used as argument in <see cref="AddComponent{T1}"/>, <see cref="RemoveComponent{T1}"/>, <see cref="DeleteEntities"/> </returns>
    public abstract void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Create entity with components.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3, T4}"/></param>
    /// <returns> Entity set identifier. Literally it can be anything, filter, query ids array, whatever. Will be used as argument in <see cref="AddComponent{T1}"/>, <see cref="RemoveComponent{T1}"/>, <see cref="DeleteEntities"/> </returns>
    public abstract void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent;

    #endregion

    #region Entity - Delete

    /// <summary>
    /// Deletes set of entities.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    public abstract void DeleteEntities(in Array entitySet);

    #endregion

    #region Entity - Add Component

    /// <summary>
    /// Add component to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1}"/></param>
    public abstract void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Add components to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2}"/></param>
    public abstract void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Add components to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3}"/></param>
    public abstract void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Add components to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3, T4}"/></param>
    public abstract void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent;

    #endregion

    #region Entity - Remove Component

    /// <summary>
    /// Removes component to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1}"/></param>
    public abstract void RemoveComponent<T1>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Removes component to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2}"/></param>
    public abstract void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Removes component to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3}"/></param>
    public abstract void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent;

    /// <summary>
    /// Removes component to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3, T4}"/></param>
    public abstract void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent;

    #endregion

    #region Entity - Count

    public abstract int CountWith<T1>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent;
    
    public abstract int CountWith<T1, T2>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent;
    
    public abstract int CountWith<T1, T2, T3>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent;
    
    public abstract int CountWith<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent;

    #endregion

    #region Entities - Get With

    public abstract bool GetSingle<T1>(in object? entity, in int poolId, ref T1 c1)
        where T1 : struct, MorpehComponent, DragonComponent;
    
    public abstract bool GetSingle<T1, T2>(in object? entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent;
    
    public abstract bool GetSingle<T1, T2, T3>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent;
    
    public abstract bool GetSingle<T1, T2, T3, T4>(in object? entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent;

    #endregion

    #region Systems

    /// <summary>
    /// Iterates world once.
    /// </summary>
    /// <param name="delta"> Delta time argument. </param>
    public abstract void Tick(float delta);

    public abstract unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent;
    
    public abstract unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent;
    
    public abstract unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent;
    
    public abstract unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent
        where T2 : struct, MorpehComponent, DragonComponent
        where T3 : struct, MorpehComponent, DragonComponent
        where T4 : struct, MorpehComponent, DragonComponent;

    #endregion
    
    /// <summary>
    /// Shuffles entities in set.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities"/> </param>
    /// <returns> New or existing shuffled set. </returns>
    public abstract Array Shuffle(in Array entitySet);

    /// <summary>
    /// Prepares entity set.
    /// </summary>
    /// <param name="count"> Count of entities to be stored. </param>
    /// <returns> New or existing shuffled set. </returns>
    public abstract Array PrepareSet(in int count);

    public override string ToString() => GetType().Name;
}