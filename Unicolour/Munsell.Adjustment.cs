using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public partial record Munsell
{
    // note: "angle" in this method refers to polar coordinates of (x, y), not to the hue degrees
    internal static double ModifyHue(double h, double v, double c, double targetAngle)
    {
        Adjustment start = null!;
        Adjustment end = new(h, v, c, targetAngle, isTargetAngle: true);
        if (end.IsWhitePoint)
        {
            return 0;
        }
        
        var hueStep = end.Unwrapped.target - end.Unwrapped.angle;
        var converged = false;
        var count = 0;
        const int maxIterations = 20;

        while (!converged && count < maxIterations)
        {
            start = end;
            end = new(start.H + hueStep, v, c, targetAngle, isTargetAngle: true);
            converged = start.IsBelowTarget && end.IsAboveTarget || start.IsAboveTarget && end.IsBelowTarget;
            count++;
        }
        
        var (startAngle, endAngle) = Hue.Unwrap(start.Angle, end.Angle);
        var totalDistance = endAngle - startAngle; // extremely rare but end can be directly on target
        var distance = totalDistance == 0 ? 0 : (start.Unwrapped.target - start.Unwrapped.angle) / totalDistance;
        
        var (startHue, endHue) = Hue.Unwrap(start.H, end.H);
        return Interpolation.Linear(startHue, endHue, distance).WithHueModulo();
    }
    
    internal static double ModifyChroma(double h, double v, double c, double targetRadius)
    {
        Adjustment start = null!;
        Adjustment end = new(h, v, c, targetRadius, isTargetAngle: false);
        if (end.IsWhitePoint)
        {
            return 0;
        }
        
        var chromaFactor = end.Target / end.Radius;
        var converged = false;
        var count = 0;
        const int maxIterations = 20;

        while (!converged && count < maxIterations)
        {
            start = end;
            end = new(h, v, start.C * chromaFactor, targetRadius, isTargetAngle: false);
            converged = start.IsBelowTarget && end.IsAboveTarget || start.IsAboveTarget && end.IsBelowTarget;
            count++;
        }
        
        var totalDistance = end.Radius - start.Radius; // extremely rare but end can be directly on target
        var distance = totalDistance == 0 ? 0 : (start.Target - start.Radius) / totalDistance;
        return Interpolation.Linear(start.C, end.C, distance);
    }
    
    private record Adjustment
    {
        internal double H { get; }
        internal double C { get; }
        internal double Angle { get; }
        internal double Radius { get; }
        internal bool IsWhitePoint => Radius == 0.0;
        internal double Target { get; }
        private bool IsTargetAngle { get; }
        internal (double angle, double target) Unwrapped { get; }
        internal bool IsBelowTarget => IsTargetAngle ? Unwrapped.angle <= Unwrapped.target : Radius <= Target;
        internal bool IsAboveTarget => IsTargetAngle ? Unwrapped.angle >= Unwrapped.target : Radius >= Target;
        
        internal Adjustment(double h, double v, double c, double target, bool isTargetAngle)
        {
            H = h;
            C = c;
            var (chromaticity, _) = ToXyy(h, v, c, MunsellXyzConfig.ChromaticAdaptor);
            (Radius, Angle) = Polar(MunsellWhitePoint.Chromaticity, chromaticity);
            Target = target;
            IsTargetAngle = isTargetAngle;
            Unwrapped = Hue.Unwrap(Angle, Target); 
        }
    }
}