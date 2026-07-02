using System.Collections.Generic;
using System.IO;
using System.Linq;
using Benchmark.Context;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Benchmark.Utils;

public class HashColumn : IColumn
{
    public static readonly IColumn Default = new HashColumn();
    private HashColumn() {}

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        foreach (var report in summary.Reports)
        {
            if (report.BenchmarkCase != benchmarkCase)
                continue;

            var hashes = GetHashesFromOutput(report, "// Hash: ");
            if (hashes.Length == 0)
                hashes = BenchmarkHash.GetPrintedHashes(benchmarkCase.Descriptor.Type.FullName);
            if (hashes.Length == 0)
                hashes = GetHashesFromFile(benchmarkCase.Descriptor.Type.FullName);
            if (hashes.Length == 0)
                hashes = GetHashesFromLog(summary, benchmarkCase, "// Hash: ");
            if (hashes.Length == 0)
                break;

            var first = hashes[0];
            return hashes.Any<string>(h => h != first) ? $"!{first}" : $" {first}";
        }

        return "?";
    }

    private static string[] GetHashesFromOutput(BenchmarkReport report, string prefix)
    {
        return (
            from executeResults in report.ExecuteResults
            from extraOutputLine in (executeResults.StandardOutput ?? []).Where<string>(line => line.StartsWith(prefix))
            select extraOutputLine[prefix.Length..]).ToArray<string>();
    }

    private static string[] GetHashesFromLog(Summary summary, BenchmarkCase benchmarkCase, string prefix)
    {
        if (summary.LogFilePath == null || !File.Exists(summary.LogFilePath))
            return [];

        var marker = "// Benchmark: " + benchmarkCase.Descriptor.Type.Name + "." + benchmarkCase.Descriptor.WorkloadMethod.Name + ":";
        var result = new List<string>();
        var active = false;

        foreach (var line in File.ReadLines(summary.LogFilePath))
        {
            if (line.StartsWith("// Benchmark: "))
                active = line.StartsWith(marker);
            else if (active && line.StartsWith("// ** Remained"))
                active = false;

            if (active && line.StartsWith(prefix))
                result.Add(line[prefix.Length..]);
        }

        return result.ToArray();
    }

    private static string[] GetHashesFromFile(string benchmarkType)
    {
        return BenchmarkHash.GetHashFileCandidates(benchmarkType)
            .Where(File.Exists)
            .SelectMany(File.ReadAllLines)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();
    }

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) => GetValue(summary, benchmarkCase);
    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
    public bool IsAvailable(Summary summary) => true;
    public string Id => nameof(HashColumn);
    public string ColumnName => "Hash";
    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Custom;
    public int PriorityInCategory => 0;
    public bool IsNumeric => false;
    public UnitType UnitType => UnitType.Dimensionless;
    public string Legend => "Hash";
    public override string ToString() => ColumnName;
}
