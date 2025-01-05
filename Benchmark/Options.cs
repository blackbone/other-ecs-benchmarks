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

        var _i = 0;
        while (_i < args.Length)
        {
            if (args[_i] == "--list")
            {
                result.PrintList = true;
                break;
            }

            if (args[_i] == "--short") result.ShortRun = true;
            if (args[_i].StartsWith("contexts=")) result.Contexts = args[_i].Split("=")[1].Split(",");
            if (args[_i].StartsWith("benchmarks=")) result.Benchmarks = args[_i].Split("=")[1].Split(",");
            if (args[_i].StartsWith("benchmark=")) result.Benchmark = args[_i].Split("=")[1];
            _i++;
        }

        return result;
    }
}
