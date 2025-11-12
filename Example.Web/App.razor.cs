using Microsoft.AspNetCore.Components;

namespace Wacton.Unicolour.Example.Web;

public partial class App : ComponentBase
{
    private bool conversionError;
    private string cssInsideGamut = null!;
    private string cssOutsideGamut = null!;
    private bool useLightText = true;
    private string rgbText = null!;
    private string warningEmoji = null!;
    private string warningText = null!;
    
    private readonly Unicolour dark = new("404046");
    private readonly Unicolour light = new("e8e8ff");
    
    protected override void OnInitialized()
    {
        UpdateDisplay();

        State.OnChange += () =>
        {
            UpdateDisplay();
            StateHasChanged();
        };
    }

    private void UpdateDisplay()
    {
        var colour = State.Colour;
        
        conversionError = Utils.HasConversionError(colour);
        cssInsideGamut = Utils.ToCss(colour, 100);
        cssOutsideGamut = Utils.ToCss(colour, colour.IsInRgbGamut ? 100 : 50);

        var rgbString = conversionError ? "NaN" : colour.Rgb.ToString();
        var hexString = !colour.IsInRgbGamut ? "#------" : colour.Hex;
        rgbText = $"RGB {rgbString} · {hexString}";

        if (conversionError)
        {
            useLightText = false;
        }
        else
        {
            useLightText = colour.Contrast(light) > colour.Contrast(dark);
        }

        if (conversionError)
        {
            warningText = "Cannot be converted";
            warningEmoji = "❌";
        }
        else if (!colour.IsInRgbGamut)
        {
            var (clippedR, clippedG, clippedB) = colour.Rgb.ConstrainedTriplet;
            warningText = $"Out of gamut, clipped to\nRGB {clippedR:F2} {clippedG:F2} {clippedB:F2} · {colour.Rgb.Byte255.ConstrainedHex}";
            warningEmoji = "⚠️";
        }
        else
        {
            warningText = string.Empty;
            warningEmoji = string.Empty;
        }
    }
}