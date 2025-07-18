namespace Wacton.Unicolour;

public enum HueSpan
{
    Shorter,
    Longer,
    Increasing,
    Decreasing
}

internal static class Hue
{
    internal static (double start, double end) Unwrap(double start, double end, HueSpan hueSpan)
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
}