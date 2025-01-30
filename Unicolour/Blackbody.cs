namespace Wacton.Unicolour;

internal static class Blackbody
{
    // https://en.wikipedia.org/wiki/Planckian_locus#The_Planckian_locus_in_the_XYZ_color_space
    internal static Chromaticity GetChromaticity(double cct, Observer observer)
    {
        var wavelengths = Cmf.RequiredWavelengths;
        var xt = Xyz.GetAbsolute(Power, observer.ColourMatchX, wavelengths, delta: 1);
        var yt = Xyz.GetAbsolute(Power, observer.ColourMatchY, wavelengths, delta: 1);
        var zt = Xyz.GetAbsolute(Power, observer.ColourMatchZ, wavelengths, delta: 1);
        var sum = xt + yt + zt;
        var chromaticityX = xt / sum;
        var chromaticityY = yt / sum;
        return new Chromaticity(chromaticityX, chromaticityY);

        double Power(int wavelength) => SpectralPower(wavelength, cct);
    }

    private static double SpectralPower(int wavelengthNanometres, double kelvins)
    {
        /*
         * don't need to reference constants h (planck's), k (boltzmann), or c (speed of light), because:
         * c1 is needed for absolute values, but only relative values are needed
         * c2 is simplified to 1.4388 * 10^-2 (value of c2 as adopted in ITS-90), as suggested by https://doi.org/10.1080/15502724.2014.839020
         * ----------
         * const double h = 6.626070e-34;
         * const double c = 299792458;
         * const double k = 1.380649e-23;
         * var c2 = h * c / k;
         */
        const double c2 = 0.014388;
        var wavelengthMetres = wavelengthNanometres * 1e-9;
        return 1 / Math.Pow(wavelengthMetres, 5) *
               (1 / (Math.Exp(c2 / (wavelengthMetres * kelvins)) - 1));
    }
}