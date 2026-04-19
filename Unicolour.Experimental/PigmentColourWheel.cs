using static Wacton.Unicolour.Hue;

namespace Wacton.Unicolour.Experimental;

public class PigmentColourWheel : ColourWheel
{
    private readonly Pigment[] pigments;
    
    internal PigmentColourWheel(Pigment red, Pigment yellow, Pigment blue, Pigment white, Pigment black)
    {
        pigments = [red, yellow, blue, white, black];
    }
    
    public override Unicolour Pure(double hue)
    {
        var weights = Weights(hue);
        return new Unicolour(pigments, weights);
    }

    public override Unicolour Tint(double hue, double weight)
    {
        var weights = Weights(hue);
        weights[3] = weight;
        return new Unicolour(pigments, weights);
    }
    
    public override Unicolour Shade(double hue, double weight)
    {
        var weights = Weights(hue);
        weights[4] = weight;
        return new Unicolour(pigments, weights);
    }
    
    public override Unicolour Tone(double hue, double weight)
    {
        // 1 weight of grey pigment is 1/2 weight white + 1/2 weight black
        var weights = Weights(hue);
        weights[3] = weight / 2.0; 
        weights[4] = weight / 2.0;
        return new Unicolour(pigments, weights);
    }

    internal static double[] Weights(double hue)
    {
        hue = hue.WithHueModulo();
        
        var weights = new double[5];
        weights[0] = PigmentWeight(hue, 0);
        weights[1] = PigmentWeight(hue, 120);
        weights[2] = PigmentWeight(hue, 240);
        weights[3] = 0;
        weights[4] = 0;
        return weights;
    }
    
    private static double PigmentWeight(double hue, double referenceHue)
    {
        const double degreeImpact = 60;
        var unwrapped = Unwrap(hue, referenceHue);
        var distance = Math.Abs(unwrapped.start - unwrapped.end);

        return distance switch
        {
            <= 0 + degreeImpact => 1,
            >= degreeImpact + degreeImpact => 0,
            _ => 1 - (distance - degreeImpact) / degreeImpact
        };
    }
}