using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Benchmark.Context;
using DCFApixels.DragonECS;
using Massive;
using Scellecs.Morpeh;
using Entity = Massive.Entifier;
using World = Massive.World;

namespace Benchmark.MassiveECS;

public class MassiveEcsContext : IBenchmarkContext<Entity>
{
	private readonly Dictionary<int, SparseSet[]> _pools = new();
	private readonly Dictionary<int, Query> _filters = new();
	private readonly List<Action> _systems = new();
	private World _world;

	public bool DeletesEntityOnLastComponentDeletion => false;

	public int NumberOfLivingEntities => _world.Entifiers.Count;

	public void Setup()
	{
		_world = new World();
	}

	public void FinishSetup()
	{
	}

	public void Cleanup()
	{
		_pools.Clear();
		_filters.Clear();
		_systems.Clear();
	}

	public void Dispose()
	{
	}

	public void Warmup<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_pools[poolId] = [_world.SparseSet<T1>()];
		_filters[poolId] = _world.Include<T1>();
	}

	public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_pools[poolId] = [_world.SparseSet<T1>(), _world.SparseSet<T2>()];
		_filters[poolId] = _world.Include<T1, T2>();
	}

	public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_pools[poolId] = [_world.SparseSet<T1>(), _world.SparseSet<T2>(), _world.SparseSet<T3>()];
		_filters[poolId] = _world.Include<T1, T2, T3>();
	}

	public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_pools[poolId] = [_world.SparseSet<T1>(), _world.SparseSet<T2>(), _world.SparseSet<T3>(), _world.SparseSet<T4>()];
		_filters[poolId] = _world.Include<T1, T2, T3, Include<T4>>();
	}

	public void DeleteEntities(in Entity[] entitySet)
	{
		for (var i = 0; i < entitySet.Length; i++)
			_world.Destroy(entitySet[i]);
	}

	public Entity[] PrepareSet(in int count)
	{
		return count > 0 ? new Entity[count] : [];
	}

	public void CreateEntities(in Entity[] entitySet)
	{
		for (var i = 0; i < entitySet.Length; i++)
			entitySet[i] = _world.CreateEntifier();
	}

	public void CreateEntities<T1>(in Entity[] entitySet, in int poolId, in T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		for (var i = 0; i < entitySet.Length; i++)
		{
			entitySet[i] = _world.CreateEntifier();
			s1.Set(entitySet[i].Id, c1);
		}
	}

	public void CreateEntities<T1, T2>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];
		for (var i = 0; i < entitySet.Length; i++)
		{
			entitySet[i] = _world.CreateEntifier();
			s1.Set(entitySet[i].Id, c1);
			s2.Set(entitySet[i].Id, c2);
		}
	}

	public void CreateEntities<T1, T2, T3>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];
		var s3 = (DataSet<T3>)pools[2];
		for (var i = 0; i < entitySet.Length; i++)
		{
			entitySet[i] = _world.CreateEntifier();
			s1.Set(entitySet[i].Id, c1);
			s2.Set(entitySet[i].Id, c2);
			s3.Set(entitySet[i].Id, c3);
		}
	}

	public void CreateEntities<T1, T2, T3, T4>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];
		var s3 = (DataSet<T3>)pools[2];
		var s4 = (DataSet<T4>)pools[3];
		for (var i = 0; i < entitySet.Length; i++)
		{
			entitySet[i] = _world.CreateEntifier();
			s1.Set(entitySet[i].Id, c1);
			s2.Set(entitySet[i].Id, c2);
			s3.Set(entitySet[i].Id, c3);
			s4.Set(entitySet[i].Id, c4);
		}
	}

	public void AddComponent<T1>(in Entity[] entitySet, in int poolId, in T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Set(entitySet[i].Id, c1);
		}
	}

	public void AddComponent<T1, T2>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Set(entitySet[i].Id, c1);
			s2.Set(entitySet[i].Id, c2);
		}
	}

	public void AddComponent<T1, T2, T3>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];
		var s3 = (DataSet<T3>)pools[2];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Set(entitySet[i].Id, c1);
			s2.Set(entitySet[i].Id, c2);
			s3.Set(entitySet[i].Id, c3);
		}
	}

	public void AddComponent<T1, T2, T3, T4>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2, in T3 c3, in T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];
		var s3 = (DataSet<T3>)pools[2];
		var s4 = (DataSet<T4>)pools[3];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Set(entitySet[i].Id, c1);
			s2.Set(entitySet[i].Id, c2);
			s3.Set(entitySet[i].Id, c3);
			s4.Set(entitySet[i].Id, c4);
		}
	}

	public void RemoveComponent<T1>(in Entity[] entitySet, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = pools[0];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Remove(entitySet[i].Id);
		}
	}

	public void RemoveComponent<T1, T2>(in Entity[] entitySet, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = pools[0];
		var s2 = pools[1];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Remove(entitySet[i].Id);
			s2.Remove(entitySet[i].Id);
		}
	}

	public void RemoveComponent<T1, T2, T3>(in Entity[] entitySet, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = pools[0];
		var s2 = pools[1];
		var s3 = pools[2];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Remove(entitySet[i].Id);
			s2.Remove(entitySet[i].Id);
			s3.Remove(entitySet[i].Id);
		}
	}

	public void RemoveComponent<T1, T2, T3, T4>(in Entity[] entitySet, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = pools[0];
		var s2 = pools[1];
		var s3 = pools[2];
		var s4 = pools[3];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Remove(entitySet[i].Id);
			s2.Remove(entitySet[i].Id);
			s3.Remove(entitySet[i].Id);
			s4.Remove(entitySet[i].Id);
		}
	}

	public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		return _filters[poolId].Count();
	}

	public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		return _filters[poolId].Count();
	}

	public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		return _filters[poolId].Count();
	}

	public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		return _filters[poolId].Count();
	}

	public bool GetSingle<T1>(in Entity entity, in int poolId, ref T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];

		c1 = s1.Get(entity.Id);
		return true;
	}

	public bool GetSingle<T1, T2>(in Entity entity, in int poolId, ref T1 c1, ref T2 c2) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];

		c1 = s1.Get(entity.Id);
		c2 = s2.Get(entity.Id);
		return true;
	}

	public bool GetSingle<T1, T2, T3>(in Entity entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];
		var s3 = (DataSet<T3>)pools[2];

		c1 = s1.Get(entity.Id);
		c2 = s2.Get(entity.Id);
		c3 = s3.Get(entity.Id);
		return true;
	}

	public bool GetSingle<T1, T2, T3, T4>(in Entity entity, in int poolId, ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];
		var s3 = (DataSet<T3>)pools[2];
		var s4 = (DataSet<T4>)pools[3];

		c1 = s1.Get(entity.Id);
		c2 = s2.Get(entity.Id);
		c3 = s3.Get(entity.Id);
		c4 = s4.Get(entity.Id);
		return true;
	}

	public void Tick(float delta)
	{
		foreach (var system in CollectionsMarshal.AsSpan(_systems))
		{
			system.Invoke();
		}
	}

	public unsafe void AddSystem<T1>(delegate*<ref T1, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_systems.Add(() => _world.ForEach((ref T1 c1) => method(ref c1)));
	}

	public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_systems.Add(() => _world.ForEach((ref T1 c1, ref T2 c2) => method(ref c1, ref c2)));
	}

	public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_systems.Add(() => _world.ForEach((ref T1 c1, ref T2 c2, ref T3 c3) => method(ref c1, ref c2, ref c3)));
	}

	public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_systems.Add(() => _world.ForEach((ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4) => method(ref c1, ref c2, ref c3, ref c4)));
	}
}
