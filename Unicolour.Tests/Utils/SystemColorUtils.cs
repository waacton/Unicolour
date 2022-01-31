namespace Wacton.Unicolour.Tests.Utils;

using System.Drawing;

internal static class SystemColorUtils
{
    public static (int r255, int g255, int b255, int a255) HexToRgb255(string hex)
    {
        var htmlHex = hex.StartsWith('#') ? hex : $"#{hex}";
        var systemColor = ColorTranslator.FromHtml(htmlHex);
        
        // unicolour is RGBA but system color is ARGB
        // hence if first byte is FF, it will be recorded as either R or A
        var hasAlpha = htmlHex.Length == 9;
        return hasAlpha
            ? (systemColor.A, systemColor.R, systemColor.G, systemColor.B)
            : (systemColor.R, systemColor.G, systemColor.B, 255);
    }
}