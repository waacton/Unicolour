namespace Wacton.Unicolour.Example.Web;

internal class SliderGradientColour
{
    internal double Value { get; set; }
    internal string ValueText { get; set; } = null!;
    internal string AxisText { get; set; } = null!;
    internal Range Range { get; set; } = null!;
    internal double Step { get; set; }
    
    internal double Min => Range.Min;
    internal double Max => Range.Max;
    internal bool InRange => Value >= Min && Value <= Max;
    
    internal Unicolour[] Stops { get; set; } = [];
    internal string CssGradient => string.Join(",", Stops.Select(x => Utils.ToCss(x, 100)));
    internal string CssAlphaGradient => string.Join(",", Stops.Select(x => Utils.ToCss(x, x.IsInRgbGamut ? 100 : 50)));
}