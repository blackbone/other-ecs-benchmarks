#!/bin/zsh

# pre-clean
rm -rf ./.benchmark_results
dotnet clean Benchmark/Benchmark.csproj -c Release
dotnet build Benchmark/Benchmark.csproj -c Release /p:CheckCacheMisses=true
if (($? > 0))
then
    exit $?;
fi

dotnet run --project Benchmark/Benchmark.csproj -c Release --no-build
cat ./.benchmark_results/**/*.md > ./results.md

# post-clean
rm -rf ./.benchmark_results
dotnet clean Benchmark/Benchmark.csproj -c Release