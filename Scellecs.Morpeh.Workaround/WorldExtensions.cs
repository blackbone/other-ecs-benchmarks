namespace Scellecs.Morpeh.Workaround;

public static class WorldExtensions
{
    public static int EntityCount(this World world)
    {
        return world.entitiesCount;
    }
}