namespace Benchmark;

public class Options
{
    public bool PrintList { get; set; }
    public string[] Contexts { get; set; }

    // single benchmark run
    public string Benchmark { get; set; }

    // multi benchmarks run
    public string[] Benchmarks { get; set; }

    public static Options Parse(in string[] args)
    {
        var result = new Options();

        var i = 0;
        while (i < args.Length)
        {
            if (args[i] == "--list")
            {
                result.PrintList = true;
                break;
            }

            if (args[i].StartsWith("contexts=")) result.Contexts = args[i].Split("=")[1].Split(",");
            if (args[i].StartsWith("benchmarks=")) result.Benchmarks = args[i].Split("=")[1].Split(",");
            if (args[i].StartsWith("benchmark=")) result.Benchmark = args[i].Split("=")[1];
            i++;
        }

        return result;
    }
}