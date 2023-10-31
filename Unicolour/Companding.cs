namespace Wacton.Unicolour;

public static class Companding
{
    public static double Gamma(double value, double gamma) => Math.Pow(value, 1 / gamma);
    public static double InverseGamma(double value, double gamma) => Math.Pow(value, gamma);
    
    public static class StandardRgb
    {
        public static double FromLinear(double linear)
        {
            if (double.IsNaN(linear)) return double.NaN;
            return Math.Sign(linear) * Nonlinear(Math.Abs(linear));
            double Nonlinear(double value)
            {
                return value <= 0.0031308
                    ? 12.92 * value
                    : 1.055 * Gamma(value, 2.4) - 0.055;
            }
        }
        
        public static double ToLinear(double nonlinear)
        {
            if (double.IsNaN(nonlinear)) return double.NaN;
            return Math.Sign(nonlinear) * Linear(Math.Abs(nonlinear));
            double Linear(double value)
            {
                return value <= 0.04045 
                    ? value / 12.92 
                    : InverseGamma((value + 0.055) / 1.055, 2.4);
            }
        }
    }

    public static class DisplayP3
    {
        public static double FromLinear(double value) => StandardRgb.FromLinear(value);
        public static double ToLinear(double value) => StandardRgb.ToLinear(value);
    }

    public static class Rec2020
    {
        private const double Alpha = 1.09929682680944;
        private const double Beta = 0.018053968510807;
        
        public static double FromLinear(double linear)
        {
            if (double.IsNaN(linear)) return double.NaN;
            return Math.Sign(linear) * Nonlinear(Math.Abs(linear));
            double Nonlinear(double e)
            {
                if (e < Beta) return 4.5 * e;
                return Alpha * Math.Pow(e, 0.45) - (Alpha - 1);
            }

        }

        public static double ToLinear(double nonlinear)
        {
            if (double.IsNaN(nonlinear)) return double.NaN;
            return Math.Sign(nonlinear) * Linear(Math.Abs(nonlinear));
            double Linear(double ePrime)
            {
                if (ePrime < Beta * 4.5) return ePrime / 4.5;
                return Math.Pow((ePrime + (Alpha - 1)) / Alpha, 1 / 0.45);
            }
        }
    }
}