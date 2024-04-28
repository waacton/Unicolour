namespace Wacton.Unicolour.Example.Web;

public class Slider
{
    private readonly int index;
    internal double Value { get; set; }
    internal string ValueText => GetValueText();
    internal string AxisText => ColourLookup.AxisLookup[ColourSpace][index];

    internal ColourSpace ColourSpace { get; set; }
    private Range Range => ColourLookup.RangeLookup[ColourSpace][index];
    internal double Min => Range.Min;
    internal double Max => Range.Max;
    internal bool InRange => Value >= Min && Value <= Max;
    internal double Step => GetStep();
    internal List<Unicolour> Stops { get; set; } = [];

    internal string CssGradient => string.Join(",", Stops.Select(x => Utils.ToCss(x, 100)));
    internal string CssAlphaGradient => string.Join(",", Stops.Select(x => Utils.ToCss(x, x.IsInDisplayGamut ? 100 : 50)));
    
    internal Slider(int index, double value)
    {
        this.index = index;
        Value = value;
    }
    
    private string GetValueText()
    {
        return Range.Distance switch
        {
            < 0.5 => $"{Value:F3}",
            < 5 => $"{Value:F2}",
            < 50 => $"{Value:F1}",
            _ => $"{Value:F0}"
        };
    }
    
    private double GetStep()
    {
        return Range.Distance switch
        {
            < 0.5 => 0.001,
            < 5 => 0.01,
            < 50 => 0.1,
            _ => 1
        };
    }
}