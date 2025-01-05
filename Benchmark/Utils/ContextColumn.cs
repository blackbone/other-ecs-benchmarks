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

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase) => benchmarkCase.Descriptor.Type.Name.Split('_')[1];

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) => benchmarkCase.Descriptor.Type.Name.Split('_')[1];

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => true;

    public bool IsAvailable(Summary summary) => true;
}
