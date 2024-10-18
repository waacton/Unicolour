using Benchmark;
using BenchmarkDotNet.Running;

// dotnet run -c Release --project Benchmark\Benchmark.csproj --framework net472 net8.0
var summary = BenchmarkRunner.Run<ConvertFromRgb>();