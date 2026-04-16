namespace Wacton.Unicolour.Experimental;

public class HueColourWheel : ColourWheel
{
    private readonly ColourSpace colourSpace;
    private readonly Unicolour reference;
    private Configuration config => reference.Configuration;
    
    private readonly Unicolour white;
    private readonly Unicolour black;
    private readonly Unicolour grey;
    
    internal HueColourWheel(ColourSpace colourSpace, Unicolour reference)
    {
        this.colourSpace = colourSpace;
        this.reference = reference;
        white = new Unicolour(config, ColourSpace.Xyy, 1);
        black = new Unicolour(config, ColourSpace.Xyy, 0);
        grey = new Unicolour(config, ColourSpace.Xyy, 0.5);
    }
    
    public override Unicolour Pure(double hue)
    {
        hue = hue.WithHueModulo();
        
        var representation = reference.GetRepresentation(colourSpace);

        (double, double, double) tuple;
        if (!representation.HasHueComponent)
        {
            tuple = (double.NaN, double.NaN, double.NaN);
        }
        else if (colourSpace == ColourSpace.Wxy)
        {
            tuple = representation.Triplet
                .WithHueMap(h => Hue.FromWavelength(h, config.Xyz))
                .WithHueOverride(hue)
                .WithHueMap(h => Hue.ToWavelength(h, config.Xyz))
                .Tuple;
        }
        else
        {
            tuple = representation.Triplet.WithHueOverride(hue).Tuple;
        }
        
        return new Unicolour(config, colourSpace, tuple, reference.Alpha.A);
    }
    
    public override Unicolour Tint(double hue, double weight)
    {
        var colour = Pure(hue);
        var amount = 0.5 * weight; // 100% weight implies even mix of hue with tint
        return colour.Mix(white, colourSpace, amount);
    }
    
    public override Unicolour Shade(double hue, double weight)
    {
        var colour = Pure(hue);
        var amount = 0.5 * weight; // 100% weight implies even mix of hue with shade
        return colour.Mix(black, colourSpace, amount);
    }
    
    public override Unicolour Tone(double hue, double weight)
    {
        var colour = Pure(hue);
        var amount = 0.5 * weight; // 100% weight implies even mix of hue with tone
        return colour.Mix(grey, colourSpace, amount);
    }
}