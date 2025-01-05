using System.Linq;
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
            from extraOutputLine in executeResults.StandardOutput.Where<string>(line => line.StartsWith(prefix))
            select extraOutputLine[prefix.Length..]).ToArray<string>();
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