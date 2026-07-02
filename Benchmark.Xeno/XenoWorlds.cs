using System.Runtime.CompilerServices;
using Benchmark;
using Xeno;

namespace Benchmark.Xeno;

[RegisterComponent(typeof(Component1))]
[RegisterComponent(typeof(Component2))]
[RegisterComponent(typeof(Component3))]
[RegisterComponent(typeof(Component4))]
[RegisterComponent(typeof(Padding1))]
public partial class XenoEntityWorld : World
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

[RegisterComponent(typeof(Component1))]
[RegisterComponent(typeof(Padding1))]
[RegisterSystem(typeof(XenoSystem1<Component1>), 0)]
public partial class XenoSystem1World : World
{
}

[RegisterComponent(typeof(Component1))]
[RegisterComponent(typeof(Component2))]
[RegisterSystem(typeof(XenoSystem2<Component1, Component2>), 0)]
public partial class XenoSystem2World : World
{
    public partial Entity CreateEntity(in Component1 c1, in Component2 c2);
}

[RegisterComponent(typeof(Component1))]
[RegisterComponent(typeof(Component2))]
[RegisterComponent(typeof(Component3))]
[RegisterSystem(typeof(XenoSystem3<Component1, Component2, Component3>), 0)]
public partial class XenoSystem3World : World
{
    public partial Entity CreateEntity(in Component1 c1, in Component2 c2, in Component3 c3);
}

[RegisterComponent(typeof(Component1))]
[RegisterComponent(typeof(Component2))]
[RegisterSystem(typeof(XenoSystem1<Component1>), 0)]
[RegisterSystem(typeof(XenoSystem1<Component2>), 1)]
[RegisterSystem(typeof(XenoSystem2<Component1, Component2>), 2)]
public partial class XenoMultiSystemsWorld : World
{
    public partial Entity CreateEntity(in Component1 c1, in Component2 c2);
}

[RegisterComponent(typeof(Component1))]
[RegisterComponent(typeof(Component2))]
[RegisterComponent(typeof(Component3))]
[RegisterComponent(typeof(Component4))]
[RegisterComponent(typeof(Component5))]
[RegisterComponent(typeof(Component6))]
[RegisterComponent(typeof(Component7))]
[RegisterComponent(typeof(Component8))]
[RegisterComponent(typeof(Component9))]
[RegisterComponent(typeof(Component10))]
[RegisterComponent(typeof(Component11))]
[RegisterComponent(typeof(Component12))]
[RegisterComponent(typeof(Component13))]
[RegisterComponent(typeof(Component14))]
[RegisterComponent(typeof(Component15))]
[RegisterComponent(typeof(Component16))]
[RegisterComponent(typeof(Component17))]
[RegisterComponent(typeof(Component18))]
[RegisterComponent(typeof(Component19))]
[RegisterComponent(typeof(Component20))]
[RegisterComponent(typeof(Component21))]
[RegisterComponent(typeof(Component22))]
[RegisterComponent(typeof(Component23))]
[RegisterComponent(typeof(Component24))]
[RegisterComponent(typeof(Component25))]
[RegisterComponent(typeof(Component26))]
[RegisterComponent(typeof(Component27))]
[RegisterComponent(typeof(Component28))]
[RegisterComponent(typeof(Component29))]
[RegisterComponent(typeof(Component30))]
[RegisterComponent(typeof(Component31))]
[RegisterComponent(typeof(Component32))]
[RegisterComponent(typeof(Component33))]
[RegisterComponent(typeof(Component34))]
[RegisterComponent(typeof(Component35))]
[RegisterComponent(typeof(Component36))]
[RegisterComponent(typeof(Component37))]
[RegisterComponent(typeof(Component38))]
[RegisterComponent(typeof(Component39))]
[RegisterComponent(typeof(Component40))]
[RegisterComponent(typeof(Component41))]
[RegisterComponent(typeof(Component42))]
[RegisterComponent(typeof(Component43))]
[RegisterComponent(typeof(Component44))]
[RegisterComponent(typeof(Component45))]
[RegisterComponent(typeof(Component46))]
[RegisterComponent(typeof(Component47))]
[RegisterComponent(typeof(Component48))]
[RegisterComponent(typeof(Component49))]
[RegisterComponent(typeof(Component50))]
[RegisterComponent(typeof(Component51))]
[RegisterComponent(typeof(Component52))]
[RegisterComponent(typeof(Component53))]
[RegisterComponent(typeof(Component54))]
[RegisterComponent(typeof(Component55))]
[RegisterComponent(typeof(Component56))]
[RegisterComponent(typeof(Component57))]
[RegisterComponent(typeof(Component58))]
[RegisterComponent(typeof(Component59))]
[RegisterComponent(typeof(Component60))]
[RegisterComponent(typeof(Component61))]
[RegisterComponent(typeof(Component62))]
[RegisterComponent(typeof(Component63))]
[RegisterComponent(typeof(Component64))]
[RegisterComponent(typeof(Component65))]
[RegisterComponent(typeof(Component66))]
[RegisterComponent(typeof(Component67))]
[RegisterComponent(typeof(Component68))]
[RegisterComponent(typeof(Component69))]
[RegisterComponent(typeof(Component70))]
[RegisterComponent(typeof(Component71))]
[RegisterComponent(typeof(Component72))]
[RegisterComponent(typeof(Component73))]
[RegisterComponent(typeof(Component74))]
[RegisterComponent(typeof(Component75))]
[RegisterComponent(typeof(Component76))]
[RegisterComponent(typeof(Component77))]
[RegisterComponent(typeof(Component78))]
[RegisterComponent(typeof(Component79))]
[RegisterComponent(typeof(Component80))]
[RegisterComponent(typeof(Component81))]
[RegisterComponent(typeof(Component82))]
[RegisterComponent(typeof(Component83))]
[RegisterComponent(typeof(Component84))]
[RegisterComponent(typeof(Component85))]
[RegisterComponent(typeof(Component86))]
[RegisterComponent(typeof(Component87))]
[RegisterComponent(typeof(Component88))]
[RegisterComponent(typeof(Component89))]
[RegisterComponent(typeof(Component90))]
[RegisterComponent(typeof(Component91))]
[RegisterComponent(typeof(Component92))]
[RegisterComponent(typeof(Component93))]
[RegisterComponent(typeof(Component94))]
[RegisterComponent(typeof(Component95))]
[RegisterComponent(typeof(Component96))]
[RegisterComponent(typeof(Component97))]
[RegisterComponent(typeof(Component98))]
[RegisterComponent(typeof(Component99))]
[RegisterComponent(typeof(Component100))]
[RegisterComponent(typeof(Padding1))]
[RegisterSystem(typeof(XenoSystem2<Component1, Component2>), 0)]
[RegisterSystem(typeof(XenoSystem2<Component2, Component3>), 1)]
[RegisterSystem(typeof(XenoSystem2<Component3, Component4>), 2)]
[RegisterSystem(typeof(XenoSystem2<Component4, Component5>), 3)]
[RegisterSystem(typeof(XenoSystem2<Component5, Component6>), 4)]
[RegisterSystem(typeof(XenoSystem2<Component6, Component7>), 5)]
[RegisterSystem(typeof(XenoSystem2<Component7, Component8>), 6)]
[RegisterSystem(typeof(XenoSystem2<Component8, Component9>), 7)]
[RegisterSystem(typeof(XenoSystem2<Component9, Component10>), 8)]
[RegisterSystem(typeof(XenoSystem2<Component10, Component11>), 9)]
[RegisterSystem(typeof(XenoSystem2<Component11, Component12>), 10)]
[RegisterSystem(typeof(XenoSystem2<Component12, Component13>), 11)]
[RegisterSystem(typeof(XenoSystem2<Component13, Component14>), 12)]
[RegisterSystem(typeof(XenoSystem2<Component14, Component15>), 13)]
[RegisterSystem(typeof(XenoSystem2<Component15, Component16>), 14)]
[RegisterSystem(typeof(XenoSystem2<Component16, Component17>), 15)]
[RegisterSystem(typeof(XenoSystem2<Component17, Component18>), 16)]
[RegisterSystem(typeof(XenoSystem2<Component18, Component19>), 17)]
[RegisterSystem(typeof(XenoSystem2<Component19, Component20>), 18)]
[RegisterSystem(typeof(XenoSystem2<Component20, Component21>), 19)]
[RegisterSystem(typeof(XenoSystem2<Component21, Component22>), 20)]
[RegisterSystem(typeof(XenoSystem2<Component22, Component23>), 21)]
[RegisterSystem(typeof(XenoSystem2<Component23, Component24>), 22)]
[RegisterSystem(typeof(XenoSystem2<Component24, Component25>), 23)]
[RegisterSystem(typeof(XenoSystem2<Component25, Component26>), 24)]
[RegisterSystem(typeof(XenoSystem2<Component26, Component27>), 25)]
[RegisterSystem(typeof(XenoSystem2<Component27, Component28>), 26)]
[RegisterSystem(typeof(XenoSystem2<Component28, Component29>), 27)]
[RegisterSystem(typeof(XenoSystem2<Component29, Component30>), 28)]
[RegisterSystem(typeof(XenoSystem2<Component30, Component31>), 29)]
[RegisterSystem(typeof(XenoSystem2<Component31, Component32>), 30)]
[RegisterSystem(typeof(XenoSystem2<Component32, Component33>), 31)]
[RegisterSystem(typeof(XenoSystem2<Component33, Component34>), 32)]
[RegisterSystem(typeof(XenoSystem2<Component34, Component35>), 33)]
[RegisterSystem(typeof(XenoSystem2<Component35, Component36>), 34)]
[RegisterSystem(typeof(XenoSystem2<Component36, Component37>), 35)]
[RegisterSystem(typeof(XenoSystem2<Component37, Component38>), 36)]
[RegisterSystem(typeof(XenoSystem2<Component38, Component39>), 37)]
[RegisterSystem(typeof(XenoSystem2<Component39, Component40>), 38)]
[RegisterSystem(typeof(XenoSystem2<Component40, Component41>), 39)]
[RegisterSystem(typeof(XenoSystem2<Component41, Component42>), 40)]
[RegisterSystem(typeof(XenoSystem2<Component42, Component43>), 41)]
[RegisterSystem(typeof(XenoSystem2<Component43, Component44>), 42)]
[RegisterSystem(typeof(XenoSystem2<Component44, Component45>), 43)]
[RegisterSystem(typeof(XenoSystem2<Component45, Component46>), 44)]
[RegisterSystem(typeof(XenoSystem2<Component46, Component47>), 45)]
[RegisterSystem(typeof(XenoSystem2<Component47, Component48>), 46)]
[RegisterSystem(typeof(XenoSystem2<Component48, Component49>), 47)]
[RegisterSystem(typeof(XenoSystem2<Component49, Component50>), 48)]
[RegisterSystem(typeof(XenoSystem2<Component50, Component51>), 49)]
[RegisterSystem(typeof(XenoSystem2<Component51, Component52>), 50)]
[RegisterSystem(typeof(XenoSystem2<Component52, Component53>), 51)]
[RegisterSystem(typeof(XenoSystem2<Component53, Component54>), 52)]
[RegisterSystem(typeof(XenoSystem2<Component54, Component55>), 53)]
[RegisterSystem(typeof(XenoSystem2<Component55, Component56>), 54)]
[RegisterSystem(typeof(XenoSystem2<Component56, Component57>), 55)]
[RegisterSystem(typeof(XenoSystem2<Component57, Component58>), 56)]
[RegisterSystem(typeof(XenoSystem2<Component58, Component59>), 57)]
[RegisterSystem(typeof(XenoSystem2<Component59, Component60>), 58)]
[RegisterSystem(typeof(XenoSystem2<Component60, Component61>), 59)]
[RegisterSystem(typeof(XenoSystem2<Component61, Component62>), 60)]
[RegisterSystem(typeof(XenoSystem2<Component62, Component63>), 61)]
[RegisterSystem(typeof(XenoSystem2<Component63, Component64>), 62)]
[RegisterSystem(typeof(XenoSystem2<Component64, Component65>), 63)]
[RegisterSystem(typeof(XenoSystem2<Component65, Component66>), 64)]
[RegisterSystem(typeof(XenoSystem2<Component66, Component67>), 65)]
[RegisterSystem(typeof(XenoSystem2<Component67, Component68>), 66)]
[RegisterSystem(typeof(XenoSystem2<Component68, Component69>), 67)]
[RegisterSystem(typeof(XenoSystem2<Component69, Component70>), 68)]
[RegisterSystem(typeof(XenoSystem2<Component70, Component71>), 69)]
[RegisterSystem(typeof(XenoSystem2<Component71, Component72>), 70)]
[RegisterSystem(typeof(XenoSystem2<Component72, Component73>), 71)]
[RegisterSystem(typeof(XenoSystem2<Component73, Component74>), 72)]
[RegisterSystem(typeof(XenoSystem2<Component74, Component75>), 73)]
[RegisterSystem(typeof(XenoSystem2<Component75, Component76>), 74)]
[RegisterSystem(typeof(XenoSystem2<Component76, Component77>), 75)]
[RegisterSystem(typeof(XenoSystem2<Component77, Component78>), 76)]
[RegisterSystem(typeof(XenoSystem2<Component78, Component79>), 77)]
[RegisterSystem(typeof(XenoSystem2<Component79, Component80>), 78)]
[RegisterSystem(typeof(XenoSystem2<Component80, Component81>), 79)]
[RegisterSystem(typeof(XenoSystem2<Component81, Component82>), 80)]
[RegisterSystem(typeof(XenoSystem2<Component82, Component83>), 81)]
[RegisterSystem(typeof(XenoSystem2<Component83, Component84>), 82)]
[RegisterSystem(typeof(XenoSystem2<Component84, Component85>), 83)]
[RegisterSystem(typeof(XenoSystem2<Component85, Component86>), 84)]
[RegisterSystem(typeof(XenoSystem2<Component86, Component87>), 85)]
[RegisterSystem(typeof(XenoSystem2<Component87, Component88>), 86)]
[RegisterSystem(typeof(XenoSystem2<Component88, Component89>), 87)]
[RegisterSystem(typeof(XenoSystem2<Component89, Component90>), 88)]
[RegisterSystem(typeof(XenoSystem2<Component90, Component91>), 89)]
[RegisterSystem(typeof(XenoSystem2<Component91, Component92>), 90)]
[RegisterSystem(typeof(XenoSystem2<Component92, Component93>), 91)]
[RegisterSystem(typeof(XenoSystem2<Component93, Component94>), 92)]
[RegisterSystem(typeof(XenoSystem2<Component94, Component95>), 93)]
[RegisterSystem(typeof(XenoSystem2<Component95, Component96>), 94)]
[RegisterSystem(typeof(XenoSystem2<Component96, Component97>), 95)]
[RegisterSystem(typeof(XenoSystem2<Component97, Component98>), 96)]
[RegisterSystem(typeof(XenoSystem2<Component98, Component99>), 97)]
[RegisterSystem(typeof(XenoSystem2<Component99, Component100>), 98)]
[RegisterSystem(typeof(XenoSystem2<Component100, Component1>), 99)]
public partial class XenoFilterMismatchWorld : World
{
}
