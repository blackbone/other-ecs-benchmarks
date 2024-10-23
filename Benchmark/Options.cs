namespace Benchmark;

public class Options
{
    public bool PrintList { get; private set; }
    public string[] Contexts { get; private set; }
    public string Benchmark { get; private set; }
    public string[] Benchmarks { get; private set; }
    public bool ShortRun { get; private set; }

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

            if (args[i] == "--short") result.ShortRun = true;
            if (args[i].StartsWith("contexts=")) result.Contexts = args[i].Split("=")[1].Split(",");
            if (args[i].StartsWith("benchmarks=")) result.Benchmarks = args[i].Split("=")[1].Split(",");
            if (args[i].StartsWith("benchmark=")) result.Benchmark = args[i].Split("=")[1];
            i++;
        }

        return result;
    }
}