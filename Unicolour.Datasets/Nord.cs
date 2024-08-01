namespace Wacton.Unicolour.Datasets;

// https://www.nordtheme.com/
public static class Nord
{
    private static readonly Configuration Config = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65);

    public static readonly Unicolour Nord0 = new(Config, "#2e3440");
    public static readonly Unicolour Nord1 = new(Config, "#3b4252");
    public static readonly Unicolour Nord2 = new(Config, "#434c5e");
    public static readonly Unicolour Nord3 = new(Config, "#4c566a");
    public static readonly Unicolour Nord4 = new(Config, "#d8dee9");
    public static readonly Unicolour Nord5 = new(Config, "#e5e9f0");
    public static readonly Unicolour Nord6 = new(Config, "#eceff4");
    public static readonly Unicolour Nord7 = new(Config, "#8fbcbb");
    public static readonly Unicolour Nord8 = new(Config, "#88c0d0");
    public static readonly Unicolour Nord9 = new(Config, "#81a1c1");
    public static readonly Unicolour Nord10 = new(Config, "#5e81ac");
    public static readonly Unicolour Nord11 = new(Config, "#bf616a");
    public static readonly Unicolour Nord12 = new(Config, "#d08770");
    public static readonly Unicolour Nord13 = new(Config, "#ebcb8b");
    public static readonly Unicolour Nord14 = new(Config, "#a3be8c");
    public static readonly Unicolour Nord15 = new(Config, "#b48ead");

    public static readonly List<Unicolour> PolarNight = new() { Nord0, Nord1, Nord2, Nord3 };
    public static readonly List<Unicolour> SnowStorm = new() { Nord4, Nord5, Nord6 };
    public static readonly List<Unicolour> Frost = new() { Nord7, Nord8, Nord9, Nord10 };
    public static readonly List<Unicolour> Aurora = new() { Nord11, Nord12, Nord13, Nord14, Nord15 };
    
    public static IEnumerable<Unicolour> All => new List<Unicolour>()
        .Concat(PolarNight)
        .Concat(SnowStorm)
        .Concat(Frost)
        .Concat(Aurora);
}