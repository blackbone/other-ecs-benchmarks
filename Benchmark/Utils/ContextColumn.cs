using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Benchmark.Utils;

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