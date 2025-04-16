namespace Wacton.Unicolour;

// unlike HLG, the nominal white luminance of 203 cd/m² is baked into the transfer functions
// and does not need to be taken into account when scaling for white luminance
internal static class Pq
{
    internal static class Smpte
    {
        private const double M1 = 2610 / 16384.0;
        private const double M2 = 2523 / 4096.0 * 128;
        private const double C1 = 3424 / 4096.0;
        private const double C2 = 2413 / 4096.0 * 32;
        private const double C3 = 2392 / 4096.0 * 32;
        
        internal static double Eotf(double ePrime, double whiteLuminance)
        {
            return Eotf(ePrime) / whiteLuminance;
        }
        
        internal static double Eotf(double ePrime)
        {
            var rootEPrime = Math.Pow(ePrime, 1 / M2);
            var top = Math.Max(rootEPrime - C1, 0);
            var bottom = C2 - C3 * rootEPrime;
            var y = Math.Pow(top / bottom, 1 / M1);
            return 10000 * y;
        }
        
        internal static double InverseEotf(double f, double whiteLuminance)
        {
            return InverseEotf(f * whiteLuminance);
        }
        
        internal static double InverseEotf(double f)
        {
            var y = f / 10000.0;
            var powerY = Math.Pow(y, M1);
            var top = C1 + C2 * powerY;
            var bottom = 1 + C3 * powerY;
            return Math.Pow(top / bottom, M2);
        }
    }

    internal static class Jzazbz
    {
        private static readonly double C1 = 3424 / Math.Pow(2, 12);
        private static readonly double C2 = 2413 / Math.Pow(2, 7);
        private static readonly double C3 = 2392 / Math.Pow(2, 7);
        private static readonly double N = 2610 / Math.Pow(2, 14);
        private static readonly double P = 1.7 * 2523 / Math.Pow(2, 5);
        
        internal static double Eotf(double ePrime, double whiteLuminance)
        {
            return Eotf(ePrime) / whiteLuminance;
        }
        
        internal static double Eotf(double ePrime)
        {
            var rootP = Math.Pow(ePrime, 1 / P);
            var top = C1 - rootP;
            var bottom = C3 * rootP - C2;
            var y = Math.Pow(top / bottom, 1 / N);
            return 10000 * y;
        }
        
        internal static double InverseEotf(double f, double whiteLuminance)
        {
            return InverseEotf(f * whiteLuminance);
        }
        
        internal static double InverseEotf(double f)
        {
            var y = f / 10000.0;
            var powerN = Math.Pow(y, N);
            var top = C1 + C2 * powerN;
            var bottom = 1 + C3 * powerN;
            return Math.Pow(top / bottom, P);
        }
    }
}