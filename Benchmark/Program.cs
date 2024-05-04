// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Benchmark;
using Benchmark._Context;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

PreloadAssemblies();

// configure runner
IConfig configuration = DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .WithOption(ConfigOptions.JoinSummary, true)
    .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest))
    ;

var contextTypes = GetNestedTypes(typeof(BenchmarkContextBase), static t => t is { IsAbstract: false, IsGenericType: false });
var baseBenchmarkTypes = GetNestedTypes(typeof(BenchmarkBase), static t => t is { IsAbstract: false, IsGenericType: true });

// run benchmarks
foreach (var baseBenchmarkType in baseBenchmarkTypes)
{
    var benchmarkTypes = contextTypes.Select(contextType => baseBenchmarkType.MakeGenericType(contextType)).ToArray();
    var benchmarkSwitcher = BenchmarkSwitcher.FromTypes(benchmarkTypes.ToArray());
    if (args.Length > 0) benchmarkSwitcher.Run(args, configuration);
    else benchmarkSwitcher.RunAll(configuration);
}

return 0;

void PreloadAssemblies()
{
    var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
    var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
    var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
    var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
    toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
}

Type[] GetNestedTypes(Type baseType, Predicate<Type> filter)
{
    return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
        .Where(t => t.IsSubclassOf(baseType) && filter(t)).ToArray();
}