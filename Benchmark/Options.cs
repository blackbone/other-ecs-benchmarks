namespace Benchmark;

public class Options
{
    public bool PrintList { get; set; }
    public string[] Contexts { get; set; }

    // single benchmark run
    public string Benchmark { get; set; }

    // multi benchmarks run
    public string[] Benchmarks { get; set; }
}