namespace Wacton.Unicolour.Datasets;

// https://github.com/bastibe/twilight
public class TwilightShifted : Colourmap
{
    internal TwilightShifted()
    {
    }
    
    public override Unicolour Map(double x) => InterpolateLookup(Lookup, x);
    public override string ToString() => nameof(TwilightShifted);

    public static readonly Unicolour[] Lookup = 
        Twilight.Lookup.Skip(Twilight.Lookup.Length / 2)
            .Concat(Twilight.Lookup.Take(Twilight.Lookup.Length / 2))
            .Reverse()
            .ToArray();
}