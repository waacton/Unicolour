namespace Wacton.Unicolour.Tests.Utils;

public record Range(double Lower, double Upper)
{
    internal double Distance => Upper - Lower;
    internal double At(double fraction) => Interpolation.Linear(Lower, Upper, fraction);
    public override string ToString() => $"[{Lower}, {Upper}]";
}