using DCFApixels.DragonECS;

namespace Benchmark.DragonECS;

public class Aspect<T1> : EcsAspect
    where T1 : struct, IEcsComponent
{
    public EcsPool<T1> Components1;

    protected override void Init(Builder b)
    {
        base.Init(b);
        Components1 = b.Include<T1>();
    }
}

public class Aspect<T1, T2> : EcsAspect
    where T1 : struct, IEcsComponent
    where T2 : struct, IEcsComponent
{
    public EcsPool<T1> Components1;
    public EcsPool<T2> Components2;

    protected override void Init(Builder b)
    {
        base.Init(b);
        Components1 = b.Include<T1>();
        Components2 = b.Include<T2>();
    }
}

public class Aspect<T1, T2, T3> : EcsAspect
    where T1 : struct, IEcsComponent
    where T2 : struct, IEcsComponent
    where T3 : struct, IEcsComponent
{
    public EcsPool<T1> Components1;
    public EcsPool<T2> Components2;
    public EcsPool<T3> Components3;

    protected override void Init(Builder b)
    {
        base.Init(b);
        Components1 = b.Include<T1>();
        Components2 = b.Include<T2>();
        Components3 = b.Include<T3>();
    }
}

public class Aspect<T1, T2, T3, T4> : EcsAspect
    where T1 : struct, IEcsComponent
    where T2 : struct, IEcsComponent
    where T3 : struct, IEcsComponent
    where T4 : struct, IEcsComponent
{
    public EcsPool<T1> Components1;
    public EcsPool<T2> Components2;
    public EcsPool<T3> Components3;
    public EcsPool<T4> Components4;

    protected override void Init(Builder b)
    {
        base.Init(b);
        Components1 = b.Include<T1>();
        Components2 = b.Include<T2>();
        Components3 = b.Include<T3>();
        Components4 = b.Include<T4>();
    }
}
