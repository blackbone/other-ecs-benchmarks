using MorpehComponent = Scellecs.Morpeh.IComponent;
using DragonComponent = DCFApixels.DragonECS.IEcsComponent;

namespace Benchmark;

// for compatibility all components are unmanaged structs
public struct Component1 : MorpehComponent, DragonComponent
{
    public int Value;
}

public struct Component2 : MorpehComponent, DragonComponent
{
    public int Value;
}

public struct Component3 : MorpehComponent, DragonComponent
{
    public int Value;
}

public struct Component4 : MorpehComponent, DragonComponent
{
    public int Value;
}

// for compatibility all components are unmanaged structs
public struct Outliner1 : MorpehComponent, DragonComponent
{
    public long Value1;
    public long Value2;
    public long Value3;
    public long Value4;
}

public struct Outliner2 : MorpehComponent, DragonComponent
{
    public long Value1;
    public long Value2;
    public long Value3;
    public long Value4;
}

public struct Outliner3 : MorpehComponent, DragonComponent
{
    public long Value1;
    public long Value2;
    public long Value3;
    public long Value4;
}

public struct Outliner4 : MorpehComponent, DragonComponent
{
    public long Value1;
    public long Value2;
    public long Value3;
    public long Value4;
}