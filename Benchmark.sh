# pre-clean
rm -rf ./.benchmark_results
dotnet clean

# restore and build
dotnet restore -c Release
dotnet build -c Release /p:CheckCacheMisses=false /p:ShortRun=true
if (($? > 0))
then
    exit $?;
fi

dotnet test -c Release --no-build
if (($? > 0))
then
    exit $?;
fi

dotnet run --project Benchmark/Benchmark.csproj -c Release --no-build /p:CheckCacheMisses=false /p:ShortRun=true

rm report.md

# Prepare the report file
echo "# LOCAL RUN BENCHMARKS:\n---" >> report.md
echo "HW Info:\n" >> report.md
cat .benchmark_results/hwinfo >> report.md
echo "\n" >> report.md

# Find and sort the markdown files by their first line
find .benchmark_results -name '*.md' -print0 | while IFS= read -r -d '' file; do
  first_line=$(head -n 1 "$file")
  echo "$first_line|$file"
done | sort | while IFS='|' read -r first_line file; do
  cat "$file" >> report.md
  echo -e "\n" >> report.md
done

# post-clean
rm -rf ./.benchmark_results
dotnet clean -c Release