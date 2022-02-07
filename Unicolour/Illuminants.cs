namespace Wacton.Unicolour;

public static class Illuminants
{
    // as far as I'm aware, these are the latest ASTM standards
    private static readonly Dictionary<Observer, Dictionary<Illuminant, (double x, double y, double z)>> WhitePoints = new()
        {
            {
                Observer.Standard2, new()
                {
                    {Illuminant.A, (109.850, 100.000, 35.585)},
                    {Illuminant.C, (98.074, 100.000, 118.232)},
                    {Illuminant.D50, (96.422, 100.000, 82.521)},
                    {Illuminant.D55, (95.682, 100.000, 92.149)},
                    {Illuminant.D65, (95.047, 100.000, 108.883)},
                    {Illuminant.D75, (94.972, 100.000, 122.638)},
                    {Illuminant.E, (100.000, 100.000, 100.000)},
                    {Illuminant.F2, (99.186, 100.000, 67.393)},
                    {Illuminant.F7, (95.041, 100.000, 108.747)},
                    {Illuminant.F11, (100.962, 100.000, 64.350)}
                }
            },
            {
                Observer.Supplementary10, new()
                {
                    {Illuminant.A, (111.144, 100.000, 35.200)},
                    {Illuminant.C, (97.285, 100.000, 116.145)},
                    {Illuminant.D50, (96.720, 100.000, 81.427)},
                    {Illuminant.D55, (95.799, 100.000, 90.926)},
                    {Illuminant.D65, (94.811, 100.000, 107.304)},
                    {Illuminant.D75, (94.416, 100.000, 120.641)},
                    {Illuminant.E, (100.000, 100.000, 100.000)},
                    {Illuminant.F2, (103.279, 100.000, 69.027)},
                    {Illuminant.F7, (95.792, 100.000, 107.686)},
                    {Illuminant.F11, (103.863, 100.000, 65.607)}
                }
            }
        };

    public static (double x, double y, double z) ReferenceWhite(Illuminant illuminant, Observer observer)
    {
        return WhitePoints[observer][illuminant];
    }
}

public enum Illuminant
{
    A,
    C,
    D50,
    D55,
    D65,
    D75,
    E,
    F2,
    F7,
    F11
}
    
public enum Observer
{
    Standard2,
    Supplementary10
}