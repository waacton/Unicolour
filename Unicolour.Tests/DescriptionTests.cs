using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class DescriptionTests
{
    [Test, Combinatorial]
    public void LightnessNotApplicable(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(-0.000000000000001, 0, 0.5, 1, 1.000000000000001)] double s,
        [Values(double.NaN)] double l)
    {
        var included = ColourDescription.NotApplicable;
        List<ColourDescription> excluded = [];
        excluded.AddRange(ColourDescription.Hues);
        excluded.AddRange(ColourDescription.Saturations);
        excluded.AddRange(ColourDescription.Lightnesses);
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void LightnessBlack(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(-0.000000000000001, 0, 0.5, 1, 1.000000000000001)] double s,
        [Values(-0.000000000000001, 0)] double l)
    {
        var included = ColourDescription.Black;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues);
        excluded.AddRange(ColourDescription.Saturations);
        excluded.AddRange(ColourDescription.Lightnesses);
        excluded.AddRange([ColourDescription.White, ColourDescription.Grey]);
        AssertDescription(h, s, l, included, excluded);
    }

    [Test, Combinatorial]
    public void LightnessShadow(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(-0.000000000000001, 0, 0.5, 1, 1.000000000000001)] double s,
        [Values(0.000000000000001, 0.199999999999999)] double l)
    {
        var included = ColourDescription.Shadow;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Lightnesses.Except([included]));
        excluded.AddRange([ColourDescription.Black, ColourDescription.White]);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void LightnessDark(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(-0.000000000000001, 0, 0.5, 1, 1.000000000000001)] double s,
        [Values(0.2, 0.399999999999999)] double l) 
    {
        var included = ColourDescription.Dark;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Lightnesses.Except([included]));
        excluded.AddRange([ColourDescription.Black, ColourDescription.White]);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void LightnessPure(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(-0.000000000000001, 0, 0.5, 1, 1.000000000000001)] double s,
        [Values(0.4, 0.599999999999999)] double l) 
    {
        var included = ColourDescription.Pure;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Lightnesses.Except([included]));
        excluded.AddRange([ColourDescription.Black, ColourDescription.White]);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void LightnessLight(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(-0.000000000000001, 0, 0.5, 1, 1.000000000000001)] double s,
        [Values(0.6, 0.799999999999999)] double l) 
    {
        var included = ColourDescription.Light;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Lightnesses.Except([included]));
        excluded.AddRange([ColourDescription.Black, ColourDescription.White]);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void LightnessPale(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(-0.000000000000001, 0, 0.5, 1, 1.000000000000001)] double s,
        [Values(0.8, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Pale;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Lightnesses.Except([included]));
        excluded.AddRange([ColourDescription.Black, ColourDescription.White]);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void LightnessWhite(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(-0.000000000000001, 0, 0.5, 1, 1.000000000000001)] double s,
        [Values(1, 1.000000000000001)] double l) 
    {
        var included = ColourDescription.White;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues);
        excluded.AddRange(ColourDescription.Saturations);
        excluded.AddRange(ColourDescription.Lightnesses);
        excluded.AddRange([ColourDescription.Black, ColourDescription.Grey]);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void SaturationNotApplicable(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(double.NaN)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l)
    {
        var included = ColourDescription.NotApplicable;
        List<ColourDescription> excluded = [];
        excluded.AddRange(ColourDescription.Hues);
        excluded.AddRange(ColourDescription.Saturations);
        excluded.AddRange(ColourDescription.Lightnesses);
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void SaturationGrey(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(-0.000000000000001, 0)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Grey;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange([ColourDescription.Black, ColourDescription.White]);
        excluded.AddRange(ColourDescription.Hues);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void SaturationFaint(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(0.000000000000001, 0.199999999999999)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Faint;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Saturations.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void SaturationWeak(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(0.2, 0.399999999999999)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Weak;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Saturations.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void SaturationMild(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(0.4, 0.599999999999999)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Mild;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Saturations.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void SaturationStrong(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(0.6, 0.799999999999999)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Strong;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Saturations.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void SaturationVibrant(
        [Values(-1, 0, 180, 360, 361)] double h,
        [Values(0.8, 1, 1.000000000000001)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Vibrant;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Saturations.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueNotApplicable(
        [Values(double.NaN)] double h,
        [Values(-0.000000000000001, 0, 0.5, 1, 1.000000000000001)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l)
    {
        var included = ColourDescription.NotApplicable;
        List<ColourDescription> excluded = [];
        excluded.AddRange(ColourDescription.Hues);
        excluded.AddRange(ColourDescription.Saturations);
        excluded.AddRange(ColourDescription.Lightnesses);
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueRed(
        [Values(345, 0, 14.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Red;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }

    [Test, Combinatorial]
    public void HueOrange(
        [Values(15, 44.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Orange;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueYellow(
        [Values(45, 74.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Yellow;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueChartreuse(
        [Values(75, 104.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Chartreuse;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueGreen(
        [Values(105, 134.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Green;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueMint(
        [Values(135, 164.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Mint;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueCyan(
        [Values(165, 194.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Cyan;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueAzure(
        [Values(195, 224.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Azure;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueBlue(
        [Values(225, 254.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Blue;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueViolet(
        [Values(255, 284.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Violet;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueMagenta(
        [Values(285, 314.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Magenta;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }
    
    [Test, Combinatorial]
    public void HueRose(
        [Values(315, 344.9999999999999)] double h,
        [Values(0.000000000000001, 0.5, 1)] double s,
        [Values(0.000000000000001, 0.5, 0.999999999999999)] double l) 
    {
        var included = ColourDescription.Rose;
        List<ColourDescription> excluded = [ColourDescription.NotApplicable];
        excluded.AddRange(ColourDescription.Hues.Except([included]));
        excluded.AddRange(ColourDescription.Greyscales);
        AssertDescription(h, s, l, included, excluded);
    }

    private static void AssertDescription(double h, double s, double l, ColourDescription included, List<ColourDescription> excluded)
    {
        List<double> hues = [h, h + 360, h - 360];
        foreach (var hue in hues)
        {
            var colour = new Unicolour(ColourSpace.Hsl, hue, s, l);
            Assert.That(colour.Description.Split(" ").Length, Is.LessThanOrEqualTo(3));
            Assert.That(colour.Description, Contains.Substring(included.ToString()));
            
            foreach (var excludedItem in excluded)
            {
                Assert.That(colour.Description, !Contains.Substring(excludedItem.ToString()));
            }
        }
    }
}