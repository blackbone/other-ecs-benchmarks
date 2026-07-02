using System.Runtime.CompilerServices;
using Benchmark;
using Xeno;

namespace Benchmark.Xeno;


[RegisterComponent(typeof(Component1), Inline = true)]
[RegisterComponent(typeof(Component2), Inline = true)]
[RegisterComponent(typeof(Component3), Inline = true)]
[RegisterComponent(typeof(Component4), Inline = true)]
[RegisterComponent(typeof(Padding1), Inline = true)]
public partial class XenoEntityWorld_Inline : World
{
    public partial Entity CreateEntity(in Component1 c1, in Component2 c2);
    public partial Entity CreateEntity(in Component2 c1, in Component3 c2);
    public partial Entity CreateEntity(in Component3 c1, in Component4 c2);
    public partial Entity CreateEntity(in Component4 c1, in Component1 c2);

    public partial Entity CreateEntity(in Component1 c1, in Component2 c2, in Component3 c3);
    public partial Entity CreateEntity(in Component2 c1, in Component3 c2, in Component4 c3);
    public partial Entity CreateEntity(in Component3 c1, in Component4 c2, in Component1 c3);
    public partial Entity CreateEntity(in Component4 c1, in Component1 c2, in Component2 c3);

    public partial Entity CreateEntity(in Component1 c1, in Component2 c2, in Component3 c3, in Component4 c4);

    public partial void Add(in Entity entity, in Component1 c1, in Component2 c2);
    public partial void Add(in Entity entity, in Component2 c1, in Component3 c2);
    public partial void Add(in Entity entity, in Component3 c1, in Component4 c2);
    public partial void Add(in Entity entity, in Component4 c1, in Component1 c2);

    public partial void Add(in Entity entity, in Component1 c1, in Component2 c2, in Component3 c3);
    public partial void Add(in Entity entity, in Component2 c1, in Component3 c2, in Component4 c3);
    public partial void Add(in Entity entity, in Component3 c1, in Component4 c2, in Component1 c3);
    public partial void Add(in Entity entity, in Component4 c1, in Component1 c2, in Component2 c3);

    public partial void Add(in Entity entity, in Component1 c1, in Component2 c2, in Component3 c3, in Component4 c4);

    public partial void RemoveComponent1AndComponent2(in Entity entity);
    public partial void RemoveComponent2AndComponent3(in Entity entity);
    public partial void RemoveComponent3AndComponent4(in Entity entity);
    public partial void RemoveComponent4AndComponent1(in Entity entity);

    public partial void RemoveComponent1AndComponent2AndComponent3(in Entity entity);
    public partial void RemoveComponent2AndComponent3AndComponent4(in Entity entity);
    public partial void RemoveComponent3AndComponent4AndComponent1(in Entity entity);
    public partial void RemoveComponent4AndComponent1AndComponent2(in Entity entity);

    public partial void RemoveComponent1AndComponent2AndComponent3AndComponent4(in Entity entity);
}

[RegisterComponent(typeof(Component1), Inline = true)]
[RegisterComponent(typeof(Padding1), Inline = true)]
[RegisterSystem(typeof(XenoSystem1<Component1>), 0, BakeQuery = true)]
public partial class XenoSystem1World_Inline : World
{
}

[RegisterComponent(typeof(Component1))]
[RegisterComponent(typeof(Component2))]
[RegisterSystem(typeof(XenoSystem2<Component1, Component2>), 0, BakeQuery = true)]
public partial class XenoSystem2World_Inline : World
{
    public partial Entity CreateEntity(in Component1 c1, in Component2 c2);
}

[RegisterComponent(typeof(Component1), Inline = true)]
[RegisterComponent(typeof(Component2), Inline = true)]
[RegisterComponent(typeof(Component3), Inline = true)]
[RegisterSystem(typeof(XenoSystem3<Component1, Component2, Component3>), 0, BakeQuery = true)]
public partial class XenoSystem3World_Inline : World
{
    public partial Entity CreateEntity(in Component1 c1, in Component2 c2, in Component3 c3);
}

[RegisterComponent(typeof(Component1), Inline = true)]
[RegisterComponent(typeof(Component2), Inline = true)]
[RegisterSystem(typeof(XenoSystem1<Component1>), 0, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem1<Component2>), 1, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component1, Component2>), 2, BakeQuery = true)]
public partial class XenoMultiSystemsWorld_Inline : World
{
    public partial Entity CreateEntity(in Component1 c1, in Component2 c2);
}

[RegisterComponent(typeof(Component1), Inline = true)]
[RegisterComponent(typeof(Component2), Inline = true)]
[RegisterComponent(typeof(Component3), Inline = true)]
[RegisterComponent(typeof(Component4), Inline = true)]
[RegisterComponent(typeof(Component5), Inline = true)]
[RegisterComponent(typeof(Component6), Inline = true)]
[RegisterComponent(typeof(Component7), Inline = true)]
[RegisterComponent(typeof(Component8), Inline = true)]
[RegisterComponent(typeof(Component9), Inline = true)]
[RegisterComponent(typeof(Component10), Inline = true)]
[RegisterComponent(typeof(Component11), Inline = true)]
[RegisterComponent(typeof(Component12), Inline = true)]
[RegisterComponent(typeof(Component13), Inline = true)]
[RegisterComponent(typeof(Component14), Inline = true)]
[RegisterComponent(typeof(Component15), Inline = true)]
[RegisterComponent(typeof(Component16), Inline = true)]
[RegisterComponent(typeof(Component17), Inline = true)]
[RegisterComponent(typeof(Component18), Inline = true)]
[RegisterComponent(typeof(Component19), Inline = true)]
[RegisterComponent(typeof(Component20), Inline = true)]
[RegisterComponent(typeof(Component21), Inline = true)]
[RegisterComponent(typeof(Component22), Inline = true)]
[RegisterComponent(typeof(Component23), Inline = true)]
[RegisterComponent(typeof(Component24), Inline = true)]
[RegisterComponent(typeof(Component25), Inline = true)]
[RegisterComponent(typeof(Component26), Inline = true)]
[RegisterComponent(typeof(Component27), Inline = true)]
[RegisterComponent(typeof(Component28), Inline = true)]
[RegisterComponent(typeof(Component29), Inline = true)]
[RegisterComponent(typeof(Component30), Inline = true)]
[RegisterComponent(typeof(Component31), Inline = true)]
[RegisterComponent(typeof(Component32), Inline = true)]
[RegisterComponent(typeof(Component33), Inline = true)]
[RegisterComponent(typeof(Component34), Inline = true)]
[RegisterComponent(typeof(Component35), Inline = true)]
[RegisterComponent(typeof(Component36), Inline = true)]
[RegisterComponent(typeof(Component37), Inline = true)]
[RegisterComponent(typeof(Component38), Inline = true)]
[RegisterComponent(typeof(Component39), Inline = true)]
[RegisterComponent(typeof(Component40), Inline = true)]
[RegisterComponent(typeof(Component41), Inline = true)]
[RegisterComponent(typeof(Component42), Inline = true)]
[RegisterComponent(typeof(Component43), Inline = true)]
[RegisterComponent(typeof(Component44), Inline = true)]
[RegisterComponent(typeof(Component45), Inline = true)]
[RegisterComponent(typeof(Component46), Inline = true)]
[RegisterComponent(typeof(Component47), Inline = true)]
[RegisterComponent(typeof(Component48), Inline = true)]
[RegisterComponent(typeof(Component49), Inline = true)]
[RegisterComponent(typeof(Component50), Inline = true)]
[RegisterComponent(typeof(Component51), Inline = true)]
[RegisterComponent(typeof(Component52), Inline = true)]
[RegisterComponent(typeof(Component53), Inline = true)]
[RegisterComponent(typeof(Component54), Inline = true)]
[RegisterComponent(typeof(Component55), Inline = true)]
[RegisterComponent(typeof(Component56), Inline = true)]
[RegisterComponent(typeof(Component57), Inline = true)]
[RegisterComponent(typeof(Component58), Inline = true)]
[RegisterComponent(typeof(Component59), Inline = true)]
[RegisterComponent(typeof(Component60), Inline = true)]
[RegisterComponent(typeof(Component61), Inline = true)]
[RegisterComponent(typeof(Component62), Inline = true)]
[RegisterComponent(typeof(Component63), Inline = true)]
[RegisterComponent(typeof(Component64), Inline = true)]
[RegisterComponent(typeof(Component65), Inline = true)]
[RegisterComponent(typeof(Component66), Inline = true)]
[RegisterComponent(typeof(Component67), Inline = true)]
[RegisterComponent(typeof(Component68), Inline = true)]
[RegisterComponent(typeof(Component69), Inline = true)]
[RegisterComponent(typeof(Component70), Inline = true)]
[RegisterComponent(typeof(Component71), Inline = true)]
[RegisterComponent(typeof(Component72), Inline = true)]
[RegisterComponent(typeof(Component73), Inline = true)]
[RegisterComponent(typeof(Component74), Inline = true)]
[RegisterComponent(typeof(Component75), Inline = true)]
[RegisterComponent(typeof(Component76), Inline = true)]
[RegisterComponent(typeof(Component77), Inline = true)]
[RegisterComponent(typeof(Component78), Inline = true)]
[RegisterComponent(typeof(Component79), Inline = true)]
[RegisterComponent(typeof(Component80), Inline = true)]
[RegisterComponent(typeof(Component81), Inline = true)]
[RegisterComponent(typeof(Component82), Inline = true)]
[RegisterComponent(typeof(Component83), Inline = true)]
[RegisterComponent(typeof(Component84), Inline = true)]
[RegisterComponent(typeof(Component85), Inline = true)]
[RegisterComponent(typeof(Component86), Inline = true)]
[RegisterComponent(typeof(Component87), Inline = true)]
[RegisterComponent(typeof(Component88), Inline = true)]
[RegisterComponent(typeof(Component89), Inline = true)]
[RegisterComponent(typeof(Component90), Inline = true)]
[RegisterComponent(typeof(Component91), Inline = true)]
[RegisterComponent(typeof(Component92), Inline = true)]
[RegisterComponent(typeof(Component93), Inline = true)]
[RegisterComponent(typeof(Component94), Inline = true)]
[RegisterComponent(typeof(Component95), Inline = true)]
[RegisterComponent(typeof(Component96), Inline = true)]
[RegisterComponent(typeof(Component97), Inline = true)]
[RegisterComponent(typeof(Component98), Inline = true)]
[RegisterComponent(typeof(Component99), Inline = true)]
[RegisterComponent(typeof(Component100), Inline = true)]
[RegisterComponent(typeof(Padding1), Inline = true)]
[RegisterSystem(typeof(XenoSystem2<Component1, Component2>), 0, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component2, Component3>), 1, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component3, Component4>), 2, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component4, Component5>), 3, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component5, Component6>), 4, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component6, Component7>), 5, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component7, Component8>), 6, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component8, Component9>), 7, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component9, Component10>), 8, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component10, Component11>), 9, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component11, Component12>), 10, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component12, Component13>), 11, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component13, Component14>), 12, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component14, Component15>), 13, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component15, Component16>), 14, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component16, Component17>), 15, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component17, Component18>), 16, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component18, Component19>), 17, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component19, Component20>), 18, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component20, Component21>), 19, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component21, Component22>), 20, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component22, Component23>), 21, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component23, Component24>), 22, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component24, Component25>), 23, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component25, Component26>), 24, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component26, Component27>), 25, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component27, Component28>), 26, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component28, Component29>), 27, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component29, Component30>), 28, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component30, Component31>), 29, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component31, Component32>), 30, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component32, Component33>), 31, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component33, Component34>), 32, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component34, Component35>), 33, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component35, Component36>), 34, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component36, Component37>), 35, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component37, Component38>), 36, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component38, Component39>), 37, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component39, Component40>), 38, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component40, Component41>), 39, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component41, Component42>), 40, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component42, Component43>), 41, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component43, Component44>), 42, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component44, Component45>), 43, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component45, Component46>), 44, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component46, Component47>), 45, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component47, Component48>), 46, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component48, Component49>), 47, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component49, Component50>), 48, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component50, Component51>), 49, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component51, Component52>), 50, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component52, Component53>), 51, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component53, Component54>), 52, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component54, Component55>), 53, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component55, Component56>), 54, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component56, Component57>), 55, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component57, Component58>), 56, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component58, Component59>), 57, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component59, Component60>), 58, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component60, Component61>), 59, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component61, Component62>), 60, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component62, Component63>), 61, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component63, Component64>), 62, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component64, Component65>), 63, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component65, Component66>), 64, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component66, Component67>), 65, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component67, Component68>), 66, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component68, Component69>), 67, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component69, Component70>), 68, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component70, Component71>), 69, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component71, Component72>), 70, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component72, Component73>), 71, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component73, Component74>), 72, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component74, Component75>), 73, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component75, Component76>), 74, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component76, Component77>), 75, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component77, Component78>), 76, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component78, Component79>), 77, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component79, Component80>), 78, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component80, Component81>), 79, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component81, Component82>), 80, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component82, Component83>), 81, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component83, Component84>), 82, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component84, Component85>), 83, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component85, Component86>), 84, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component86, Component87>), 85, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component87, Component88>), 86, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component88, Component89>), 87, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component89, Component90>), 88, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component90, Component91>), 89, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component91, Component92>), 90, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component92, Component93>), 91, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component93, Component94>), 92, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component94, Component95>), 93, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component95, Component96>), 94, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component96, Component97>), 95, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component97, Component98>), 96, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component98, Component99>), 97, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component99, Component100>), 98, BakeQuery = true)]
[RegisterSystem(typeof(XenoSystem2<Component100, Component1>), 99, BakeQuery = true)]
public partial class XenoFilterMismatchWorld_Inline : World
{
}
