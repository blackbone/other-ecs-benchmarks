using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Benchmark;
using BenchmarkDotNet.Attributes;

namespace Bentchmark.Tests;

public static class Helper {
    public static IEnumerable<Action<IBenchmark>> GetInjections(Type benchmarkType) {
        var paramProps = benchmarkType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.Name != nameof(IBenchmark.EntityCount))
            .Select(p => (p, attr: p.GetCustomAttribute<ParamsAttribute>()))
            .Where(t => t.attr != null)
            .ToArray();

        var allValues = paramProps
            .Select(t => t.attr.Values)
            .ToArray();

        foreach (var combination in CartesianProduct(allValues)) {
            yield return benchmark => {
                benchmark.EntityCount = Constants.SmallEntityCount;
                for (int i = 0; i < paramProps.Length; i++) {
                    paramProps[i].p.SetValue(benchmark, combination[i]);
                }
            };
        }
    }
    
    private static IEnumerable<object[]> CartesianProduct(object[][] sequences) {
        int[] indices = new int[sequences.Length];
        while (true) {
            var result = new object[sequences.Length];
            for (int i = 0; i < sequences.Length; i++) {
                result[i] = sequences[i][indices[i]];
            }
            yield return result;

            int k = sequences.Length - 1;
            while (k >= 0 && ++indices[k] >= sequences[k].Length) {
                indices[k] = 0;
                k--;
            }
            if (k < 0) yield break;
        }
    }
}
