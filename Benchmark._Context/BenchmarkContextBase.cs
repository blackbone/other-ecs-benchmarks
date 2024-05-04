using MorpehComponent = Scellecs.Morpeh.IComponent;


namespace Benchmark._Context
{
    public abstract class BenchmarkContextBase : IDisposable
    {
        public abstract void Setup(int entityCount);
        public abstract void Cleanup();
        public void Dispose() { }
        
        public abstract void Commit();

        public abstract int CreateEntity();
        public abstract int CreateEntity<T1>() where T1 : struct, MorpehComponent;
        public abstract int CreateEntity<T1, T2>() where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent;
        public abstract int CreateEntity<T1, T2, T3>() where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent where T3 : struct, MorpehComponent;
        public abstract int CreateEntity<T1, T2, T3, T4>() where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent where T3 : struct, MorpehComponent where T4 : struct, MorpehComponent;

        public abstract void AddComponent<T1>(in int id) where T1 : struct, MorpehComponent;
        public abstract void AddComponent<T1, T2>(in int id) where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent;
        public abstract void AddComponent<T1, T2, T3>(in int id) where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent where T3 : struct, MorpehComponent;
        public abstract void AddComponent<T1, T2, T3, T4>(in int id) where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent where T3 : struct, MorpehComponent where T4 : struct, MorpehComponent;
        
        public abstract void RemoveComponent<T1>(in int id) where T1 : struct, MorpehComponent;
        public abstract void RemoveComponent<T1, T2>(in int id) where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent;
        public abstract void RemoveComponent<T1, T2, T3>(in int id) where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent where T3 : struct, MorpehComponent;
        public abstract void RemoveComponent<T1, T2, T3, T4>(in int id) where T1 : struct, MorpehComponent where T2 : struct, MorpehComponent where T3 : struct, MorpehComponent where T4 : struct, MorpehComponent;
    }
}