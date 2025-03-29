namespace Wacton.Unicolour;

public class YbrConfiguration
{
    // note: BT recommendations use the format 219 * X + 16, so the range is 16 to 219 + 16 = 235
    public static readonly YbrConfiguration Rec601 = new(0.299, 0.114, (16, 235), (16, 240), "Rec. 601"); // https://www.itu.int/rec/R-REC-BT.601
    public static readonly YbrConfiguration Rec709 = new(0.2126, 0.0722, (16, 235), (16, 240), "Rec. 709"); // https://www.itu.int/rec/R-REC-BT.709
    public static readonly YbrConfiguration Rec2020 = new(0.2627, 0.0593, (16, 235), (16, 240), "Rec. 2020"); // https://www.itu.int/rec/R-REC-BT.2020
    public static readonly YbrConfiguration Jpeg = new(0.299, 0.114, (0, 255), (0, 255), "JPEG");

    public double Kr { get; }
    public double Kb { get; }
    public double Kg => 1 - (Kr + Kb);
    public (double min, double max) RangeY { get; }
    public (double min, double max) RangeC { get; }
    public string Name { get; }

    public YbrConfiguration(double kr, double kb, (double min, double max) rangeY, (double min, double max) rangeC, string name = Utils.Unnamed)
    {
        Kr = kr;
        Kb = kb;
        RangeY = rangeY;
        RangeC = rangeC;
        Name = name;
    }

    public override string ToString() => $"{Name} · Kr {Kr} · Kb {Kb} · range Y {RangeY} · range C {RangeC}";
}