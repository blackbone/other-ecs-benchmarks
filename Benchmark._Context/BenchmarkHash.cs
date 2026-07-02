using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Benchmark.Context;

public static class BenchmarkHash
{
    private const ulong Offset = 14695981039346656037UL;
    private const ulong Prime = 1099511628211UL;

    [ThreadStatic] private static ulong _value;
    [ThreadStatic] private static bool _initialized;
    private static readonly Dictionary<string, List<string>> PrintedHashes = new();

    public static bool Enabled { get; set; } = true;

    public static void Reset()
    {
        if (!Enabled)
        {
            _initialized = false;
            return;
        }

        ArrayExtensions.ResetRandom();
        _value = Offset;
        _initialized = true;
    }

    public static void AddEntityCount(int count)
    {
        if (!Enabled)
            return;

        AddText("Entities");
        AddInt32(count);
    }

    public static void AddCreateEntities(int count, int entityCount)
    {
        if (!Enabled)
            return;

        AddOperation("CreateEntities", count, entityCount);
    }

    public static void AddCreateEntities<T1>(int count, in T1 c1, int entityCount) where T1 : struct
    {
        if (!Enabled)
            return;

        AddOperation("CreateEntities", count, entityCount);
        AddComponent(in c1);
    }

    public static void AddCreateEntities<T1, T2>(int count, in T1 c1, in T2 c2, int entityCount)
        where T1 : struct where T2 : struct
    {
        if (!Enabled)
            return;

        AddOperation("CreateEntities", count, entityCount);
        AddComponent(in c1);
        AddComponent(in c2);
    }

    public static void AddCreateEntities<T1, T2, T3>(int count, in T1 c1, in T2 c2, in T3 c3, int entityCount)
        where T1 : struct where T2 : struct where T3 : struct
    {
        if (!Enabled)
            return;

        AddOperation("CreateEntities", count, entityCount);
        AddComponent(in c1);
        AddComponent(in c2);
        AddComponent(in c3);
    }

    public static void AddCreateEntities<T1, T2, T3, T4>(int count, in T1 c1, in T2 c2, in T3 c3, in T4 c4, int entityCount)
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct
    {
        if (!Enabled)
            return;

        AddOperation("CreateEntities", count, entityCount);
        AddComponent(in c1);
        AddComponent(in c2);
        AddComponent(in c3);
        AddComponent(in c4);
    }

    public static void AddComponentEntities<T1>(int count, in T1 c1, int entityCount) where T1 : struct
    {
        if (!Enabled)
            return;

        AddOperation("AddComponent", count, entityCount);
        AddComponent(in c1);
    }

    public static void AddComponentEntities<T1, T2>(int count, in T1 c1, in T2 c2, int entityCount)
        where T1 : struct where T2 : struct
    {
        if (!Enabled)
            return;

        AddOperation("AddComponent", count, entityCount);
        AddComponent(in c1);
        AddComponent(in c2);
    }

    public static void AddComponentEntities<T1, T2, T3>(int count, in T1 c1, in T2 c2, in T3 c3, int entityCount)
        where T1 : struct where T2 : struct where T3 : struct
    {
        if (!Enabled)
            return;

        AddOperation("AddComponent", count, entityCount);
        AddComponent(in c1);
        AddComponent(in c2);
        AddComponent(in c3);
    }

    public static void AddComponentEntities<T1, T2, T3, T4>(int count, in T1 c1, in T2 c2, in T3 c3, in T4 c4, int entityCount)
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct
    {
        if (!Enabled)
            return;

        AddOperation("AddComponent", count, entityCount);
        AddComponent(in c1);
        AddComponent(in c2);
        AddComponent(in c3);
        AddComponent(in c4);
    }

    public static void AddRemoveComponent<T1>(int count, int entityCount) where T1 : struct
    {
        if (!Enabled)
            return;

        AddOperation("RemoveComponent", count);
        AddType<T1>();
    }

    public static void AddRemoveComponent<T1, T2>(int count, int entityCount)
        where T1 : struct where T2 : struct
    {
        if (!Enabled)
            return;

        AddOperation("RemoveComponent", count);
        AddType<T1>();
        AddType<T2>();
    }

    public static void AddRemoveComponent<T1, T2, T3>(int count, int entityCount)
        where T1 : struct where T2 : struct where T3 : struct
    {
        if (!Enabled)
            return;

        AddOperation("RemoveComponent", count);
        AddType<T1>();
        AddType<T2>();
        AddType<T3>();
    }

    public static void AddRemoveComponent<T1, T2, T3, T4>(int count, int entityCount)
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct
    {
        if (!Enabled)
            return;

        AddOperation("RemoveComponent", count);
        AddType<T1>();
        AddType<T2>();
        AddType<T3>();
        AddType<T4>();
    }

    public static void AddDeleteEntities(int count, int entityCount)
    {
        if (!Enabled)
            return;

        AddOperation("DeleteEntities", count, entityCount);
    }

    public static void AddTick(float delta, int entityCount)
    {
        if (!Enabled)
            return;

        AddText("Tick");
        AddInt32(BitConverter.SingleToInt32Bits(delta));
        AddInt32(entityCount);
    }

    public static string PrintAndReset()
    {
        return PrintAndReset(null);
    }

    public static string PrintAndReset(string benchmarkType)
    {
        if (!Enabled)
            return string.Empty;

        EnsureInitialized();
        var hash = _value.ToString("X16", CultureInfo.InvariantCulture);
        Console.WriteLine("// Hash: " + hash);
        if (benchmarkType != null)
        {
            lock (PrintedHashes)
            {
                if (!PrintedHashes.TryGetValue(benchmarkType, out var hashes))
                {
                    hashes = [];
                    PrintedHashes.Add(benchmarkType, hashes);
                }

                hashes.Add(hash);
            }

            Directory.CreateDirectory(".benchmark_results/hash");
            File.AppendAllLines(GetHashFileCandidates(benchmarkType)[0], [hash]);
        }

        Reset();
        return hash;
    }

    public static string[] GetPrintedHashes(string benchmarkType)
    {
        lock (PrintedHashes)
        {
            if (PrintedHashes.TryGetValue(benchmarkType, out var hashes))
                return hashes.ToArray();

            var shortName = benchmarkType[(benchmarkType.LastIndexOf('.') + 1)..];
            var matching = PrintedHashes
                .Where(pair => pair.Key == shortName || pair.Key.EndsWith("." + shortName, StringComparison.Ordinal))
                .SelectMany(pair => pair.Value)
                .ToArray();
            return matching;
        }
    }

    public static void ClearPrintedHashes()
    {
        lock (PrintedHashes)
        {
            PrintedHashes.Clear();
        }
    }

    public static string[] GetHashFileCandidates(string benchmarkType)
    {
        var shortName = benchmarkType[(benchmarkType.LastIndexOf('.') + 1)..];
        return [
            ".benchmark_results/hash/" + SanitizeFileName(benchmarkType) + ".txt",
            ".benchmark_results/hash/" + SanitizeFileName(shortName) + ".txt"
        ];
    }

    private static string SanitizeFileName(string value)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            value = value.Replace(c, '_');

        return value.Replace('.', '_');
    }

    private static void AddOperation(string name, int count, int entityCount)
    {
        AddText(name);
        AddInt32(count);
        AddInt32(entityCount);
    }

    private static void AddOperation(string name, int count)
    {
        AddText(name);
        AddInt32(count);
    }

    private static void AddComponent<T>(in T component) where T : struct
    {
        AddType<T>();

        foreach (var field in ComponentFields<T>.Fields)
        {
            AddText(field.Name);
            AddObject(field.GetValue(component));
        }
    }

    private static void AddType<T>()
    {
        AddText(typeof(T).FullName ?? typeof(T).Name);
    }

    private static void AddObject(object value)
    {
        switch (value)
        {
            case null:
                AddByte(0);
                break;
            case int v:
                AddInt32(v);
                break;
            case long v:
                AddInt64(v);
                break;
            case float v:
                AddInt32(BitConverter.SingleToInt32Bits(v));
                break;
            case double v:
                AddInt64(BitConverter.DoubleToInt64Bits(v));
                break;
            case bool v:
                AddByte(v ? (byte)1 : (byte)0);
                break;
            default:
                AddText(Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty);
                break;
        }
    }

    private static void AddText(string value)
    {
        AddInt32(value.Length);
        foreach (var c in value)
            AddInt32(c);
    }

    private static void AddInt64(long value)
    {
        unchecked
        {
            AddInt32((int)value);
            AddInt32((int)(value >> 32));
        }
    }

    private static void AddInt32(int value)
    {
        unchecked
        {
            AddByte((byte)value);
            AddByte((byte)(value >> 8));
            AddByte((byte)(value >> 16));
            AddByte((byte)(value >> 24));
        }
    }

    private static void AddByte(byte value)
    {
        EnsureInitialized();
        _value ^= value;
        _value *= Prime;
    }

    private static void EnsureInitialized()
    {
        if (!_initialized)
            Reset();
    }

    private static class ComponentFields<T>
    {
        public static readonly FieldInfo[] Fields = typeof(T)
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .OrderBy(field => field.Name, StringComparer.Ordinal)
            .ToArray();
    }
}
