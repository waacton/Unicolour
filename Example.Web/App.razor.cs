using Microsoft.AspNetCore.Components;

namespace Wacton.Unicolour.Example.Web;

public partial class App : ComponentBase
{
    private Mode mode = Mode.Light;
    
    private bool conversionError;
    private string cssInsideGamut = null!;
    private string cssOutsideGamut = null!;
    private bool useLightText = true;
    private string rgbText = null!;
    private string warningEmoji = null!;
    private string warningText = null!;
    
    internal static readonly Unicolour Dark = new("404046");
    internal static readonly Unicolour Light = new("e8e8ff");
    
    protected override void OnInitialized()
    {
        UpdateDisplay();

        State.OnColourChange += () =>
        {
            UpdateDisplay();
            StateHasChanged();
        };
        
        State.OnBusyChange += StateHasChanged;
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
            useLightText = true;
        }
        else
        {
            useLightText = colour.Contrast(Light) > colour.Contrast(Dark);
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

    private void SetMode(Mode newMode)
    {
        mode = newMode;
    }

    private enum Mode
    {
        Light,
        Print,
        Paint
    }
}