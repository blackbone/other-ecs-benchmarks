#!/bin/sh
$SHORT_RUN=false 

# pre-clean
rm -rf ./.benchmark_results
dotnet clean -c Release

# restore and build
dotnet restore -c Release
dotnet build -c Release /p:CheckCacheMisses=false /p:ShortRun=$SHORT_RUN
if (($? > 0))
then
    exit $?;
fi

#dotnet test -c Release --no-build
#if (($? > 0))
#then
#    exit $?;
#fi

dotnet run --project Benchmark/Benchmark.csproj -c Release --no-build benchmarks=System

# post-clean
rm -rf ./.benchmark_results
dotnet clean -c Release