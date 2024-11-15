using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using XenoComponent = Xeno.IComponent;
using FrifloComponent = Friflo.Engine.ECS.IComponent;

namespace Benchmark._Context;

public interface IBenchmarkContext : IDisposable
{
    /// <summary>
    ///     Does framework deletes entity on last component deletion.
    ///     E.g. is there auto despawn of empty entities.
    /// </summary>
    public bool DeletesEntityOnLastComponentDeletion { get; }

    /// <summary>
    ///     Current count of entities in ECS.
    /// </summary>
    public int EntityCount { get; }

    /// <summary>
    ///     Initialize world for test.
    /// </summary>
    /// <param name="entityCount"> Amount of entities will be used in benchmark. </param>
    public void Setup();

    /// <summary>
    ///     Indicates setup has been finished.
    /// </summary>
    public void FinishSetup();

    /// <summary>
    ///     Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId">
    ///     Id of cache - will be passed to <see cref="CreateEntities{T1}" />,
    ///     <see cref="AddComponent{T1}" />, <see cref="RemoveComponent{T1}" />.
    /// </param>
    public void Warmup<T1>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId">
    ///     Id of cache - will be passed to <see cref="CreateEntities{T1,T2}" />,
    ///     <see cref="AddComponent{T1, T2}" />, <see cref="RemoveComponent{T1, T2}" />.
    /// </param>
    public void Warmup<T1, T2>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId">
    ///     Id of cache - will be passed to <see cref="CreateEntities{T1,T2,T3}" />,
    ///     <see cref="AddComponent{T1, T2, T3}" />, <see cref="RemoveComponent{T1, T2, T3}" />.
    /// </param>
    public void Warmup<T1, T2, T3>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Place where you can set up your stashes, caches, etc.
    /// </summary>
    /// <param name="poolId">
    ///     Id of cache - will be passed to <see cref="CreateEntities{T1,T2,T3,T4}" />,
    ///     <see cref="AddComponent{T1, T2, T3, T4}" />, <see cref="RemoveComponent{T1, T2, T3, T4}" />.
    /// </param>
    public void Warmup<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Clean up the world.
    /// </summary>
    public void Cleanup();

    #region Entity - Delete

    /// <summary>
    ///     Deletes set of entities.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    public void DeleteEntities(in Array entitySet);

    #endregion

    /// <summary>
    ///     Prepares entity set.
    /// </summary>
    /// <param name="count"> Count of entities to be stored. </param>
    /// <returns> New or existing shuffled set. </returns>
    public Array PrepareSet(in int count);

    #region Entity - Create

    /// <summary>
    ///     Create empty entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    public void CreateEntities(in Array entitySet);

    /// <summary>
    ///     Create entity with components.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1}" /></param>
    /// <returns>
    ///     Entity set identifier. Literally it can be anything, filter, query ids array, whatever. Will be used as
    ///     argument in <see cref="AddComponent{T1}" />, <see cref="RemoveComponent{T1}" />, <see cref="DeleteEntities" />
    /// </returns>
    public void CreateEntities<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Create entity with components.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2}" /></param>
    /// <returns>
    ///     Entity set identifier. Literally it can be anything, filter, query ids array, whatever. Will be used as
    ///     argument in <see cref="AddComponent{T1}" />, <see cref="RemoveComponent{T1}" />, <see cref="DeleteEntities" />
    /// </returns>
    public void CreateEntities<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Create entity with components.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3}" /></param>
    /// <returns>
    ///     Entity set identifier. Literally it can be anything, filter, query ids array, whatever. Will be used as
    ///     argument in <see cref="AddComponent{T1}" />, <see cref="RemoveComponent{T1}" />, <see cref="DeleteEntities" />
    /// </returns>
    public void CreateEntities<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Create entity with components.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3, T4}" /></param>
    /// <returns>
    ///     Entity set identifier. Literally it can be anything, filter, query ids array, whatever. Will be used as
    ///     argument in <see cref="AddComponent{T1}" />, <see cref="RemoveComponent{T1}" />, <see cref="DeleteEntities" />
    /// </returns>
    public void CreateEntities<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    #endregion

    #region Entity - Add Component

    /// <summary>
    ///     Add component to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1}" /></param>
    public void AddComponent<T1>(in Array entitySet, in int poolId = -1, in T1 c1 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Add components to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2}" /></param>
    public void AddComponent<T1, T2>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Add components to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3}" /></param>
    public void AddComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Add components to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3, T4}" /></param>
    public void AddComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1, in T1 c1 = default, in T2 c2 = default, in T3 c3 = default, in T4 c4 = default)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    #endregion

    #region Entity - Remove Component

    /// <summary>
    ///     Removes component to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1}" /></param>
    public void RemoveComponent<T1>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Removes component to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2}" /></param>
    public void RemoveComponent<T1, T2>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Removes component to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3}" /></param>
    public void RemoveComponent<T1, T2, T3>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    /// <summary>
    ///     Removes component to entity.
    /// </summary>
    /// <param name="entitySet"> Entity set object returned by <see cref="CreateEntities" /> </param>
    /// <param name="poolId"> ID of pool from <see cref="Warmup{T1, T2, T3, T4}" /></param>
    public void RemoveComponent<T1, T2, T3, T4>(in Array entitySet, in int poolId = -1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    #endregion

    #region Entity - Count

    public int CountWith<T1>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    public int CountWith<T1, T2>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    public int CountWith<T1, T2, T3>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    public int CountWith<T1, T2, T3, T4>(in int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    #endregion

    #region Entities - Get With

    public bool GetSingle<T1>(in object entity, in int poolId, ref T1 c1)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    public bool GetSingle<T1, T2>(in object entity, in int poolId, ref T1 c1, ref T2 c2)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    public bool GetSingle<T1, T2, T3>(in object entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    public bool GetSingle<T1, T2, T3, T4>(in object entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    #endregion

    #region Systems

    /// <summary>
    ///     Iterates world once.
    /// </summary>
    /// <param name="delta"> Delta time argument. </param>
    public void Tick(float delta);

    public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId)
        where T1 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T2 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T3 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent
        where T4 : struct, MorpehComponent, DragonComponent, XenoComponent, FrifloComponent;

    #endregion
}