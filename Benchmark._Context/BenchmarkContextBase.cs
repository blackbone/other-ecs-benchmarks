using MorpehComponent = Scellecs.Morpeh.IComponent;


namespace Benchmark._Context;

public abstract class BenchmarkContextBase : IDisposable
{
    /// <summary>
    /// Initialize world for test.
    /// </summary>
    /// <param name="entityCount"> Amount of entities will be used in benchmark. </param>
    public abstract void Setup(int entityCount);

    /// <summary>
    /// Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId"> Id of cache - will be passed to <see cref="CreateEntity{T1}"/>, <see cref="AddComponent{T1}"/>, <see cref="RemoveComponent{T1}"/>. </param>
    public abstract void Warmup<T1>(int poolId) where T1 : struct, MorpehComponent;

    /// <summary>
    /// Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId"> Id of cache - will be passed to <see cref="CreateEntity{T1, T2}"/>, <see cref="AddComponent{T1, T2}"/>, <see cref="RemoveComponent{T1, T2}"/>. </param>
    public abstract void Warmup<T1, T2>(int poolId)
        where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent;

    /// <summary>
    /// Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId"> Id of cache - will be passed to <see cref="CreateEntity{T1, T2, T3}"/>, <see cref="AddComponent{T1, T2, T3}"/>, <see cref="RemoveComponent{T1, T2, T3}"/>. </param>
    public abstract void Warmup<T1, T2, T3>(int poolId) where T1 : struct, MorpehComponent
        where T2 : struct, MorpehComponent
        where T3 : struct, MorpehComponent;

    /// <summary>
    /// Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId"> Id of cache - will be passed to <see cref="CreateEntity{T1, T2, T3, T4}"/>, <see cref="AddComponent{T1, T2, T3, T4}"/>, <see cref="RemoveComponent{T1, T2, T3, T4}"/>. </param>
    public abstract void Warmup<T1, T2, T3, T4>(int poolId) where T1 : struct, MorpehComponent
        where T2 : struct, MorpehComponent
        where T3 : struct, MorpehComponent
        where T4 : struct, MorpehComponent;

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

    /// <summary>
    /// Create empty entity.
    /// </summary>
    /// <returns> Sequence number of created entity - this will be used as argument in entity manipulation. </returns>
    public abstract int CreateEntity();

    /// <summary>
    /// Create entity with components.
    /// </summary>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1}"/></param>
    /// <returns> Sequence number of created entity - this will be used as argument in entity manipulation. </returns>
    public abstract int CreateEntity<T1>(in int poolId = -1) where T1 : struct, MorpehComponent;

    /// <summary>
    /// Create entity with components.
    /// </summary>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2}"/></param>
    /// <returns> Sequence number of created entity - this will be used as argument in entity manipulation. </returns>
    public abstract int CreateEntity<T1, T2>(in int poolId = -1) where T1 : struct, MorpehComponent
        where T2 : struct, MorpehComponent;

    /// <summary>
    /// Create entity with components.
    /// </summary>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3}"/></param>
    /// <returns> Sequence number of created entity - this will be used as argument in entity manipulation. </returns>
    public abstract int CreateEntity<T1, T2, T3>(in int poolId = -1) where T1 : struct, MorpehComponent
        where T2 : struct, MorpehComponent
        where T3 : struct, MorpehComponent;

    /// <summary>
    /// Create entity with components.
    /// </summary>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3, T4}"/></param>
    /// <returns> Sequence number of created entity - this will be used as argument in entity manipulation. </returns>
    public abstract int CreateEntity<T1, T2, T3, T4>(in int poolId = -1) where T1 : struct, MorpehComponent
        where T2 : struct, MorpehComponent
        where T3 : struct, MorpehComponent
        where T4 : struct, MorpehComponent;

    /// <summary>
    /// Add component to entity.
    /// </summary>
    /// <param name="entityIds"> Sequence identifier returned by <see cref="CreateEntity"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1}"/></param>
    public abstract void AddComponent<T1>(in int[] entityIds, in int poolId = -1) where T1 : struct, MorpehComponent;

    /// <summary>
    /// Add components to entity.
    /// </summary>
    /// <param name="entityIds"> Sequence identifier returned by <see cref="CreateEntity"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2}"/></param>
    public abstract void AddComponent<T1, T2>(in int[] entityIds, in int poolId = -1) where T1 : struct, MorpehComponent
        where T2 : struct, MorpehComponent;

    /// <summary>
    /// Add components to entity.
    /// </summary>
    /// <param name="entityIds"> Sequence identifier returned by <see cref="CreateEntity"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3}"/></param>
    public abstract void AddComponent<T1, T2, T3>(in int[] entityIds, in int poolId = -1)
        where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent where T3 : struct, MorpehComponent;

    /// <summary>
    /// Add components to entity.
    /// </summary>
    /// <param name="entityIds"> Sequence identifier returned by <see cref="CreateEntity"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3, T4}"/></param>
    public abstract void AddComponent<T1, T2, T3, T4>(in int[] entityIds, in int poolId = -1)
        where T1 : struct, MorpehComponent
        where T2 : struct, MorpehComponent
        where T3 : struct, MorpehComponent
        where T4 : struct, MorpehComponent;

    /// <summary>
    /// Removes component to entity.
    /// </summary>
    /// <param name="entityIds"> Sequence identifier returned by <see cref="CreateEntity"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1}"/></param>
    public abstract void RemoveComponent<T1>(in int[] entityIds, in int poolId = -1) where T1 : struct, MorpehComponent;

    /// <summary>
    /// Removes component to entity.
    /// </summary>
    /// <param name="entityIds"> Sequence identifier returned by <see cref="CreateEntity"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2}"/></param>
    public abstract void RemoveComponent<T1, T2>(in int[] entityIds, in int poolId = -1)
        where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent;

    /// <summary>
    /// Removes component to entity.
    /// </summary>
    /// <param name="entityIds"> Sequence identifier returned by <see cref="CreateEntity"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3}"/></param>
    public abstract void RemoveComponent<T1, T2, T3>(in int[] entityIds, in int poolId = -1)
        where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent where T3 : struct, MorpehComponent;

    /// <summary>
    /// Removes component to entity.
    /// </summary>
    /// <param name="entityIds"> Sequence identifier returned by <see cref="CreateEntity"/> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3, T4}"/></param>
    public abstract void RemoveComponent<T1, T2, T3, T4>(in int[] entityIds, in int poolId = -1)
        where T1 : struct, MorpehComponent
        where T2 : struct, MorpehComponent
        where T3 : struct, MorpehComponent
        where T4 : struct, MorpehComponent;
}