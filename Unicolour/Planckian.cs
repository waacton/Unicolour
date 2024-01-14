namespace Wacton.Unicolour;

internal class Planckian
{
    internal const double InitialStepPercentage = 0.25;
    internal Observer Observer { get; }
    internal Lazy<List<Coordinate>> StandardRangeCoordinates { get; }
    internal Lazy<List<Coordinate>> BelowRangeCoordinates { get; }
    internal Lazy<List<Coordinate>> AboveRangeCoordinates { get; }

    internal Planckian(Observer observer)
    {
        Observer = observer;
        StandardRangeCoordinates = new Lazy<List<Coordinate>>(() => Get(1000, 20000, InitialStepPercentage));
        BelowRangeCoordinates = new Lazy<List<Coordinate>>(() => Get(500, 1000, InitialStepPercentage));
        AboveRangeCoordinates = new Lazy<List<Coordinate>>(() => Get(20000, 1e9, InitialStepPercentage));
    }
    
    internal List<Coordinate> Get(double startCct, double endCct, double stepPercentage)
    {
        var coordinates = new List<Coordinate>();
        var cct = startCct;
        var stepMultiplier = stepPercentage / 100.0;

        var shouldFinish = false;
        while (!shouldFinish)
        {
            coordinates.Add(FromCct(cct));
            shouldFinish = cct > endCct;
            cct += cct * stepMultiplier;
        }
        
        // insert an additional coordinate before the start CCT
        // so that the start CCT can be matched without issue
        var beforeFirstCct = startCct - startCct * stepMultiplier;
        coordinates.Insert(0, FromCct(beforeFirstCct));
        return coordinates;
    }
    
    private Coordinate FromCct(double cct)
    {
        var (u, v) = Blackbody.GetChromaticity(cct, Observer).Uv;
        return new Coordinate(cct, u, v);
    }
    
    internal record Coordinate(double T, double U, double V)
    {
        internal double T { get; } = T;
        internal double U { get; } = U;
        internal double V { get; } = V;

        public override string ToString() => $"{T:F2}K · {U:F6}, {V:F6}";
    }
    
    internal record Distance(Coordinate Coordinate, double D)
    {
        internal Coordinate Coordinate { get; } = Coordinate;
        internal double T => Coordinate.T;
        internal double U => Coordinate.U;
        internal double V => Coordinate.V;
        internal double D { get; } = D;
    
        public override string ToString() => $"{Coordinate} · {D:F6}";
    }

    internal record SearchResult(Distance Previous, Distance Closest, Distance Next, double Vx)
    {
        internal Distance Previous { get; } = Previous;
        internal Distance Closest { get; } = Closest;
        internal Distance Next { get; } = Next;
        internal double Vx { get; } = Vx;

        public override string ToString() => $"{Previous.T} < {Closest.T} < {Next.T}";
    }

    public override string ToString() => $"Planckian tables for {Observer}";
}