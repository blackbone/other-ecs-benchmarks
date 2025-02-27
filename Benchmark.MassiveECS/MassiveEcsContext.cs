using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Benchmark._Context;
using DCFApixels.DragonECS;
using Massive;
using Scellecs.Morpeh;
using Entity = Massive.Entity;
using Filter = Massive.Filter;

namespace Benchmark.MassiveECS;

public class MassiveEcsContext : IBenchmarkContext<Entity>
{
	private readonly Dictionary<int, SparseSet[]> _pools = new();
	private readonly Dictionary<int, Filter> _filters = new();
	private readonly List<Action> _systems = new();
	private Registry _registry;

	public bool DeletesEntityOnLastComponentDeletion => false;

	public int NumberOfLivingEntities => _registry.Entities.Count;

	public void Setup()
	{
		_registry = new Registry();
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
		_pools[poolId] = [_registry.Set<T1>()];
		_filters[poolId] = _registry.Filter<Include<T1>>();
	}

	public void Warmup<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_pools[poolId] = [_registry.Set<T1>(), _registry.Set<T2>()];
		_filters[poolId] = _registry.Filter<Include<T1, T2>>();
	}

	public void Warmup<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_pools[poolId] = [_registry.Set<T1>(), _registry.Set<T2>(), _registry.Set<T3>()];
		_filters[poolId] = _registry.Filter<Include<T1, T2, T3>>();
	}

	public void Warmup<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_pools[poolId] = [_registry.Set<T1>(), _registry.Set<T2>(), _registry.Set<T3>(), _registry.Set<T4>()];
		_filters[poolId] = _registry.Filter<Include<T1, T2, T3, Include<T4>>>();
	}

	public void DeleteEntities(in Entity[] entitySet)
	{
		for (var i = 0; i < entitySet.Length; i++)
			if (_registry.IsAlive(entitySet[i]))
				_registry.Destroy(entitySet[i]);
	}

	public Entity[] PrepareSet(in int count)
	{
		return count > 0 ? new Entity[count] : [];
	}

	public void CreateEntities(in Entity[] entitySet)
	{
		for (var i = 0; i < entitySet.Length; i++)
			entitySet[i] = _registry.CreateEntity();
	}

	public void CreateEntities<T1>(in Entity[] entitySet, in int poolId, in T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		for (var i = 0; i < entitySet.Length; i++)
		{
			entitySet[i] = _registry.CreateEntity();
			s1.Assign(entitySet[i].Id, c1);
		}
	}

	public void CreateEntities<T1, T2>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];
		for (var i = 0; i < entitySet.Length; i++)
		{
			entitySet[i] = _registry.CreateEntity();
			s1.Assign(entitySet[i].Id, c1);
			s2.Assign(entitySet[i].Id, c2);
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
			entitySet[i] = _registry.CreateEntity();
			s1.Assign(entitySet[i].Id, c1);
			s2.Assign(entitySet[i].Id, c2);
			s3.Assign(entitySet[i].Id, c3);
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
			entitySet[i] = _registry.CreateEntity();
			s1.Assign(entitySet[i].Id, c1);
			s2.Assign(entitySet[i].Id, c2);
			s3.Assign(entitySet[i].Id, c3);
			s4.Assign(entitySet[i].Id, c4);
		}
	}

	public void AddComponent<T1>(in Entity[] entitySet, in int poolId, in T1 c1) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Assign(entitySet[i].Id, c1);
		}
	}

	public void AddComponent<T1, T2>(in Entity[] entitySet, in int poolId, in T1 c1, in T2 c2) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		var s2 = (DataSet<T2>)pools[1];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Assign(entitySet[i].Id, c1);
			s2.Assign(entitySet[i].Id, c2);
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
			s1.Assign(entitySet[i].Id, c1);
			s2.Assign(entitySet[i].Id, c2);
			s3.Assign(entitySet[i].Id, c3);
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
			s1.Assign(entitySet[i].Id, c1);
			s2.Assign(entitySet[i].Id, c2);
			s3.Assign(entitySet[i].Id, c3);
			s4.Assign(entitySet[i].Id, c4);
		}
	}

	public void RemoveComponent<T1>(in Entity[] entitySet, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = (DataSet<T1>)pools[0];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Unassign(entitySet[i].Id);
		}
	}

	public void RemoveComponent<T1, T2>(in Entity[] entitySet, in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		var pools = _pools[poolId];
		var s1 = pools[0];
		var s2 = pools[1];
		for (var i = 0; i < entitySet.Length; i++)
		{
			s1.Unassign(entitySet[i].Id);
			s2.Unassign(entitySet[i].Id);
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
			s1.Unassign(entitySet[i].Id);
			s2.Unassign(entitySet[i].Id);
			s3.Unassign(entitySet[i].Id);
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
			s1.Unassign(entitySet[i].Id);
			s2.Unassign(entitySet[i].Id);
			s3.Unassign(entitySet[i].Id);
			s4.Unassign(entitySet[i].Id);
		}
	}

	public int CountWith<T1>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		return _registry.View().Filter(_filters[poolId]).Count();
	}

	public int CountWith<T1, T2>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		return _registry.View().Filter(_filters[poolId]).Count();
	}

	public int CountWith<T1, T2, T3>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		return _registry.View().Filter(_filters[poolId]).Count();
	}

	public int CountWith<T1, T2, T3, T4>(in int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		return _registry.View().Filter(_filters[poolId]).Count();
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
		_systems.Add(() =>
		{
			var entityAction = new PointerInvocationEntityAction<T1> { Method = method };
			_registry.View().ForEach<PointerInvocationEntityAction<T1>, T1>(ref entityAction);
		});
	}

	public unsafe void AddSystem<T1, T2>(delegate*<ref T1, ref T2, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_systems.Add(() =>
		{
			var entityAction = new PointerInvocationEntityAction<T1, T2> { Method = method };
			_registry.View().ForEach<PointerInvocationEntityAction<T1, T2>, T1, T2>(ref entityAction);
		});
	}

	public unsafe void AddSystem<T1, T2, T3>(delegate*<ref T1, ref T2, ref T3, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_systems.Add(() =>
		{
			var entityAction = new PointerInvocationEntityAction<T1, T2, T3> { Method = method };
			_registry.View().ForEach<PointerInvocationEntityAction<T1, T2, T3>, T1, T2, T3>(ref entityAction);
		});
	}

	public unsafe void AddSystem<T1, T2, T3, T4>(delegate*<ref T1, ref T2, ref T3, ref T4, void> method, int poolId) where T1 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T2 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T3 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent where T4 : struct, IComponent, IEcsComponent, Xeno.IComponent, Friflo.Engine.ECS.IComponent, FFS.Libraries.StaticEcs.IComponent
	{
		_systems.Add(() =>
		{
			var entityAction = new PointerInvocationEntityAction<T1, T2, T3, T4> { Method = method };
			_registry.View().ForEach<PointerInvocationEntityAction<T1, T2, T3, T4>, T1, T2, T3, T4>(ref entityAction);
		});
	}
}
