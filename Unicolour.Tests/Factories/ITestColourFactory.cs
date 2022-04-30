namespace Wacton.Unicolour.Tests.Factories;

using Wacton.Unicolour.Tests.Utils;

internal interface ITestColourFactory
{
    TestColour FromRgb255(int r255, int g255, int b255) => FromRgb255(r255, g255, b255, $"RGB ({r255:000} {g255:000} {b255:000})");
    TestColour FromRgb255(int r255, int g255, int b255, string name)
    {
        var r = r255 / 255.0;
        var g = g255 / 255.0;
        var b = b255 / 255.0;
        return FromRgb(r, g, b, name);
    }
    
    TestColour FromRgb(ColourTriplet triplet) => FromRgb(triplet.First, triplet.Second, triplet.Third, $"RGB {triplet}");
    TestColour FromRgb(double r, double g, double b, string name);
    
    TestColour FromHsb(ColourTriplet triplet) => FromHsb(triplet.First, triplet.Second, triplet.Third, $"HSB {triplet}");
    TestColour FromHsb(double h, double s, double b, string name);
    
    TestColour FromHsl(ColourTriplet triplet) => FromHsl(triplet.First, triplet.Second, triplet.Third, $"HSL {triplet}");
    TestColour FromHsl(double h, double s, double l, string name);
    
    TestColour FromXyz(ColourTriplet triplet) => FromXyz(triplet.First, triplet.Second, triplet.Third, $"XYZ {triplet}");
    TestColour FromXyz(double x, double y, double z, string name);
    
    TestColour FromLab(ColourTriplet triplet) => FromLab(triplet.First, triplet.Second, triplet.Third, $"LAB {triplet}");
    TestColour FromLab(double l, double a, double b, string name);
    
    TestColour FromLchab(ColourTriplet triplet) => FromLchab(triplet.First, triplet.Second, triplet.Third, $"LCHab {triplet}");
    TestColour FromLchab(double l, double c, double h, string name);
    
    TestColour FromLuv(ColourTriplet triplet) => FromLuv(triplet.First, triplet.Second, triplet.Third, $"LUV {triplet}");
    TestColour FromLuv(double l, double u, double v, string name);
    
    TestColour FromLchuv(ColourTriplet triplet) => FromLchuv(triplet.First, triplet.Second, triplet.Third, $"LCHuv {triplet}");
    TestColour FromLchuv(double l, double c, double h, string name);
}