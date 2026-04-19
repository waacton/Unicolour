using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Hct : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double C => Second;
    public double T => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsTripletAchromatic => false;
    
    public Hct(double h, double c, double t) : this(h, c, t, Limitation.None) {}
    public Hct(double t) : this(0, 0, t, Limitation.Achromatic) {}
    internal Hct(double h, double c, double t, Limitation limitation) : base(h, c, t, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{H:F1}° {C:F2} {T:F2}" : $"{NoHue}° {C:F2} {T:F2}";
    public override string ToString() => base.ToString();
    
    /*
     * HCT is a transform of XYZ
     * (just a combination of LAB & CAM16, but with specific XYZ & CAM configuration, so can't reuse existing colour space calculations)
     * Forward: https://material.io/blog/science-of-color-design
     * Reverse: n/a - no published reverse transform and I don't want to port Google code, so using my own naive search
     *
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    private static readonly WhitePoint HctWhitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
    private static readonly ChromaticAdaptor HctChromaticAdaptor = new(HctWhitePoint, ChromaticAdaptation.Bradford);

    internal static Cam16 Cam16Component(Xyz xyz) => Cam16.FromXyz(xyz, CamConfiguration.Hct, HctChromaticAdaptor);
    internal static Lab LabComponent(Xyz xyz) => Lab.FromXyz(xyz);
    
    internal static Hct FromXyz(Xyz xyz, ChromaticAdaptor chromaticAdaptor)
    {
        var d65Xyz = chromaticAdaptor.AdaptTo(xyz, HctWhitePoint);
        var cam16 = Cam16Component(d65Xyz);
        var lab = LabComponent(d65Xyz);

        var h = cam16.Model.H;
        var c = cam16.Model.C;
        var t = lab.L;
        return new Hct(h, c, t, xyz.Limitation);
    }
    
    internal static Xyz ToXyz(Hct hct, ChromaticAdaptor chromaticAdaptor)
    {
        var targetY = Lab.ToXyz(new Lab(hct.T, 0, 0), HctWhitePoint).Y;
        var result = FindBestJ(targetY, hct);
        var d65Xyz = result.Converged ? result.Data.Xyz : new Xyz(double.NaN, double.NaN, double.NaN, HctWhitePoint);
        var (x, y, z) = chromaticAdaptor.AdaptFrom(d65Xyz);
        return new Xyz(x, y, z, chromaticAdaptor.WhitePoint, hct.Limitation) { HctToXyzSearchResult = result };
    }

    // i'm sure some smart people have some fancy-pants algorithms to do this efficiently
    // but until there's some kind of published reverse transformation algorithm, this gets the job done
    // (albeit rather slowly...)
    private static HctToXyzSearchResult FindBestJ(double targetY, Hct hct)
    {
        var latest = GetStartingData(targetY, hct);
        var best = latest;
        
        var step = latest.J;
        var iterations = 0;
        while (!double.IsNaN(latest.DeltaY) && Math.Abs(latest.DeltaY) > 0.000000001 && iterations < 100)
        {
            var j = latest.J + (latest.DeltaY > 0 ? -step : step);
            var data = ProcessJ(targetY, j, hct);
            var deltaY = data.DeltaY;
            if (Math.Abs(deltaY) < Math.Abs(best.DeltaY))
            {
                best = data;
            }

            // change in sign of delta means target is now in the other direction
            var overshot = double.IsNaN(deltaY) || Math.Sign(latest.DeltaY) != Math.Sign(deltaY);
            if (overshot)
            {
                step /= 2.0;
            }

            latest = data;
            iterations++;
        }

        var converged = !double.IsNaN(latest.DeltaY) && iterations < 100;
        return new HctToXyzSearchResult(best, iterations, converged);
    }
    
    private static HctToXyzSearchData GetStartingData(double targetY, Hct hct)
    {
        List<(double, double)> xzPairs = [(0, 0), (0, 1), (1, 0), (1, 1)];
        var best = InitialData;
        
        foreach (var (x, z) in xzPairs)
        {
            var xyz = new Xyz(x, targetY, z, HctWhitePoint);
            var j = Cam16.FromXyz(xyz, CamConfiguration.Hct, HctChromaticAdaptor).Model.J;
            var data = ProcessJ(targetY, j, hct);
            if (Math.Abs(data.DeltaY) < best.DeltaY)
            {
                best = data;
            }
        }

        return best;
    }

    private static HctToXyzSearchData ProcessJ(double targetY, double j, Hct hct)
    {
        var (h, c, _) = hct;
        var camModel = new Cam.Model(j, c, h, 0, 0, 0);
        var cam16 = new Cam16(camModel, CamConfiguration.Hct, Limitation.None);
        var xyz = Cam16.ToXyz(cam16, CamConfiguration.Hct, HctChromaticAdaptor);
        var deltaY = xyz.Y - targetY;
        return new HctToXyzSearchData(hct, j, cam16, xyz, targetY, deltaY);
    }
    
    private static readonly HctToXyzSearchData InitialData = new(
        J: double.PositiveInfinity, DeltaY: double.PositiveInfinity, 
        Hct: null!, Cam16: null!, Xyz: null!, TargetY: double.NaN);
}

// only for potential debugging or diagnostics
// until there is an "official" HCT -> XYZ reverse transform
internal record HctToXyzSearchResult(HctToXyzSearchData Data, int Iterations, bool Converged)
{
    internal HctToXyzSearchData Data { get; } = Data;
    internal int Iterations { get; } = Iterations;
    internal bool Converged { get; } = Converged;
    public override string ToString() => $"{Data} · Iterations:{Iterations} · Converged:{Converged}";
}

internal record HctToXyzSearchData(Hct Hct, double J, Cam16 Cam16, Xyz Xyz, double TargetY, double DeltaY)
{
    internal Hct Hct { get; } = Hct;
    internal double J { get; } = J;
    internal Cam16 Cam16 { get; } = Cam16;
    internal Xyz Xyz { get; } = Xyz;
    internal double TargetY { get; } = TargetY;
    internal double DeltaY { get; } = DeltaY;
    public override string ToString() => $"J:{J:F4} · ΔY:{DeltaY:F4}";
}