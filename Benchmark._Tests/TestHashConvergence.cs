using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Benchmark.Context;
using BenchmarkDotNet.Attributes;
using Helper = Bentchmark.Tests.Helper;

namespace Benchmark.Tests;

[NonParallelizable]
public class TestHashConvergence {
    private const int Runs = 3;

    [Test]
    public void HashCanBeDisabledGlobally() {
        var originalEnabled = BenchmarkHash.Enabled;
        var originalOut = Console.Out;
        var writer = new StringWriter();

        try {
            BenchmarkHash.Enabled = false;
            BenchmarkHash.ClearPrintedHashes();
            Console.SetOut(writer);

            BenchmarkHash.Reset();
            BenchmarkHash.AddEntityCount(1);
            var hash = BenchmarkHash.PrintAndReset("Benchmark.Disabled");

            Assert.That(hash, Is.Empty);
            Assert.That(writer.ToString(), Is.Empty);
            Assert.That(BenchmarkHash.GetPrintedHashes("Benchmark.Disabled"), Is.Empty);
        }
        finally {
            Console.SetOut(originalOut);
            BenchmarkHash.Enabled = originalEnabled;
            BenchmarkHash.ClearPrintedHashes();
        }
    }

    [Test]
    public void HashesConvergeForAllBenchmarks() {
        var originalOut = Console.Out;
        var originalError = Console.Error;

        try {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);

            var results = GetHashResults().ToArray();
            var mismatches = results
                .GroupBy(result => result.GroupKey)
                .Where(group => group.Select(result => result.Hash).Distinct(StringComparer.Ordinal).Count() > 1)
                .ToArray();

            if (mismatches.Length > 0) {
                Assert.Fail("Hash convergence failed:\n" + string.Join("\n\n", mismatches.Select(FormatMismatch)));
            }
        }
        finally {
            Console.SetOut(originalOut);
            Console.SetError(originalError);
            BenchmarkHash.ClearPrintedHashes();
        }
    }

    private static IEnumerable<HashResult> GetHashResults() {
        foreach (var pair in BenchMap.Runs) {
            var benchmarkName = pair.Key.Name;

            foreach (var benchmarkType in pair.Value) {
                if (!benchmarkType.IsAssignableTo(typeof(IBenchmark))) {
                    continue;
                }

                foreach (var injection in Helper.GetBenchmarkInjections(benchmarkType)) {
                    yield return RunBenchmark(benchmarkName, benchmarkType, injection);
                }
            }
        }
    }

    private static HashResult RunBenchmark(string benchmarkName, Type benchmarkType, Helper.BenchmarkInjection injection) {
        var hashes = new List<string>();
        for (var i = 0; i < Runs; i++) {
            hashes.Add(RunBenchmarkOnce(benchmarkType, injection));
        }

        Assert.That(
            hashes.Distinct(StringComparer.Ordinal).Count(),
            Is.EqualTo(1),
            benchmarkType.FullName + " hash must be stable between runs: " + string.Join(", ", hashes));

        return new HashResult(
            benchmarkName + "|" + injection.Key,
            benchmarkName,
            GetContextName(benchmarkName, benchmarkType),
            injection.Key,
            hashes[0]);
    }

    private static string RunBenchmarkOnce(Type benchmarkType, Helper.BenchmarkInjection injection) {
        BenchmarkHash.ClearPrintedHashes();
        ArrayExtensions.ResetRandom();

        var benchmark = (IBenchmark)Activator.CreateInstance(benchmarkType);
        injection.Apply(benchmark);

        var globalSetups = GetMethodsWithAttribute<GlobalSetupAttribute>(benchmarkType);
        var iterationSetups = GetMethodsWithAttribute<IterationSetupAttribute>(benchmarkType);
        var benchmarkMethods = GetMethodsWithAttribute<BenchmarkAttribute>(benchmarkType);
        var iterationCleanups = GetMethodsWithAttribute<IterationCleanupAttribute>(benchmarkType);
        var globalCleanups = GetMethodsWithAttribute<GlobalCleanupAttribute>(benchmarkType);

        Assert.That(benchmarkMethods.Length, Is.EqualTo(1), benchmarkType.FullName + " must have exactly one [Benchmark] method.");

        InvokeAll(benchmark, globalSetups);
        try {
            InvokeAll(benchmark, iterationSetups);
            Invoke(benchmark, benchmarkMethods[0]);
            InvokeAll(benchmark, iterationCleanups);

            var hashes = BenchmarkHash.GetPrintedHashes(benchmarkType.FullName);
            Assert.That(hashes.Length, Is.EqualTo(1), benchmarkType.FullName + " must print one hash per run.");
            return hashes[0];
        }
        finally {
            InvokeAll(benchmark, globalCleanups);
            BenchmarkHash.ClearPrintedHashes();
        }
    }

    private static MethodInfo[] GetMethodsWithAttribute<TAttribute>(Type type) where TAttribute : Attribute {
        return type
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(method => method.GetCustomAttribute<TAttribute>() != null)
            .OrderBy(method => method.MetadataToken)
            .ToArray();
    }

    private static string GetContextName(string benchmarkName, Type benchmarkType) {
        var prefix = benchmarkName + "_";
        return benchmarkType.Name.StartsWith(prefix, StringComparison.Ordinal)
            ? benchmarkType.Name[prefix.Length..]
            : benchmarkType.Name;
    }

    private static void InvokeAll(object target, IEnumerable<MethodInfo> methods) {
        foreach (var method in methods) {
            Invoke(target, method);
        }
    }

    private static void Invoke(object target, MethodInfo method) {
        try {
            method.Invoke(target, null);
        }
        catch (TargetInvocationException ex) when (ex.InnerException != null) {
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
        }
    }

    private static string FormatMismatch(IGrouping<string, HashResult> group) {
        return group.First().BenchmarkName + " [" + group.First().InjectionKey + "]\n" +
               string.Join("\n", group.Select(result => result.ContextName + ": " + result.Hash).OrderBy(value => value, StringComparer.Ordinal));
    }

    private sealed class HashResult {
        public HashResult(string groupKey, string benchmarkName, string contextName, string injectionKey, string hash) {
            GroupKey = groupKey;
            BenchmarkName = benchmarkName;
            ContextName = contextName;
            InjectionKey = injectionKey;
            Hash = hash;
        }

        public string GroupKey { get; }

        public string BenchmarkName { get; }

        public string ContextName { get; }

        public string InjectionKey { get; }

        public string Hash { get; }
    }
}
