using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
            var mask = data.mask;
            for (int i = 0; i < base.world.EntityCount; i++) {
                if (mask.Get(i))
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

            var mask = new BitArray(data1.mask).And(data2.mask);
            for (int i = 0; i < base.world.EntityCount; i++) {
                if (mask.Get(i))
                    OnUpdate(ref data1.data[i], ref data2.data[i]);
            }
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

            var mask = new BitArray(data1.mask).And(data2.mask).And(data3.mask);
            for (int i = 0; i < base.world.EntityCount; i++) {
                if (mask.Get(i))
                    OnUpdate(ref data1.data[i], ref data2.data[i], ref data3.data[i]);
            }
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

            var mask = new BitArray(data1.mask).And(data2.mask).And(data3.mask).And(data4.mask);
            for (int i = 0; i < base.world.EntityCount; i++) {
                if (mask.Get(i))
                    OnUpdate(ref data1.data[i], ref data2.data[i], ref data3.data[i], ref data4.data[i]);
            }
        }

        protected abstract void OnUpdate(ref T1 c1, ref T2 c2, ref T3 c3, ref T4 c4);
    }

    public class ArrayWorld : IDisposable {
        private const ulong IS_ALIVE_MASK = 0b1000000000000000000000000000000000000000000000000000000000000000L;
        private const int ID_SHIFT = 31;

        private ulong[] entities;
        internal readonly Dictionary<Type, Data> components = new Dictionary<Type, Data>();
        private readonly List<ArraySystem> systems = new List<ArraySystem>();

        public int EntityCount { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayWorld(int entityCount) {
            entities = new ulong[entityCount];
            InitEntities(0, entityCount);
            EntityCount = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitEntities(int min, int max) {
            while (min < max) {
                entities[min] = ~IS_ALIVE_MASK & ((ulong)min << ID_SHIFT);
                min++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong CreateEntity() {
            if (EntityCount == entities.Length) {
                throw new IndexOutOfRangeException();
            }

            entities[EntityCount] |= IS_ALIVE_MASK;
            return entities[EntityCount++];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestroyEntity(ulong e) {
            var idx = getIdx(e);
            if (e != entities[idx]) return;
            entities[idx] = ++e & ~IS_ALIVE_MASK;
            EntityCount--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<T>(in ulong e) {
            var idx = getIdx(e);
            if (e != entities[idx]) return;

            if (!components.TryGetValue(typeof(T), out var data)) return;
            ((Data<T>)data).Remove(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count<T1>() {
            if (!components.TryGetValue(typeof(T1), out var data1)) return 0;
            return data1.mask.Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count<T1, T2>() {
            if (!components.TryGetValue(typeof(T1), out var data1)) return 0;
            if (!components.TryGetValue(typeof(T2), out var data2)) return 0;

            return new BitArray(data1.mask).And(data2.mask).Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count<T1, T2, T3>() {
            if (!components.TryGetValue(typeof(T1), out var data1)) return 0;
            if (!components.TryGetValue(typeof(T2), out var data2)) return 0;
            if (!components.TryGetValue(typeof(T3), out var data3)) return 0;

            return new BitArray(data1.mask).And(data2.mask).And(data3.mask).Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count<T1, T2, T3, T4>() {
            if (!components.TryGetValue(typeof(T1), out var data1)) return 0;
            if (!components.TryGetValue(typeof(T2), out var data2)) return 0;
            if (!components.TryGetValue(typeof(T3), out var data3)) return 0;
            if (!components.TryGetValue(typeof(T4), out var data4)) return 0;

            return new BitArray(data1.mask).And(data2.mask).And(data3.mask).And(data4.mask).Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>(in ulong e) {
            var idx = getIdx(e);
            if (e != entities[idx]) return default;

            if (!components.TryGetValue(typeof(T), out var data)) return default;
            return ((Data<T>)data).Get(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Ref<T>(in ulong e, ref T c) {
            var idx = getIdx(e);
            if (e != entities[idx]) return default;

            if (!components.TryGetValue(typeof(T), out var data)) return default;
            return ((Data<T>)data).Ref(idx, ref c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() {
            entities = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Preallocate<T>(int entityCount) {
            components[typeof(T)] = new Data<T>(entityCount);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint getIdx(ulong entityId) => (uint)((entityId & ~IS_ALIVE_MASK) >> ID_SHIFT);

        internal class Data {
            public readonly BitArray mask;
            public Data(int count) => mask = new BitArray(count);
        }

        internal class Data<T> : Data {
            internal readonly T[] data;

            public Data(int count):base(count) => data = new T[count];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(in uint idx, in T c) {
                if (mask.Get((int)idx)) return;
                if (idx >= data.Length) throw new IndexOutOfRangeException();
                data[idx] = c;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Remove(in uint idx) {
                if (idx >= data.Length) return;
                if (!mask.Get((int)idx)) return;
                data[idx] = default;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Get(in uint idx) {
                if (idx >= data.Length) return default;
                if (!mask.Get((int)idx)) return default;
                return data[idx];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Ref(in uint idx, ref T c) {
                if (idx >= data.Length) return false;
                if (!mask.Get((int)idx)) return false;
                c = ref data[idx];
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(float delta) {
            foreach (var system in systems)
                system.Update(delta);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddSystem(ArraySystem system) {
            system.world = this;
            systems.Add(system);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
