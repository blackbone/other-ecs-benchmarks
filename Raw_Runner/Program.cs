using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Benchmark;
using Benchmark.Context;
using Benchmark.Xeno;
// using Benchmark.Generated;
using BenchmarkDotNet.Attributes;

const bool IS_ITERATIVE_RUN = true;
const int ITERATIONS_OR_MILLISECONDS = 10;
const int ENTITY_COUNT = 1_000_000;

BenchmarkHash.Enabled = false;

var selectedBench = Environment.GetEnvironmentVariable("RAW_BENCH");

if (!string.IsNullOrWhiteSpace(selectedBench)) {
    var type = AppDomain.CurrentDomain
        .GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .FirstOrDefault(t => t.Name == selectedBench);
    if (type == null)
        throw new InvalidOperationException($"Benchmark type '{selectedBench}' not found.");

    RunBenchType(type);
    return;
}

RunBenchType(typeof(FilterMismatchSystems_MassiveEcsContext));
RunBenchType(typeof(FilterMismatchSystems_StaticEcsContext));
RunBenchType(typeof(FilterMismatchSystems_LeoEcsLiteContext));
RunBenchType(typeof(FilterMismatchSystems_MorpehContext));
RunBenchType(typeof(FilterMismatchSystems_XenoContext));
RunBenchType(typeof(FilterMismatchSystems_XenoContext_Inline));

Console.WriteLine();

RunBenchType(typeof(RareFilterMismatchSystems_MassiveEcsContext));
RunBenchType(typeof(RareFilterMismatchSystems_StaticEcsContext));
RunBenchType(typeof(RareFilterMismatchSystems_LeoEcsLiteContext));
RunBenchType(typeof(RareFilterMismatchSystems_MorpehContext));
RunBenchType(typeof(RareFilterMismatchSystems_XenoContext));
RunBenchType(typeof(RareFilterMismatchSystems_XenoContext_Inline));

Console.WriteLine();

RunBenchType(typeof(SystemWith3Components_MassiveEcsContext));
RunBenchType(typeof(SystemWith3Components_StaticEcsContext));
RunBenchType(typeof(SystemWith3Components_LeoEcsLiteContext));
RunBenchType(typeof(SystemWith3Components_MorpehContext));
RunBenchType(typeof(SystemWith3Components_XenoContext));
RunBenchType(typeof(SystemWith3Components_XenoContext_Inline));

Console.WriteLine();

RunBenchType(typeof(MultiSystems_MassiveEcsContext));
RunBenchType(typeof(MultiSystems_StaticEcsContext));
RunBenchType(typeof(MultiSystems_LeoEcsLiteContext));
RunBenchType(typeof(MultiSystems_MorpehContext));
RunBenchType(typeof(MultiSystems_XenoContext));
RunBenchType(typeof(MultiSystems_XenoContext_Inline));

Console.WriteLine();

RunBenchType(typeof(FourRemoveThreeComponents_MassiveEcsContext));
RunBenchType(typeof(FourRemoveThreeComponents_StaticEcsContext));
RunBenchType(typeof(FourRemoveThreeComponents_LeoEcsLiteContext));
RunBenchType(typeof(FourRemoveThreeComponents_MorpehContext));
RunBenchType(typeof(FourRemoveThreeComponents_XenoContext));
RunBenchType(typeof(FourRemoveThreeComponents_XenoContext_Inline));

Console.WriteLine();

RunBenchType(typeof(OneAddThreeComponents_MassiveEcsContext));
RunBenchType(typeof(OneAddThreeComponents_StaticEcsContext));
RunBenchType(typeof(OneAddThreeComponents_LeoEcsLiteContext));
RunBenchType(typeof(OneAddThreeComponents_MorpehContext));
RunBenchType(typeof(OneAddThreeComponents_XenoContext));
RunBenchType(typeof(OneAddThreeComponents_XenoContext_Inline));

return;


Console.WriteLine("use this for testing and debugging\n\n");

var results = new Dictionary<Type, double[]>();
foreach (var (bench, impls) in BenchMap.Runs) {
    Console.WriteLine(bench.Name + " benchmark runs...\n");

    var res = new double[impls.Length];
    for (var i = 0; i < impls.Length; i++) {
        res[i] = RunBenchType(impls[i]);
    }
    results[bench] = res;

    Console.WriteLine();
}

PrintResults(results);

Console.WriteLine("finished successfully");
return;

double RunBenchType(Type type) {
    if (IS_ITERATIVE_RUN) return RunBenchIterations(type);
    return RunBenchDuration(type);
}


double RunBenchIterations(Type type) {
    Console.Write($"bench {type.Name} ... ");
    var b = (IBenchmark)Activator.CreateInstance(type);
    InjectParameters(b);
    b.GlobalSetup();

    { // warmup
        b.IterationSetup();
        b.Run();
        b.IterationCleanup();
    }

    var n = 0;
    var t = 0.0;
    var iterations = Environment.GetEnvironmentVariable("RAW_ITERATIONS") is { Length: > 0 } rawIterations
                     && int.TryParse(rawIterations, out var parsedIterations)
        ? parsedIterations
        : ITERATIONS_OR_MILLISECONDS;
    while (n < iterations) {
        b.IterationSetup();
        var sw = Stopwatch.StartNew();
        b.Run();
        t += sw.Elapsed.TotalMilliseconds;
        n++;
        b.IterationCleanup();
    }

    b.GlobalCleanup();

    Console.WriteLine($"did {n} iterations for {t / n} ms average");
    return t / n;
}

double RunBenchDuration(Type type) {
    var b = (IBenchmark)Activator.CreateInstance(type);
    InjectParameters(b);
    b.GlobalSetup();

    { // warmup
        b.IterationSetup();
        b.Run();
        b.IterationCleanup();
    }

    var n = 0;
    double t = 0.0;
    var _ = Stopwatch.StartNew();
    var milliseconds = Environment.GetEnvironmentVariable("RAW_ITERATIONS") is { Length: > 0 } rawMilliseconds
                       && int.TryParse(rawMilliseconds, out var parsedMilliseconds)
        ? parsedMilliseconds
        : ITERATIONS_OR_MILLISECONDS;
    while (_.ElapsedMilliseconds < milliseconds) {
        b.IterationSetup();
        var sw = Stopwatch.StartNew();
        b.Run();
        t += sw.Elapsed.TotalMilliseconds;
        n++;
        b.IterationCleanup();
    }

    b.GlobalCleanup();

    Console.WriteLine($"bench {type.Name} did {n} iterations for {t / n} ms average");
    return n;
}

static void InjectParameters(IBenchmark benchmark) {
    benchmark.EntityCount = Environment.GetEnvironmentVariable("RAW_ENTITY_COUNT") is { Length: > 0 } rawEntityCount
                            && int.TryParse(rawEntityCount, out var parsedEntityCount)
        ? parsedEntityCount
        : ENTITY_COUNT;

    var properties = benchmark.GetType()
        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .Where(p => p.Name != nameof(IBenchmark.EntityCount))
        .Select(p => (p, p.GetCustomAttribute<ParamsAttribute>()))
        .Where(kv => kv.Item2 != null)
        .ToArray();
    foreach (var (property, attribute) in properties) {
        property.SetValue(benchmark, attribute.Values[0]);
    }
}

static void PrintResults(Dictionary<Type, double[]> data) {
    // Determine the column widths
    int typeColumnWidth = data.Keys.Max(k => k.Name.Length) + 2;
    var ctxs = BenchMap.Contexts.Keys.ToArray();
    int[] valueColumnWidths = Enumerable
        .Range(0, data.Values.Max(v => v.Length))
        .Select(i => Math.Max(
                ctxs[i].Name.Length + 2,
                data.Values.Where(v => i < v.Length).Max(v => $"{v[i].ToString(IS_ITERATIVE_RUN ? "F4" : "N0")} {(IS_ITERATIVE_RUN ? "ms" : "n")} ".Length)
            ))
        .ToArray();

    // Print header
    Console.Write("Type".PadRight(typeColumnWidth));
    for (int i = 0; i < ctxs.Length; i++) {
        Console.Write("|");
        Console.Write($"{ctxs[i].Name} ".PadLeft(valueColumnWidths[i]));
    }
    Console.WriteLine();

    // Print separator
    Console.WriteLine(new string('-', typeColumnWidth + valueColumnWidths.Sum() + valueColumnWidths.Length * 2 - 1));

    // Print rows
    foreach (var kvp in data)
    {
        Console.Write(kvp.Key.Name.PadRight(typeColumnWidth));
        for (int i = 0; i < valueColumnWidths.Length; i++)
        {
            Console.Write("|");
            if (i < kvp.Value.Length)
                Console.Write($"{kvp.Value[i].ToString(IS_ITERATIVE_RUN ? "F4" : "N0")} {(IS_ITERATIVE_RUN ? "ms" : "n")} ".PadLeft(valueColumnWidths[i]));
            else
                Console.Write(new string(' ', valueColumnWidths[i] + 1));
        }
        Console.WriteLine();
    }
}
//*/
