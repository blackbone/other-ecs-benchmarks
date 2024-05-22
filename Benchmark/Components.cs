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

public struct Component5 : MorpehComponent, DragonComponent
{
    public int Value;
}