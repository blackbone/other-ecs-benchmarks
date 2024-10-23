using System.Diagnostics;
using Benchmark;
using Benchmark.Generated;

Console.WriteLine("use this for testing and debugging");

IBenchmark b = new SystemWith1Component_FlecsNETContext();
b.EntityCount = 100_000;
var sw = Stopwatch.StartNew();

b.GlobalSetup();
while (true) {// sw.ElapsedMilliseconds < 60_000) {
    b.IterationSetup();
    b.Run();
    b.IterationCleanup();
}
b.GlobalCleanup();
Console.WriteLine("finished successfully");

// TODO:
// 0. элиминировать оверхед при миграции архетипов, посмотреть профайлинг
// 1. фиксировать спаны айдишек энтитей до начала итерации
// 2. обработать кейс кода е1 меняет архетип е2 (юзер код) фикс поможет на добавление, но не на удаление, для удаления надо декрементнуть каунт в спане
// 3. п2 + что-то свапбакает е2 на место е3 которая будет учавствовать в итерации (юзер код)