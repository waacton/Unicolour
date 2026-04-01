using System.Collections.Generic;

namespace Wacton.Unicolour.Tests.Utils;

internal static class StandardRgb
{
    internal static readonly Unicolour Red = new(ColourSpace.Rgb, 1, 0, 0);
    internal static readonly Unicolour Green = new(ColourSpace.Rgb, 0, 1, 0);
    internal static readonly Unicolour Blue = new(ColourSpace.Rgb, 0, 0, 1);
    internal static readonly Unicolour Cyan = new(ColourSpace.Rgb, 0, 1, 1);
    internal static readonly Unicolour Magenta = new(ColourSpace.Rgb, 1, 0, 1);
    internal static readonly Unicolour Yellow = new(ColourSpace.Rgb, 1, 1, 0);
    internal static readonly Unicolour Black = new(ColourSpace.Rgb, 0, 0, 0);
    internal static readonly Unicolour White = new(ColourSpace.Rgb, 1, 1, 1);
    internal static readonly Unicolour Grey = new(ColourSpace.Rgb, 0.5, 0.5, 0.5);

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