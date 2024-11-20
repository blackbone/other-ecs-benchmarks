// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Benchmark;
using Benchmark.Utils;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
#if DEBUG
using BenchmarkDotNet.Toolchains.InProcess.Emit;
#endif

// clear previous results
if (Directory.Exists(".benchmark_results"))
    Directory.Delete(".benchmark_results", true);

// parse cmd args
var options = Options.Parse(args);
Console.WriteLine("Using Options:");
Console.WriteLine(JsonSerializer.Serialize(options, JsonSerializerOptions.Default));

// configure jobs
var shortJob = Job.ShortRun
#if DEBUG
    .WithToolchain(InProcessEmitToolchain.Instance)
#endif
    .WithStrategy(RunStrategy.Monitoring)
    .WithInvocationCount(1)
    .WithIterationCount(4)
    .WithEvaluateOverhead(false)
    .WithWarmupCount(0)
    .Apply();

var precisionJob = Job.Default
#if DEBUG
    .WithToolchain(InProcessEmitToolchain.Instance)
#endif
    .WithStrategy(RunStrategy.Throughput)
    .WithUnrollFactor(1)
    .Apply();

// configure runner
IConfig configuration = DefaultConfig.Instance
    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .WithOption(ConfigOptions.JoinSummary, true)
    .AddColumn(new ContextColumn())
    .WithSummaryStyle(SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend))
    .HideColumns(Column.Gen0, Column.Gen1, Column.Gen2, Column.Type, Column.Method, Column.Namespace);

if (options.PrintList)
{
    File.WriteAllText("benchmarks.txt", string.Join("\n", BenchMap.Runs.Keys.Select(b => b.Name[..^2])));
    return 0;
}

var baseBenchmarkTypes = BenchMap.Runs.Keys.ToArray();
if (!string.IsNullOrEmpty(options.Benchmark))
    baseBenchmarkTypes = [baseBenchmarkTypes.FirstOrDefault(ctx => ctx.Name[..^2] == options.Benchmark)];
else if (options.Benchmarks is { Length: > 0 })
    baseBenchmarkTypes = baseBenchmarkTypes.Where(ctx => options.Benchmarks.Any(c => ctx.Name.Contains(c))).ToArray();

Console.WriteLine("Benchmarks:");
Console.WriteLine(string.Join("\n", baseBenchmarkTypes.Select(b => $"\t{b.Name}")));
Console.WriteLine();

var contextTypes = BenchMap.Contexts.Keys.ToArray();
if (options.Contexts is { Length: > 0 })
    contextTypes = contextTypes.Where(ctx => options.Contexts.Any(c => ctx.Name.Contains(c))).ToArray();

Console.WriteLine("Contexts:");
Console.WriteLine(string.Join("\n", contextTypes.Select(b => $"\t{b.Name}")));
Console.WriteLine();

Console.WriteLine($"{baseBenchmarkTypes.Length * contextTypes.Length} total benchmarks.");

// run benchmarks
foreach (var baseBenchmarkType in baseBenchmarkTypes)
{
    // this is all benchmarks
    var benchmarkAllTypes = BenchMap.Runs[baseBenchmarkType];

    // we keep only those which intersects with lists of selected context types
    var benchmarkTypes = new List<Type>();
    foreach (var contextBenchTypes in contextTypes.Select(t => BenchMap.Contexts[t]))
        benchmarkTypes.AddRange(benchmarkAllTypes.Where(t => contextBenchTypes.Contains(t)));

    var benchmarkSwitcher = BenchmarkSwitcher.FromTypes(benchmarkTypes.ToArray());

    var job = options.ShortRun ? shortJob : precisionJob;
    var summaries = benchmarkSwitcher.RunAll(configuration.AddJob(job)).ToArray();

    // post process benchmark reports
    foreach (var summary in summaries)
    {
        var rootDir = summary.ResultsDirectoryPath;
        var rootDirInfo = new DirectoryInfo(rootDir);
        var name = rootDirInfo.Parent.Name;

        var reports = Directory.GetFiles(rootDir, "*.md", SearchOption.TopDirectoryOnly);
        foreach (var report in reports)
        {
            var contents = File.ReadAllLines(report).ToList();

            if (contents[0] == "```")
            {
                var hwInfo = new List<string>();

                // remove hw info
                hwInfo.Add(contents[0]);
                contents.RemoveAt(0);
                while (contents[0] != "```")
                {
                    hwInfo.Add(contents[0]);
                    contents.RemoveAt(0);
                }

                hwInfo.Add(contents[0]);
                contents.RemoveAt(0);

                File.WriteAllText(".benchmark_results/hwinfo", string.Join("\n", hwInfo));
            }

            File.WriteAllText(report, $"# {name}\n\n{string.Join("\n", contents)}");
        }
    }
}

return 0;
