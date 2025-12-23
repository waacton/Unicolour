using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Wacton.Unicolour;

namespace Benchmark;

[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net80)]
public class Power
{
    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Arguments))]
    public double MathPow(double number, double exponent)
    {
        Utils.UseOldPower = true;
        return Utils.Power(number, exponent);
    }
    
    [Benchmark]
    [ArgumentsSource(nameof(Arguments))]
    public double CustomPow(double number, int exponent)
    {
        Utils.UseOldPower = false;
        return Utils.Power(number, exponent);
    }
    
    public IEnumerable<object[]> Arguments =>
    [
        [123.456, 0], [123.456, 1], [123.456, 2], [123.456, 10], [123.456, 15], [123.456, 25], [123.456, 100],
        // [123.456, -1], [123.456, -2], [123.456, -10], [123.456, -15]
    ];
}