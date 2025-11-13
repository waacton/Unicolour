using Microsoft.AspNetCore.Components;

namespace Wacton.Unicolour.Example.Web.Layout;

public partial class MainLayout : LayoutComponentBase
{
    // arguably this could all be moved to State...
    private bool conversionError;
    private string cssInsideGamut = null!;
    private string cssOutsideGamut = null!;
    private bool useLightText = true;
    private string rgbText = null!;
    private string warningEmoji = null!;
    private string warningText = null!;
    
    private string TextOnColourCssClass => useLightText ? "light-text-with-contrast" : "dark-text-with-contrast";
    
    internal static readonly Unicolour Dark = new("404046");
    internal static readonly Unicolour Light = new("e8e8ff");
    
    protected override void OnInitialized()
    {
        UpdateMainDisplay();

        State.OnColourChange += () =>
        {
            UpdateMainDisplay();
            StateHasChanged();
        };

        State.OnBusyChange += StateHasChanged;
    }

    private void UpdateMainDisplay()
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
}