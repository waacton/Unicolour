namespace Wacton.Unicolour.Tests.Utils;

using System.Collections.Generic;

internal static class StandardRgb
{
    internal static Unicolour Red = new(ColourSpace.Rgb, 1, 0, 0);
    internal static Unicolour Green = new(ColourSpace.Rgb, 0, 1, 0);
    internal static Unicolour Blue = new(ColourSpace.Rgb, 0, 0, 1);
    internal static Unicolour Cyan = new(ColourSpace.Rgb, 0, 1, 1);
    internal static Unicolour Magenta = new(ColourSpace.Rgb, 1, 0, 1);
    internal static Unicolour Yellow = new(ColourSpace.Rgb, 1, 1, 0);
    internal static Unicolour Black = new(ColourSpace.Rgb, 0, 0, 0);
    internal static Unicolour White = new(ColourSpace.Rgb, 1, 1, 1);
    internal static Unicolour Grey = new(ColourSpace.Rgb, 0.5, 0.5, 0.5);

    internal static readonly Dictionary<string, Unicolour> Lookup = new()
    {
        { nameof(Red), Red },
        { nameof(Green), Green },
        { nameof(Blue), Blue },
        { nameof(Cyan), Cyan },
        { nameof(Magenta), Magenta },
        { nameof(Yellow), Yellow },
        { nameof(Black), Black },
        { nameof(White), White },
        { nameof(Grey), Grey }
    };
}