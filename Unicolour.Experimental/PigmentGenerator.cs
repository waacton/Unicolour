using System;
using System.Linq;
using static Wacton.Unicolour.Experimental.MatrixUtils;

namespace Wacton.Unicolour.Experimental;

public static class PigmentGenerator
{
    private static readonly Illuminant illuminant = Illuminant.D65;
    private static readonly Observer observer = Observer.Degree2;
    private static readonly WhitePoint d65WhitePoint = illuminant.GetWhitePoint(observer);
    
    internal const int Start = 380;
    internal const int Interval = 10;
    internal const int Wavelengths = 36;
    private static double[] NotNumber => new double[Wavelengths].Select(_ => double.NaN).ToArray();

    internal const double Tolerance = 1e-8;
    
    public static Pigment From(Unicolour colour)
    {
        var rgb = colour.Rgb.Triplet;
        var xyz = colour.Xyz.Triplet;
        var xyzConfig = colour.Configuration.Xyz;
        
        // handle black and white as special conditions
        // unlikely to encounter exact RGB of 0s or 1s unless intentionally specified
        // for black reflectance, want close to zero reflectance
        // (zero and double.Epsilon are too small and results in NaN when converted to XYZ)
        var reflectance = rgb switch
        {
            (0.0, 0.0, 0.0) => new double[Wavelengths].Select(_ => Tolerance).ToArray(),
            (1.0, 1.0, 1.0) => new double[Wavelengths].Select(_ => 1.0).ToArray(),
            _ => GenerateReflectance(xyz, xyzConfig)
        };

        return new Pigment(Start, Interval, reflectance);
    }

    /*
     * https://doi.org/10.1002/col.22437 using LHTSS (see also http://scottburns.us/reflectance-curves-from-srgb-2/)
     * generating a curve for "object colours" (reflectance distribution between 0 - 1)
     * ----------
     * NOTE: also implemented LLSS to generate a curve for any non-imaginary colour (lies within spectral locus)
     * to attempt to better support wide-gamut (http://scottburns.us/rec-2020-rgb-to-spectrum-conversion-for-reflectances/)
     * but was inaccurate in roundtrip tests; perhaps one to revisit in future
     */
    private static double[] GenerateReflectance(ColourTriplet xyz, XyzConfiguration xyzConfig)
    {
        // custom illuminant from users might not have SPD, or SPD might not have the required wavelengths
        // so for simplicity and consistency, calculations are performed in default D65/2° (also assumed by matrix A)
        var d65Xyz = Adaptation.WhitePoint(xyz, xyzConfig.WhitePoint, d65WhitePoint);
        var d65XyzMatrix = Matrix.FromTriplet(d65Xyz);
        
        var z = new double[Wavelengths];
        var lambda = new double[3];
        var iteration = 0;

        while (iteration < 20)
        {
            var f = Lhtss.F(z, lambda, d65XyzMatrix);
            var j = Lhtss.J(z, lambda);
            
            var delta = SystemOfLinearEquations.Solve(j, f.Negate());
            var zDelta = delta.Take(Wavelengths).ToArray();
            var lambdaDelta = delta.Skip(Wavelengths).ToArray();

            z = z.Zip(zDelta, (x, xDelta) => x + xDelta).ToArray();
            lambda = lambda.Zip(lambdaDelta, (x, xDelta) => x + xDelta).ToArray();
            
            if (f.Any(double.IsNaN)) return NotNumber;
            if (f.All(value => Math.Abs(value) < Tolerance)) return z.Select(Lhtss.Rho).ToArray();
            
            iteration++;
        }

        return NotNumber;
    }
    
    // http://scottburns.us/reflectance-curves-from-srgb-10/
    private static class Lhtss
    {
        internal static double Rho(double z) => (Math.Tanh(z) + 1) / 2.0; // ρ 
        
        internal static double[] F(double[] z, double[] lambda, Matrix xyz)
        {
            var sechZ = z.Select(value => Math.Pow(Sech(value), 2) / 2.0);
            var rhoZ = z.Select(Rho);

            var fUpper = D.Value.Multiply(z).Zip(Diag(sechZ).Multiply(A.Value).Multiply(lambda), (a, b) => a + b);
            var fLower = A.Value.Transpose().Multiply(rhoZ).Zip(xyz, (a, b) => a - b);
            return Concat(fUpper, fLower);
        }
    
        internal static Matrix J(double[] z, double[] lambda)
        {
            var sechTanhZ = z.Select(value => Math.Pow(Sech(value), 2) * Math.Tanh(value));
            var sechZ = z.Select(value => Math.Pow(Sech(value), 2) / 2.0);
            var diagSechZ = Diag(sechZ);

            var upperLeft = D.Value.Zip(Diag(Diag(sechTanhZ).Multiply(A.Value).Multiply(lambda).GetCol(0)), (a, b) => a - b);
            var upperRight = diagSechZ.Multiply(A.Value);
            var lowerLeft = A.Value.Transpose().Multiply(diagSechZ);
            var lowerRight = new Matrix(new double[3, 3]); // all zeroes
            return Concat(upperLeft, upperRight, lowerLeft, lowerRight);
        }
        
        private static double Sech(double value) => 1 / Math.Cosh(value);
    }
    
    private static readonly Lazy<Matrix> A = new(GetA);
    private static Matrix GetA()
    {
        var a = new Matrix(new double[Wavelengths, 3]);
        var w = new double[Wavelengths];
        for (var i = 0; i < Wavelengths; i++)
        {
            var wavelength = Start + i * Interval;
            a[i, 0] = observer.ColourMatchX(wavelength);
            a[i, 1] = observer.ColourMatchY(wavelength);
            a[i, 2] = observer.ColourMatchZ(wavelength);
            w[i] = illuminant.Spd![wavelength];
        }
        
        // normalise w by second CMF column (Y - luminance)
        var scale = a.GetCol(1).Zip(w, (an, wn) => an * wn).Sum();
        w = w.Select(wn => wn / scale).ToArray();
        return Diag(w).Multiply(a);
    }
    
    // tridiagonal
    private static readonly Lazy<Matrix> D = new(GetD);
    private static Matrix GetD()
    {
        var data = new double[Wavelengths, Wavelengths];
        for (var i = 0; i < Wavelengths; i++)
        {
            data[i, i] = 4;
            
            if (i + 1 < Wavelengths)
            {
                data[i, i + 1] = -2;
                data[i + 1, i] = -2;
            }
            
            if (i - 1 > 0)
            {
                data[i, i - 1] = -2;
                data[i - 1, i] = -2;
            }
        }

        data[0, 0] = 2;
        data[Wavelengths - 1, Wavelengths - 1] = 2;
        
        return new Matrix(data);
    }
}