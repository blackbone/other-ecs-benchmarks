namespace Benchmark;

public static class Constants
{
#if SHORT_RUN
    public const int EntityCount = 500;
#else
    public const int EntityCount = 500_000;
#endif
}