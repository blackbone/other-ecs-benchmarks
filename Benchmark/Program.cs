// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Benchmark;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

// parse cmd args
var options = ParseCommandLineArgs(args);

// preload assemblies
PreloadAssemblies();

// configure jobs
var shortJob = Job.ShortRun
    .WithStrategy(RunStrategy.Monitoring)
    .Apply();

var clearEachInvocationJob = Job.Dry
    .WithInvocationCount(1)
    .WithIterationCount(1)
    .WithStrategy(RunStrategy.Throughput)
    .Apply();
var precisionJob = Job.Default
    .WithUnrollFactor(16)
    .WithStrategy(RunStrategy.Throughput)
    .Apply();

// configure runner
IConfig configuration = DefaultConfig.Instance
    .AddExporter(MarkdownExporter.GitHub)
    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .WithOption(ConfigOptions.JoinSummary, true)
    .HideColumns(Column.Gen0, Column.Gen1, Column.Gen2, Column.Type, Column.Error, Column.Method, Column.StdDev)
    .AddColumn(new ContextColumn())
    ;

var baseBenchmarkTypes = GetNestedTypes(typeof(BenchmarkBase), static t => t is { IsAbstract: false, IsGenericType: true });
if (!string.IsNullOrEmpty(options.Benchmark)) baseBenchmarkTypes = [baseBenchmarkTypes.FirstOrDefault(ctx => ctx.Name[..^2] == options.Benchmark)];
else if (options.Benchmarks is { Length: > 0 }) baseBenchmarkTypes = baseBenchmarkTypes.Where(ctx => options.Benchmarks.Any(c => ctx.Name.Contains(c))).ToArray();

if (options.PrintList)
{
    File.WriteAllLines("benchmarks.txt", baseBenchmarkTypes.Select(b => b.Name[..^2]));
    return 0;
}

Console.WriteLine("Benchmarks:");
Console.WriteLine(string.Join("\n", baseBenchmarkTypes.Select(b => $"\t{b.Name}")));
Console.WriteLine();

var contextTypes = GetNestedTypes(typeof(BenchmarkContextBase), static t => t is { IsAbstract: false, IsGenericType: false });
if (options.Contexts is { Length: > 0 }) contextTypes = contextTypes.Where(ctx => options.Contexts.Any(c => ctx.Name.Contains(c))).ToArray();

Console.WriteLine("Contexts:");
Console.WriteLine(string.Join("\n", contextTypes.Select(b => $"\t{b.Name}")));
Console.WriteLine();

// run benchmarks
foreach (var baseBenchmarkType in baseBenchmarkTypes)
{
    var benchmarkTypes = contextTypes.Select(contextType => baseBenchmarkType.MakeGenericType(contextType)).ToArray();
    var benchmarkSwitcher = BenchmarkSwitcher.FromTypes(benchmarkTypes.ToArray());
    var perInvocationSetup = baseBenchmarkType.GetCustomAttribute<BenchmarkCategoryAttribute>()?.Categories.Contains(Categories.PerInvocationSetup) ?? false;
    
    #if SHORT_RUN
    benchmarkSwitcher.RunAll(configuration.AddJob(shortJob));
    #else
    benchmarkSwitcher.RunAll(configuration.AddJob(perInvocationSetup ? clearEachInvocationJob : precisionJob));
    #endif
}

// join reports
var contents = Directory.GetFiles("./.benchmark_results", "*.md", SearchOption.AllDirectories)
    .Order()
    .Select(file => (Regex.Match(file, @"\.benchmark_results/(?'name'\w+)/").Groups["name"].Value,
        File.ReadLines(file).ToArray()))
    .ToArray();

// find and add header
var content = new List<string>();
var i = 1;
while (!contents[0].Item2[i].StartsWith("```")) i++;
i++;
content.AddRange(contents[0].Item2[..i]);
content.Add(string.Empty);

// add benchmark contents
foreach (var (benchmark, reportContent) in contents)
{
    content.Add($"# {benchmark}");
    i = 1;
    while (!contents[0].Item2[i].StartsWith("```")) i++;
    i++;
    content.AddRange(reportContent[i..]);
    content.Add(string.Empty);
}

File.WriteAllText("./report.md", string.Join("\r\n", content), Encoding.UTF8);

return 0;

static void PreloadAssemblies()
{
    var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
    if (loadedAssemblies.Count == 0) return;
    var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
    if (loadedPaths.Length == 0) return;
    var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
    if (referencedPaths.Length == 0) return;
    var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase))
        .ToList();
    if (toLoad.Count == 0) return;
    toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
}

static Type[] GetNestedTypes(Type baseType, Predicate<Type> filter)
{
    return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
        .Where(t => t.IsSubclassOf(baseType) && filter(t)).ToArray();
}

static Options ParseCommandLineArgs(in string[] args)
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