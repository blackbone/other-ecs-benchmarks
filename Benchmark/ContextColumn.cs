using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Benchmark;

public class ContextColumn : IColumn
{
    public string Id => "Context";
    public string ColumnName => "Context";
    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Job;
    public int PriorityInCategory => -1;
    public bool IsNumeric => false;
    public UnitType UnitType => UnitType.Dimensionless;
    public string Legend => "Ecs context for given benchmark";

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        return benchmarkCase.Descriptor.Type.GetGenericArguments()[0].Name.Replace("Context", "");
    }

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
    {
        return benchmarkCase.Descriptor.Type.GetProperty("Context")?.PropertyType.Name.Replace("Context", "") ?? "N/A";
    }

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase)
    {
        return true;
    }

    public bool IsAvailable(Summary summary)
    {
        return true;
    }
}

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
                break;

            var first = hashes[0];
            return hashes.Any(h => h != first) ? $"!{first}" : $" {first}";
        }

        return "?";
    }

    private static string[] GetHashesFromOutput(BenchmarkReport report, string prefix)
    {
        return (
            from executeResults in report.ExecuteResults
            from extraOutputLine in executeResults.StandardOutput.Where(line => line.StartsWith(prefix))
            select extraOutputLine[prefix.Length..]).ToArray();
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