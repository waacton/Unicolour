using Benchmark;
using BenchmarkDotNet.Running;

/*
 * dotnet run -c Release --project Benchmark\Benchmark.csproj --framework net10.0 --runtimes net10.0 net8.0 net472
 */

// serialises a seed to be deserialised and used by the randomiser in each benchmark
// so that each benchmark session uses random input, but each benchmark within a session uses the same input 
// (BenchmarkDotNet starts a new process per benchmark, so cannot store static state)
// can of course be hardcoded to compare against previous benchmark sessions
var seed = new Randomiser().Seed;
Serialisation.WriteSeed(seed);

_ = BenchmarkRunner.Run<Construct>();
_ = BenchmarkRunner.Run<ConvertColourSpace>();
_ = BenchmarkRunner.Run<ConvertToIcc>();
_ = BenchmarkRunner.Run<ConvertFromIcc>();
