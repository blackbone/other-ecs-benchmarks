#!/bin/sh

# pre-clean
rm -rf ./.benchmark_results
dotnet clean

# restore and build
dotnet restore -c Debug
dotnet build -c Debug /p:CheckCacheMisses=false /p:ShortRun=true
if (($? > 0))
then
    exit $?;
fi

#dotnet test -c Debug --no-build
#if (($? > 0))
#then
#    exit $?;
#fi

dotnet run --project Benchmark/Benchmark.csproj -c Debug --no-build /p:CheckCacheMisses=false /p:ShortRun=true

rm report.md

echo "HW Info:\n" >> report.md
cat .benchmark_results/hwinfo >> report.md
echo "\n" >> report.md
find .benchmark_results -name '*.md' -print0 | while IFS= read -r -d '' file; do
  cat "$file" >> report.md
done

# post-clean
rm -rf ./.benchmark_results
dotnet clean -c Debug