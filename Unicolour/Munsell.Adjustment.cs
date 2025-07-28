namespace Wacton.Unicolour;

public partial record Munsell
{
    // note: "angle" in this method refers to polar coordinates of (x, y), not to the hue degrees
    internal static Munsell ModifyHue(Munsell munsell, double targetAngle)
    {
        var (_, v, c) = munsell;
        Adjustment start = null!;
        Adjustment end = new(munsell, targetAngle, isTargetAngle: true);
        if (end.IsWhitePoint)
        {
            return new Munsell(v);
        }
        
        var hueStep = end.Unwrapped.target - end.Unwrapped.angle;
        var converged = false;
        var count = 0;
        const int maxIterations = 20;

        while (!converged && count < maxIterations)
        {
            start = end;
            end = new(new(start.Munsell.H + hueStep, v, c), targetAngle, isTargetAngle: true);
            converged = start.IsBelowTarget && end.IsAboveTarget || start.IsAboveTarget && end.IsBelowTarget;
            count++;
        }
        
        var (startAngle, endAngle) = Hue.Unwrap(start.Angle, end.Angle);
        var totalDistance = endAngle - startAngle; // extremely rare but end can be directly on target
        var distance = totalDistance == 0 ? 0 : (start.Unwrapped.target - start.Unwrapped.angle) / totalDistance;
        
        var (startHue, endHue) = Hue.Unwrap(start.Munsell.H, end.Munsell.H);
        var h = Interpolation.Linear(startHue, endHue, distance).Modulo(360);
        return new Munsell(h, v, c);
    }
    
    internal static Munsell ModifyChroma(Munsell munsell, double targetRadius)
    {
        var (h, v, _) = munsell;
        Adjustment start = null!;
        Adjustment end = new(munsell, targetRadius, isTargetAngle: false);
        if (end.IsWhitePoint)
        {
            return new Munsell(v);
        }
        
        var chromaFactor = end.Target / end.Radius;
        var converged = false;
        var count = 0;
        const int maxIterations = 20;

        while (!converged && count < maxIterations)
        {
            start = end;
            end = new(new(h, v, start.Munsell.C * chromaFactor), targetRadius, isTargetAngle: false);
            converged = start.IsBelowTarget && end.IsAboveTarget || start.IsAboveTarget && end.IsBelowTarget;
            count++;
        }
        
        var totalDistance = end.Radius - start.Radius; // extremely rare but end can be directly on target
        var distance = totalDistance == 0 ? 0 : (start.Target - start.Radius) / totalDistance;
        var c = Interpolation.Linear(start.Munsell.C, end.Munsell.C, distance);
        return new Munsell(h, v, c);
    }
    
    private record Adjustment
    {
        internal Munsell Munsell { get; }
        internal double Angle { get; }
        internal double Radius { get; }
        internal bool IsWhitePoint => Radius == 0.0;
        internal double Target { get; }
        private bool IsTargetAngle { get; }
        internal (double angle, double target) Unwrapped { get; }
        internal bool IsBelowTarget => IsTargetAngle ? Unwrapped.angle <= Unwrapped.target : Radius <= Target;
        internal bool IsAboveTarget => IsTargetAngle ? Unwrapped.angle >= Unwrapped.target : Radius >= Target;
        
        internal Adjustment(Munsell munsell, double target, bool isTargetAngle)
        {
            Munsell = munsell;
            (Radius, Angle) = LineSegment.Polar(WhiteChromaticity, ToXyy(munsell, XyzConfigC).Chromaticity);
            Target = target;
            IsTargetAngle = isTargetAngle;
            Unwrapped = Hue.Unwrap(Angle, Target); 
        }
    }
}