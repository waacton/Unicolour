namespace Wacton.Unicolour.Example.Web;

internal class SliderSolidColour
{
    private readonly Unicolour colour;
    
    internal double Value { get; set; }
    internal string ValueText => $"{Value:F2}";
    internal string AxisText { get; }
    private Range Range => new(0, 1);

    internal double Min => Range.Min;
    internal double Max => Range.Max;
    internal bool InRange => Value >= Min && Value <= Max;
    internal double Step => 0.01;

    internal readonly string LabelText;
    internal string CssBackground => colour.Hex;
    internal bool UseLightLabelText => colour.Contrast(App.Light) > colour.Contrast(App.Dark);
    
    internal SliderSolidColour(Unicolour colour, string labelText, string axisText)
    {
        this.colour = colour.MapToRgbGamut(GamutMap.RgbClipping);
        LabelText = labelText;
        AxisText = axisText;
    }
}