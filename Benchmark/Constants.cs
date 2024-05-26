namespace Benchmark;

public static class Constants
{
    public const int Seed = 0x7ADE7455;
    public const int SmallEntityCount = 500;
    public const int MidEntityCount = 20_000;
    public const int LargeEntityCount = 500_000;
    
#if SHORT_RUN
    public const int EntityCount = SmallEntityCount;
    public const int SystemEntityCount = SmallEntityCount;
#else
    public const int EntityCount = LargeEntityCount;
    public const int SystemEntityCount = MidEntityCount;
#endif
    
    
}