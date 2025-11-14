namespace Wacton.Unicolour.Example.Web;

internal record Range(double Min, double Max)
{
    private readonly double Distance = Math.Abs(Max - Min);

    internal double DefaultStep => Distance switch
    {
        < 0.5 => 0.001,
        < 5 => 0.01,
        < 50 => 0.1,
        _ => 1
    };

    internal string DefaultValueString(double value) => Distance switch
    {
        < 0.5 => $"{value:F3}",
        < 5 => $"{value:F2}",
        < 50 => $"{value:F1}",
        _ => $"{value:F0}"
    };
}