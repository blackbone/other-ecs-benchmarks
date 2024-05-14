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
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

PreloadAssemblies();

// configure runner
IConfig configuration = DefaultConfig.Instance
        .AddJob(Job.Default
            .WithUnrollFactor(16)
            .WithStrategy(RunStrategy.Throughput)
            .WithAnalyzeLaunchVariance(true)
            .Apply())
        .AddExporter(MarkdownExporter.GitHub)
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .WithOption(ConfigOptions.JoinSummary, true)
        .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest))
        .HideColumns(Column.Gen0, Column.Gen1, Column.Gen2, Column.Error, Column.StdDev, Column.Method)
    ;

var contextTypes = GetNestedTypes(typeof(BenchmarkContextBase),
    static t => t is { IsAbstract: false, IsGenericType: false });
var baseBenchmarkTypes =
    GetNestedTypes(typeof(BenchmarkBase), static t => t is { IsAbstract: false, IsGenericType: true });

// run benchmarks
foreach (var baseBenchmarkType in baseBenchmarkTypes)
{
    var benchmarkTypes = contextTypes.Select(contextType => baseBenchmarkType.MakeGenericType(contextType)).ToArray();
    var benchmarkSwitcher = BenchmarkSwitcher.FromTypes(benchmarkTypes.ToArray());
    if (args.Length > 0) benchmarkSwitcher.Run(args, configuration);
    else benchmarkSwitcher.RunAll(configuration);
}

// join reports
var contents = Directory.GetFiles("./.benchmark_results", "*.md", SearchOption.AllDirectories)
    .Order()
    .Select(file => (Regex.Match(file, @"\.benchmark_results/(?'name'\w+)/").Groups["name"].Value,
        File.ReadLines(file).ToArray()))
    .ToArray();

var content = new List<string>();
content.AddRange(contents[0].Item2[..11]);
content.Add(string.Empty);
foreach (var (benchmark, reportContent) in contents)
{
    content.Add($"# {benchmark}");
    content.AddRange(reportContent[11..]);
    content.Add(string.Empty);
}

File.WriteAllText("./report.md", string.Join("\r\n", content), Encoding.UTF8);

return 0;

void PreloadAssemblies()
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

Type[] GetNestedTypes(Type baseType, Predicate<Type> filter)
{
    return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
        .Where(t => t.IsSubclassOf(baseType) && filter(t)).ToArray();
}