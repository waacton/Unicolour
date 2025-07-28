namespace Wacton.Unicolour;

public class SpectralCoefficients
{
    public int Interval { get; }
    public virtual bool IsValid => GetIsValid();

    internal int Start { get; }
    internal int[] Wavelengths { get; }
    internal double[] Coefficients { get; }
    internal double this[int wavelength] => Coefficients[GetIndex(wavelength)];

    public SpectralCoefficients(int start, int interval, params double[] coefficients)
    {
        // distinct needed when interval = 0, resulting in an array of start wavelengths
        var wavelengths = Enumerable.Range(0, coefficients.Length).Select(i => GetWavelength(start, i, interval)).Distinct().ToArray();

        if (interval >= 0)
        {
            Start = start;
            Interval = interval;
            Coefficients = coefficients;
            Wavelengths = wavelengths.ToArray();
        }
        else
        {
            Start = wavelengths.Last();
            Interval = -interval;
            Coefficients = coefficients.Reverse().ToArray();
            Wavelengths = wavelengths.Reverse().ToArray();
        }
    }
    
    internal double Get(int wavelength, MissingWavelength missingWavelength)
    {
        if (Wavelengths.Contains(wavelength))
        {
            return Coefficients[GetIndex(wavelength)];
        }

        return missingWavelength switch
        {
            MissingWavelength.Zero => 0.0,
            MissingWavelength.Interpolate when Wavelengths.Length == 0 => double.NaN,
            MissingWavelength.Interpolate when Wavelengths.Length == 1 => Coefficients.First(),
            MissingWavelength.Interpolate when wavelength < Wavelengths.Min() => Coefficients.First(),
            MissingWavelength.Interpolate when wavelength > Wavelengths.Max() => Coefficients.Last(),
            MissingWavelength.Interpolate => InterpolateCoefficients(),
            _ => throw new ArgumentOutOfRangeException(nameof(missingWavelength), missingWavelength, null)
        };

        double InterpolateCoefficients()
        {
            var lowerWavelength = Wavelengths.Last(x => x < wavelength);
            var upperWavelength = Wavelengths.First(x => x > wavelength);
            var lower = Coefficients[GetIndex(lowerWavelength)];
            var upper = Coefficients[GetIndex(upperWavelength)];
            var distance = (wavelength - lowerWavelength) / (double)(upperWavelength - lowerWavelength);
            return Interpolation.Linear(lower, upper, distance);
        }
    }
    
    protected bool GetIsValid()
    {
        return Coefficients.Length switch
        {
            1 => true, // every other wavelength determined by MissingWavelength
            0 => false, // every other wavelength determined by MissingWavelength, but cannot perform Interpolate
            _ => Interval switch // when multiple wavelengths present, take interval into account
            {
                0 => false, // zero interval makes no sense (will treat the first coefficient as the only one)
                _ => Start % Interval == 0 // the start wavelength should be divisible by the interval (e.g. 5 nm intervals cannot start from 361 nm)
            }
        };
    }

    private int GetWavelength(int index) => GetWavelength(Start, index, Interval);
    private int GetIndex(int wavelength) => Interval == 0 ? 0 : (wavelength - Start) / Interval;
    
    private static int GetWavelength(int start, int index, int interval) => interval == 0 ? start : start + index * interval;
    
    public override string ToString() => Coefficients.Any() ? $"{Start} nm to {GetWavelength(Wavelengths.Length - 1)} nm by {Interval} nm" : "(no wavelength data)";
}

internal enum MissingWavelength
{
    Zero,
    Interpolate
}