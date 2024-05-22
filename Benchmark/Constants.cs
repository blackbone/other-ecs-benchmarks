namespace Benchmark;

public static class Constants
{
    public const int Seed = 0x7ADE7455;
    
#if SHORT_RUN
    public const int EntityCount = 500;
#else
    public const int EntityCount = 500_000;
#endif
}