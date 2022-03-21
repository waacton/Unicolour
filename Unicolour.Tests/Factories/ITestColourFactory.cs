namespace Wacton.Unicolour.Tests.Factories;

using Wacton.Unicolour.Tests.Utils;

internal interface ITestColourFactory
{
    TestColour FromRgb255(int r255, int g255, int b255) => FromRgb255(r255, g255, b255, $"RGB [{r255:000} {g255:000} {b255:000}]");
    TestColour FromRgb255(int r255, int g255, int b255, string name)
    {
        var r = r255 / 255.0;
        var g = g255 / 255.0;
        var b = b255 / 255.0;
        return FromRgb(r, g, b, name);
    }
    
    TestColour FromRgb(double r, double g, double b) => FromRgb(r, g, b, $"RGB [{r} {g} {b}]");
    TestColour FromRgb(double r, double g, double b, string name);
    
    TestColour FromHsb(double h, double s, double b) => FromHsb(h, s, b, $"HSB [{h} {s} {b}]");
    TestColour FromHsb(double h, double s, double b, string name);
    
    TestColour FromHsl(double h, double s, double l) => FromHsl(h, s, l, $"HSL [{h} {s} {l}]");
    TestColour FromHsl(double h, double s, double l, string name);
}