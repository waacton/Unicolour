using static Wacton.Unicolour.Planckian;

namespace Wacton.Unicolour;

public record Temperature(double Cct, double Duv = 0.0)
{
    public double Cct { get; } = Cct;
    public double Duv { get; } = Duv;
    public bool IsValid => Math.Abs(Duv) <= 0.05; // CCT should not be used if chromaticity > 0.05 from Planckian locus
    public bool IsHighAccuracy => Cct is >= 1000 and <= 20000; // paper asserts accuracy within 1 K CCT & -0.03 to 0.03 Duv for 1000 K -> 20000 K

    /*
     * temperature in unicolour is the CCT & Duv of the planckian / blackbody locus
     * however CCT is sometimes used with respect to the "daylight locus", derived from the CIE D-illuminants 
     * e.g.
     * - D65 was intended to match 6500 K, now 6504 K ... [more accurately 6503.616, after constant c2 changed from 0.014380 to 0.014388]
     * - D65 chromaticity is ~(0.3127, 0.3290) ... [although there is seemingly no consensus on what the actual accurate value should be]
     * - D65 chromaticity does not lie on the blackbody locus, but is ~0.0032 uv above it ... [on isotherm perpendicular to blackbody locus]
     * - Therefore, chromaticity ~(0.3127, 0.3290) should have a temperature ~(6504 K, Duv 0.003)
     * - Why is the blackbody temperature actually 6502.7 K? 🤷 Suggests D65 isn't actually on the 6504 isotherm? 🤷
     */
    internal static Temperature FromCct(double cct, Locus locus, Planckian planckian)
    {
        return locus switch
        {
            Locus.Blackbody => new Temperature(cct, Duv: 0),
            Locus.Daylight => FromChromaticity(Daylight.GetChromaticity(cct), planckian),
            _ => throw new ArgumentOutOfRangeException(nameof(locus), locus, null)
        };
    }
    
    /*
     * Temperature is a transform of (u, v) Chromaticity
     * Forward: https://doi.org/10.1080/15502724.2014.839020
     * Reverse: https://doi.org/10.1080/15502724.2014.839020
     */
    
    internal static Temperature FromChromaticity(Chromaticity chromaticity, Planckian planckian)
    {
        var searchResult = CascadeExpansionSearch(chromaticity, planckian);
        if (searchResult == null) return new Temperature(double.NaN, double.NaN);
        
        var triangularTemperature = TriangularSolution(searchResult);
        var parabolicTemperature = ParabolicSolution(searchResult, planckian.Observer);
        return Math.Abs(triangularTemperature.Duv) < 0.002 ? triangularTemperature : parabolicTemperature;
    }
    
    internal static Chromaticity ToChromaticity(Temperature temperature, Observer observer)
    {
        var (u0, v0) = Blackbody.GetChromaticity(temperature.Cct, observer).Uv;
        var (u1, v1) = Blackbody.GetChromaticity(temperature.Cct + 0.01, observer).Uv;
        var du = u0 - u1;
        var dv = v0 - v1;
        var denominator = Math.Sqrt(Math.Pow(du, 2) + Math.Pow(dv, 2));
        var sin = dv / denominator;
        var cos = du / denominator;
        var u = u0 - temperature.Duv * sin;
        var v = v0 + temperature.Duv * cos;
        return Chromaticity.FromUv(u, v);
    }

    private static SearchResult? CascadeExpansionSearch(Chromaticity chromaticity, Planckian planckian)
    {
        var coordinates = planckian.StandardRangeCoordinates.Value;
        var isBoundaryCase = false;
        var iteration = 0;
        var stepPercentage = InitialStepPercentage;
        
        while (true)
        {
            var (planckianTable, m) = GetPlanckianTable(coordinates, chromaticity);
            
            var isLowerBoundary = m == 0;
            var isUpperBoundary = m == coordinates.Count - 1;
            if (isLowerBoundary || isUpperBoundary)
            {
                // if at the boundary of the step table, and already dealing with a boundary condition
                // CCT of coordinate is outwith the range 500 K - 1,000,000,000 K and likely not sensible
                if (isBoundaryCase) return null;
                
                // if at the boundary of the step table, and not yet dealing with a boundary condition
                // CCT of coordinate is outwith the initial standard range 1,000 K - 20,000 K
                // so restart the process with a range that is either below or above the standard range
                coordinates = isLowerBoundary 
                    ? planckian.BelowRangeCoordinates.Value 
                    : planckian.AboveRangeCoordinates.Value;
                
                isBoundaryCase = true;
                continue;
            }

            var prev = planckianTable[m - 1];
            var curr = planckianTable[m];
            var next = planckianTable[m + 1];
            if (iteration >= 3)
            {
                return new SearchResult(prev, curr, next, chromaticity.V);
            }

            iteration++;
            stepPercentage /= 10.0;
            coordinates = planckian.Get(prev.T, next.T, stepPercentage);
            isBoundaryCase = false;
        }
    }
    
    private static (List<Distance> planckianTable, int m) GetPlanckianTable(List<Coordinate> coordinates, Chromaticity chromaticity)
    {
        var (ux, vx) = chromaticity.Uv;
        var planckianTable = new List<Distance>();
        var minDistance = double.NaN;
        var m = 0;

        for (var i = 0; i < coordinates.Count; i++)
        {
            var coordinate = coordinates[i];
            var uDelta = ux - coordinate.U;
            var vDelta = vx - coordinate.V;
            var distance = Math.Sqrt(Math.Pow(uDelta, 2) + Math.Pow(vDelta, 2));
            planckianTable.Add(new Distance(coordinate, distance));

            if (distance >= minDistance) continue;
            minDistance = distance;
            m = i;
        }

        return (planckianTable, m);
    }
    
    private static Temperature TriangularSolution(SearchResult searchResult)
    {
        var (prev, _, next, vx) = searchResult;
        
        var l = Math.Sqrt(Math.Pow(next.U - prev.U, 2) + Math.Pow(next.V - prev.V, 2));
        var x = (Math.Pow(prev.D, 2) - Math.Pow(next.D, 2) + Math.Pow(l, 2)) / 2 * l;
        var tx = prev.T + (next.T - prev.T) * (x / l);
        
        // using 0.25% step table so correction not required (also note: 0.99991 is only appropriate for 1% table)
        // var txcor = tx * 0.99991;
        var vtx = prev.V + (next.V - prev.V) * (x / l);
        var sign = vx - vtx >= 0 ? 1 : -1;
        var duv = Math.Pow(Math.Pow(prev.D, 2) - Math.Pow(x, 2), 0.5) * sign;
        return new Temperature(tx, duv);
    }
    
    private static Temperature ParabolicSolution(SearchResult searchResult, Observer observer)
    {
        var (prev, curr, next, vx) = searchResult;

        var x = (next.T - curr.T) * (prev.T - next.T) * (curr.T - prev.T);
        var inverseX = Math.Pow(x, -1);
        
        var a = inverseX * (prev.T * (next.D - curr.D) 
                            + curr.T * (prev.D - next.D) 
                            + next.T * (curr.D - prev.D));
        
        var b = inverseX * -(Math.Pow(prev.T, 2) * (next.D - curr.D) 
                             + Math.Pow(curr.T, 2) * (prev.D - next.D) 
                             + Math.Pow(next.T, 2) * (curr.D - prev.D));
        
        var c = inverseX * -(prev.D * (next.T - curr.T) * curr.T * next.T
                             + curr.D * (prev.T - next.T) * prev.T * next.T
                             + next.D * (curr.T - prev.T) * prev.T * curr.T);

        var tx = -b / (2 * a);
        
        // using 0.25% step table so correction not required (also note: 0.99991 is only appropriate for 1% table)
        // var txcor = tx * 0.99991;
        var vtx = Blackbody.GetChromaticity(tx, observer).V;
        var sign = vx - vtx >= 0 ? 1 : -1;
        var duv = sign * (a * Math.Pow(tx, 2) + b * tx + c);
        return new Temperature(tx, duv);
    }

    public override string ToString() => double.IsNaN(Cct) || double.IsNaN(Duv) ? "-" : $"{Cct:F1} K (Δuv {Duv:F5})";
}

public enum Locus
{
    Blackbody,
    Daylight
}