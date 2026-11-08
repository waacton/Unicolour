namespace Wacton.Unicolour;

public record Cam16ViewingConditions(
    double C, double Nc, double Dr, double Dg, double Db, 
    double Fl, double N, double Z, double Nbb, double Ncb, double Aw)
{
    public double C { get; } = C;
    public double Nc { get; } = Nc;
    public double Dr { get; } = Dr;
    public double Dg { get; } = Dg;
    public double Db { get; } = Db;
    public double Fl { get; } = Fl;
    public double N { get; } = N;
    public double Z { get; } = Z;
    public double Nbb { get; } = Nbb;
    public double Ncb { get; } = Ncb;
    public double Aw { get; } = Aw;
}