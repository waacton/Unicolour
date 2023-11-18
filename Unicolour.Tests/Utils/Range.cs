namespace Wacton.Unicolour.Tests.Utils;

public record Range(double Lower, double Upper)
{
    public double At(double fraction) => Interpolation.Interpolate(Lower, Upper, fraction);
    public override string ToString() => $"[{Lower}, {Upper}]";
}