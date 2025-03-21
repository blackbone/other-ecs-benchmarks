using System;
using System.Collections.Generic;
using System.Linq;

namespace AECS {
    public abstract class ArraySystem
    {
        private ArrayWorld _world;

        protected float delta;

        internal ArrayWorld world {
            get => _world;
            set {
                _world = value;
                OnWorldInjected();
            }
        }

        protected abstract void OnWorldInjected();

        protected internal abstract void Update(float delta);
    }

    public abstract class ArraySystem<T> : ArraySystem {
        private ArrayWorld.Data<T> data;

        protected sealed override void OnWorldInjected() {
            data = world.GetData<T>();
        }

        protected internal sealed override void Update(float delta) {
            this.delta = delta;
            foreach (var i in data.indices) {
                OnUpdate(ref data.data[i]);
            }
        }

        protected abstract void OnUpdate(ref T c);
    }

    public abstract class ArraySystem<T1, T2> : ArraySystem {
        private ArrayWorld.Data<T1> data1;
        private ArrayWorld.Data<T2> data2;

        protected sealed override void OnWorldInjected() {
            data1 = world.GetData<T1>();
            data2 = world.GetData<T2>();
        }

        protected internal  sealed override void Update(float delta) {
            this.delta = delta;

            var ids = data1.indices
                .Intersect(data2.indices);

            foreach (var idx in ids)
                OnUpdate(ref data1.data[idx], ref data2.data[idx]);
        }

        protected abstract void OnUpdate(ref T1 c1, ref T2 c2);
    }

    public abstract class ArraySystem<T1, T2, T3> : ArraySystem {
        private ArrayWorld.Data<T1> data1;
        private ArrayWorld.Data<T2> data2;
        private ArrayWorld.Data<T3> data3;

        protected sealed override void OnWorldInjected() {
            data1 = world.GetData<T1>();
            data2 = world.GetData<T2>();
            data3 = world.GetData<T3>();
        }

        protected internal  sealed override void Update(float delta) {
            this.delta = delta;

            var ids = data1.indices
                .Intersect(data2.indices)
                .Intersect(data3.indices);

            foreach (var idx in ids)
                OnUpdate(ref data1.data[idx], ref data2.data[idx], ref data3.data[idx]);
        }

        protected abstract void OnUpdate(ref T1 c1, ref T2 c2, ref T3 c3);
    }

    public abstract class ArraySystem<T1, T2, T3, T4> : ArraySystem {
        private ArrayWorld.Data<T1> data1;
        private ArrayWorld.Data<T2> data2;
        private ArrayWorld.Data<T3> data3;
        private ArrayWorld.Data<T4> data4;

        protected sealed override void OnWorldInjected() {
            data1 = world.GetData<T1>();
            data2 = world.GetData<T2>();
            data3 = world.GetData<T3>();
            data4 = world.GetData<T4>();
        }

        protected internal  sealed override void Update(float delta) {
            this.delta = delta;

            var ids = data1.indices
                .Intersect(data2.indices)
                .Intersect(data3.indices)
                .Intersect(data4.indices);

            foreach (var idx in ids)
                OnUpdate(ref data1.data[idx], ref data2.data[idx], ref data3.data[idx], ref data4.data[idx]);
        }

        protected abstract void OnUpdate(ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4);
    }

    public class ArrayWorld : IDisposable {
        private const ulong IS_ALIVE_MASK = 0b1000000000000000000000000000000000000000000000000000000000000000L;

        private ulong[] entities;
        internal readonly Dictionary<Type, Data> components = new Dictionary<Type, Data>();
        private readonly List<ArraySystem> systems = new List<ArraySystem>();

        public int EntityCount { get; private set; }

        public ArrayWorld() {
            entities = new ulong[1024];
            InitEntities(0, 1024);
            EntityCount = 0;
        }

        private void InitEntities(int min, int max) {
            while (min < max) {
                entities[min] = ~IS_ALIVE_MASK & ((ulong)min << 31);
                min++;
            }
        }

        public ulong CreateEntity() {
            if (EntityCount == entities.Length) {
                Array.Resize(ref entities, EntityCount * 2);
                InitEntities(EntityCount, EntityCount * 2);
            }

            entities[EntityCount] &= IS_ALIVE_MASK;
            return entities[EntityCount++];
        }

        public void DestroyEntity(ulong e) {
            var idx = getIdx(e);
            if (e != entities[idx]) return;
            entities[idx] = ++e & ~IS_ALIVE_MASK;
        }

        public void AddComponent<T>(in ulong e, in T c1) {
            var idx = getIdx(e);
            if (e != entities[idx]) return;
            Data<T> tData;
            if (!components.TryGetValue(typeof(T), out var data))
                components[typeof(T)] = tData = new Data<T>(EntityCount);
            else
                tData = (Data<T>)data;

            tData.Add(idx, c1);
        }

        public void RemoveComponent<T>(in ulong e) {
            var idx = getIdx(e);
            if (e != entities[idx]) return;

            if (!components.TryGetValue(typeof(T), out var data)) return;
            ((Data<T>)data).Remove(idx);
        }

        public int Count<T1>() {
            if (!components.TryGetValue(typeof(T1), out var data1)) return 0;
            return data1.indices.Count;
        }

        public int Count<T1, T2>() {
            if (!components.TryGetValue(typeof(T1), out var data1)) return 0;
            if (!components.TryGetValue(typeof(T2), out var data2)) return 0;

            return data1.indices
                .Intersect(data2.indices)
                .Count();
        }

        public int Count<T1, T2, T3>() {
            if (!components.TryGetValue(typeof(T1), out var data1)) return 0;
            if (!components.TryGetValue(typeof(T2), out var data2)) return 0;
            if (!components.TryGetValue(typeof(T3), out var data3)) return 0;

            return data1.indices
                .Intersect(data2.indices)
                .Intersect(data3.indices)
                .Count();
        }

        public int Count<T1, T2, T3, T4>() {
            if (!components.TryGetValue(typeof(T1), out var data1)) return 0;
            if (!components.TryGetValue(typeof(T2), out var data2)) return 0;
            if (!components.TryGetValue(typeof(T3), out var data3)) return 0;
            if (!components.TryGetValue(typeof(T4), out var data4)) return 0;

            return data1.indices
                .Intersect(data2.indices)
                .Intersect(data3.indices)
                .Intersect(data4.indices)
                .Count();
        }

        public T Get<T>(in ulong e) {
            var idx = getIdx(e);
            if (e != entities[idx]) return default;

            if (!components.TryGetValue(typeof(T), out var data)) return default;
            return ((Data<T>)data).Get(idx);
        }

        public bool Ref<T>(in ulong e, ref T c) {
            var idx = getIdx(e);
            if (e != entities[idx]) return default;

            if (!components.TryGetValue(typeof(T), out var data)) return default;
            return ((Data<T>)data).Ref(idx, ref c);
        }

        public void Dispose() {
            entities = null;
        }


        private static uint getIdx(ulong entityId) => (uint)(entityId & ~IS_ALIVE_MASK) >> 31;
        private static int getEnclosingPo2(uint v) {
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            return (int)(v + 1);
        }

        internal class Data {
            public readonly HashSet<uint> indices = new HashSet<uint>();
        }

        internal class Data<T> : Data {
            internal T[] data;

            public Data(int count) => data = new T[count];

            public void Add(in uint idx, in T c) {
                if (indices.Add(idx)) return;
                if (idx >= data.Length) System.Array.Resize(ref data, getEnclosingPo2(idx));
                data[idx] = c;
            }

            public void Remove(in uint idx) {
                if (idx >= data.Length) return;
                if (indices.Remove(idx)) return;
                data[idx] = default;
            }

            public T Get(in uint idx) {
                if (idx >= data.Length) return default;
                if (indices.Contains(idx)) return default;
                return data[idx];
            }

            public bool Ref(in uint idx, ref T c) {
                if (idx >= data.Length) return false;
                if (indices.Contains(idx)) return false;
                c = ref data[idx];
                return true;
            }
        }

        public void Update(float delta) {
            foreach (var system in systems)
                system.Update(delta);
        }

        public void AddSystem(ArraySystem system) {
            system.world = this;
            systems.Add(system);
        }
        internal Data<T> GetData<T>() {
            Data<T> data;
            if (!components.TryGetValue(typeof(T), out var d))
                components[typeof(T)] = data = new Data<T>(EntityCount);
            else
                data = (Data<T>)d;

            return data;
        }
    }
}
