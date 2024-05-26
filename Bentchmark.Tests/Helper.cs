using System.Reflection;
using Benchmark;
using Benchmark._Context;
using BenchmarkDotNet.Attributes;

namespace Bentchmark.Tests;

public static class Helper
{
    private static bool _preloaded;
    
    public static IEnumerable<Type> GetContextTypes()
    {
        PreloadAssemblies();
        return GetNestedTypes(typeof(BenchmarkContextBase), static t => t is { IsAbstract: false, IsGenericType: false });
    }

    public static IEnumerable<Type> GetBenchmarkTypes()
    {
        PreloadAssemblies();
        return GetNestedTypes(typeof(BenchmarkBase), static t => t is { IsAbstract: false, IsGenericType: true });
    }
    
    public static void InjectParameters(BenchmarkBase benchmark)
    {
        benchmark.EntityCount = Constants.LargeEntityCount;

        var properties = benchmark.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(p => (p, p.GetCustomAttribute<ParamsAttribute>()))
            .Where(kv => kv.Item2 != null)
            .ToArray();
        foreach (var (property, attribute) in properties)
            property.SetValue(benchmark, attribute!.Values[0]);
    }
    
    private static void PreloadAssemblies()
    {
        if (_preloaded) return;
        
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
        
        _preloaded = true;
    }

    private static Type[] GetNestedTypes(Type baseType, Predicate<Type> filter)
    {
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(baseType) && filter(t)).ToArray();
    }
}