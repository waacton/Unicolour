namespace Wacton.Unicolour.Datasets;

// https://doi.org/10.2307/1420820 Table II(3.7)
public static class MacAdam
{
    private static readonly Configuration Config = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65);

    public static readonly IEnumerable<Unicolour> Limits10 = MacAdamLimits.Get(0.10).Select(chromaticity => new Unicolour(Config, chromaticity, 0.10)).ToArray();
    public static readonly IEnumerable<Unicolour> Limits20 = MacAdamLimits.Get(0.20).Select(chromaticity => new Unicolour(Config, chromaticity, 0.20)).ToArray();
    public static readonly IEnumerable<Unicolour> Limits30 = MacAdamLimits.Get(0.30).Select(chromaticity => new Unicolour(Config, chromaticity, 0.30)).ToArray();
    public static readonly IEnumerable<Unicolour> Limits40 = MacAdamLimits.Get(0.40).Select(chromaticity => new Unicolour(Config, chromaticity, 0.40)).ToArray();
    public static readonly IEnumerable<Unicolour> Limits50 = MacAdamLimits.Get(0.50).Select(chromaticity => new Unicolour(Config, chromaticity, 0.50)).ToArray();
    public static readonly IEnumerable<Unicolour> Limits60 = MacAdamLimits.Get(0.60).Select(chromaticity => new Unicolour(Config, chromaticity, 0.60)).ToArray();
    public static readonly IEnumerable<Unicolour> Limits70 = MacAdamLimits.Get(0.70).Select(chromaticity => new Unicolour(Config, chromaticity, 0.70)).ToArray();
    public static readonly IEnumerable<Unicolour> Limits80 = MacAdamLimits.Get(0.80).Select(chromaticity => new Unicolour(Config, chromaticity, 0.80)).ToArray();
    public static readonly IEnumerable<Unicolour> Limits90 = MacAdamLimits.Get(0.90).Select(chromaticity => new Unicolour(Config, chromaticity, 0.90)).ToArray();
    public static readonly IEnumerable<Unicolour> Limits95 = MacAdamLimits.Get(0.95).Select(chromaticity => new Unicolour(Config, chromaticity, 0.95)).ToArray();
    
    public static IEnumerable<Unicolour> All => new List<Unicolour>()
        .Concat(Limits10)
        .Concat(Limits20)
        .Concat(Limits30)
        .Concat(Limits40)
        .Concat(Limits50)
        .Concat(Limits60)
        .Concat(Limits70)
        .Concat(Limits80)
        .Concat(Limits90)
        .Concat(Limits95);
}