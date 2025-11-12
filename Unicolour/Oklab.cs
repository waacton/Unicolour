using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Oklab : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double L => First;
    public double A => Second;
    public double B => Third;
    internal override bool IsGreyscale => L is <= 0.0 or >= 1.0 || (A.Equals(0.0) && B.Equals(0.0));

    public Oklab(double l, double a, double b) : this(l, a, b, ColourHeritage.None) {}
    internal Oklab(ColourTriplet triplet, ColourHeritage heritage) : this(triplet.First, triplet.Second, triplet.Third, heritage) {}
    internal Oklab(double l, double a, double b, ColourHeritage heritage) : base(l, a, b, heritage) {}

    protected override string String => $"{L:F2} {A:+0.00;-0.00;0.00} {B:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * OKLAB is a transform of XYZ 
     * Forward: https://bottosson.github.io/posts/oklab/#converting-from-xyz-to-oklab
     * Reverse: https://bottosson.github.io/posts/oklab/#converting-from-xyz-to-oklab
     *
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    /*
     * NOTE: this definition of M1 is no longer used
     * internal static readonly Matrix M1 = new(new[,]
     * {
     *     { +0.8189330101, +0.3618667424, -0.1288597137 },
     *     { +0.0329845436, +0.9293118715, +0.0361456387 },
     *     { +0.0482003018, +0.2643662691, +0.6338517070 }
     * });
    * --------------------
     * article defines XYZ -> OKLAB transform but source code performs direct RGB -> OKLAB transform (https://github.com/bottosson/bottosson.github.io/blob/f6f08b7fde9436be1f20f66cebbc739d660898fd/misc/colorpicker/colorconversion.js#L163)
     * RGB -> OKLAB behaviour only matches XYZ -> OKLAB when white point values are the ones that were used to derive M1
     * however there is no agreement on white point values 😒 (https://ninedegreesbelow.com/photography/well-behaved-profiles-quest.html#white-point-values)
     * see also: https://github.com/w3c/csswg-drafts/issues/6642#issuecomment-943521484
     * also discussed in `KnownOklabTests.cs`
     * --------------------
     * Unicolour assumes that
     * - RGB -> OKLAB is the intended transformation, using the matrix defined there
     * - XYZ -> OKLAB matrix M1 is a result of RGB -> OKLAB at specific white point values
     * in which case M1 needs to be calculated according to RGB configuration to ensure OKLAB values for RGB colours are correct
     * --------------------
     * LMS = RgbToOklab * Rgb
     * LMS = M1 * RgbToXyz * Rgb
     * RgbToOklab = M1 * RgbToXyz
     * M1 = RgbToOklab * RgbToXyz^-1
     * --------------------
     * 💩💩💩 the "proper" way to define M1 is hidden in some random GitHub thread, instead of in any Oklab documentation
     * https://github.com/w3c/csswg-drafts/issues/6642#issuecomment-945714988
     * modern colour spaces seem to make it hard to implement things accurately (see also: XYB)
     * will consider reworking M1 calculation if there's ever any complaints to my reverse engineering
     */
    
    private static readonly WhitePoint D65WhitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
    
    private static Matrix GetM1(Matrix rgbToXyzMatrix) => RgbToOklab.Multiply(rgbToXyzMatrix.Inverse());
    private static readonly Matrix RgbToOklab = new(new[,]
    {
        { 0.4122214708, 0.5363325363, 0.0514459929 },
        { 0.2119034982, 0.6806995451, 0.1073969566 },
        { 0.0883024619, 0.2817188376, 0.6299787005 }
    });
    
    private static readonly Matrix M2 = new(new[,]
    {
        { +0.2104542553, +0.7936177850, -0.0040720468 },
        { +1.9779984951, -2.4285922050, +0.4505937099 },
        { +0.0259040371, +0.7827717662, -0.8086757660 }
    });
    
    internal static Oklab FromXyz(Xyz xyz, XyzConfiguration xyzConfig, RgbConfiguration rgbConfig)
    {
        var xyzMatrix = Matrix.From(xyz);
        var d65Matrix = Adaptation.WhitePoint(xyzMatrix, xyzConfig.WhitePoint, D65WhitePoint, xyzConfig.AdaptationMatrix);
        var m1 = GetM1(rgbConfig.RgbToXyzMatrix);
        var lmsMatrix = m1.Multiply(d65Matrix);
        var lmsNonLinearMatrix = lmsMatrix.Select(CubeRoot);
        var labMatrix = M2.Multiply(lmsNonLinearMatrix);
        return new Oklab(labMatrix.ToTriplet(), ColourHeritage.From(xyz));
    }
    
    internal static Xyz ToXyz(Oklab oklab, XyzConfiguration xyzConfig, RgbConfiguration rgbConfig)
    {
        var labMatrix = Matrix.From(oklab);
        var lmsNonLinearMatrix = M2.Inverse().Multiply(labMatrix);
        var lmsMatrix = lmsNonLinearMatrix.Select(x => Math.Pow(x, 3));
        var m1 = GetM1(rgbConfig.RgbToXyzMatrix);
        var d65Matrix = m1.Inverse().Multiply(lmsMatrix);
        var xyzMatrix = Adaptation.WhitePoint(d65Matrix, D65WhitePoint, xyzConfig.WhitePoint, xyzConfig.AdaptationMatrix);
        return new Xyz(xyzMatrix.ToTriplet(), ColourHeritage.From(oklab));
    }
}