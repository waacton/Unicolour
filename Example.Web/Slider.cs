using Wacton.Unicolour.Example.Web.Layout;

namespace Wacton.Unicolour.Example.Web;

public abstract class Slider
{
    internal double Value { get; set; }
    internal string ValueText { get; set; } = null!;
    internal string LabelText { get; set; } = null!;
    internal Range Range { get; set; } = null!;
    internal double Step { get; set; }
    
    internal double Min => Range.Min;
    internal double Max => Range.Max;
    internal bool InRange => Value >= Min && Value <= Max;
}

internal class GradientColourSlider : Slider
{
    internal Unicolour[] Stops { get; set; } = [];
    internal string CssGradient => string.Join(",", Stops.Select(x => Utils.ToCss(x, 100)));
    internal string CssAlphaGradient => string.Join(",", Stops.Select(x => Utils.ToCss(x, x.IsInRgbGamut ? 100 : 50)));
}

internal class SolidColourSlider : Slider
{
    internal Unicolour Colour { get; set; } = null!;
    internal string CssBackground => Colour.Hex;
    internal string LabelTextCssClass => Colour.Contrast(MainLayout.Light) > Colour.Contrast(MainLayout.Dark) ? "light-text-with-contrast" : "dark-text-with-contrast";
}