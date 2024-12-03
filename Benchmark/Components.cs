using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;
using XenoComponent = Xeno.IComponent;
using FrifloComponent = Friflo.Engine.ECS.IComponent;
using StaticEcsComponent = FFS.Libraries.StaticEcs.IComponent;

namespace Benchmark;

// for compatibility all components are unmanaged structs
public struct Component1 : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
{
    public int Value;
}

public struct Component2 : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
{
    public int Value;
}

public struct Component3 : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
{
    public int Value;
}

public struct Component4 : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
{
    public int Value;
}

// for compatibility all components are unmanaged structs
public struct Padding1 : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
{
    public long Value1;
    public long Value2;
    public long Value3;
    public long Value4;
}

public struct Padding2 : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
{
    public long Value1;
    public long Value2;
    public long Value3;
    public long Value4;
}

public struct Padding3 : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
{
    public long Value1;
    public long Value2;
    public long Value3;
    public long Value4;
}

public struct Padding4 : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent
{
    public long Value1;
    public long Value2;
    public long Value3;
    public long Value4;
}