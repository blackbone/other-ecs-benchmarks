#!/bin/sh

# pre-clean
rm -rf ./.benchmark_results
dotnet clean Benchmark/Benchmark.csproj -c Release
dotnet build Benchmark/Benchmark.csproj -c Release /p:CheckCacheMisses=false
if (($? > 0))
then
    exit $?;
fi

dotnet run --project Benchmark/Benchmark.csproj -c Release --no-build

# post-clean
rm -rf ./.benchmark_results
dotnet clean Benchmark/Benchmark.csproj -c Release