namespace Wacton.Unicolour;

public enum HueSpan
{
    Shorter,
    Longer,
    Increasing,
    Decreasing
}

public static class Hue
{
    internal static (double start, double end) Unwrap(double start, double end, HueSpan hueSpan = HueSpan.Shorter)
    {
        return hueSpan switch
        {
            HueSpan.Shorter => (end - start) switch
            {
                > 180 => (start + 360, end),
                < -180 => (start, end + 360),
                _ => (start, end)
            },
            HueSpan.Longer => (end - start) switch
            {
                > 0 and < 180 => (start + 360, end),
                > -180 and <= 0 => (start, end + 360),
                _ => (start, end)
            },
            HueSpan.Increasing => (start, end < start ? end + 360 : end),
            HueSpan.Decreasing => (start < end ? start + 360 : start, end),
            _ => throw new ArgumentOutOfRangeException(nameof(hueSpan), hueSpan, null)
        };
    }
    
        
    public static double FromMunsell(double hueNumber, string hueLetter)
    {
        hueNumber = hueNumber.Clamp(0, 10);
        hueLetter = hueLetter.Trim().ToUpper();
        
        var bandIndex = Array.IndexOf(Munsell.Hues, hueLetter);
        if (bandIndex == -1) return double.NaN;
            
        var minDegrees = bandIndex * Munsell.DegreesPerHueLetter;
        var maxDegrees = (bandIndex + 1) * Munsell.DegreesPerHueLetter;
        var distance = hueNumber / 10.0;
        var baseDegrees = Interpolation.Linear(minDegrees, maxDegrees, distance);
        var degrees = baseDegrees - 2 * Munsell.DegreesPerHueNumber; // shifts degrees so 5R is 0 instead of 0R / 10RP
        return degrees.Modulo(360);
    }

    public static (double number, string letter) ToMunsell(double degrees)
    {
        if (double.IsNaN(degrees) || double.IsInfinity(degrees)) return (double.NaN, string.Empty);

        var baseDegrees = degrees + 2 * Munsell.DegreesPerHueNumber; // shifts degrees so 0R is 0 instead of 5R
        baseDegrees = baseDegrees.Modulo(360);
        var bandLocation = baseDegrees / Munsell.DegreesPerHueLetter;
        var bandIndex = (int)Math.Truncate(bandLocation);
        var hueLetter = Munsell.Hues[bandIndex];
        var hueNumber = (bandLocation - bandIndex) * 10;
        if (hueNumber != 0) return (hueNumber, hueLetter);
        
        bandIndex = bandIndex == 0 ? Munsell.Hues.Length - 1 : bandIndex - 1;
        hueLetter = Munsell.Hues[bandIndex];
        hueNumber = 10;
        return (hueNumber, hueLetter);
    }
    
    // this is only used by the table of radially interpolated hue segments
    // which are defined anti-clockwise as red -> blue is depicted on a chromaticity diagram
    // in terms of traditional hue degrees, this means the end is defined first
    internal static bool IsBetween(double h, (double number, string letter) end, (double number, string letter) start)
    {
        var startDegrees = FromMunsell(start.number, start.letter);
        var endDegrees = FromMunsell(end.number, end.letter);
        var adapted = Unwrap(startDegrees, endDegrees);
        return IsBetween(h, adapted.start, adapted.end) || IsBetween(h + 360, adapted.start, adapted.end);
    }

    private static bool IsBetween(double h, double start, double end)
    {
        var min = Math.Min(start, end);
        var max = Math.Max(start, end);
        return h >= min && h <= max;
    }
}