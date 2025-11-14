using Microsoft.AspNetCore.Components;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Example.Web;

public partial class PigmentPicker : ComponentBase
{
    // TODO: single-constant approximation variant?
    // private readonly static Pigment redPigment = PigmentGenerator.From(new("#FF0000"));
    // private readonly static Pigment yellowPigment = PigmentGenerator.From(new("#FFFF00"));
    // private readonly static Pigment bluePigment = PigmentGenerator.From(new("#0000FF"));

    private static readonly Pigment[] AllPigments =
    [
        ArtistPaint.QuinacridoneMagenta, ArtistPaint.QuinacridoneRed, ArtistPaint.PyrroleRed, ArtistPaint.CadmiumRedLight,
        ArtistPaint.PyrroleOrange, ArtistPaint.CadmiumOrange,
        ArtistPaint.DiarylideYellow, ArtistPaint.HansaYellowOpaque, ArtistPaint.BismuthVanadateYellow,
        ArtistPaint.PhthaloGreenYellowShade, ArtistPaint.PhthaloGreenBlueShade,
        ArtistPaint.CeruleanBlueChromium, ArtistPaint.CobaltBlue, ArtistPaint.UltramarineBlue, ArtistPaint.PhthaloBlueGreenShade, ArtistPaint.PhthaloBlueRedShade,
        ArtistPaint.DioxazinePurple,
        ArtistPaint.BoneBlack, ArtistPaint.TitaniumWhite
    ];
    
    private static readonly Dictionary<Pigment, SolidColourSlider> PigmentToSlider = new();

    protected override void OnInitialized()
    {
        PigmentToSlider.Clear();

        AddPigment(ArtistPaint.QuinacridoneRed);
        AddPigment(ArtistPaint.HansaYellowOpaque);
        AddPigment(ArtistPaint.CobaltBlue);

        SetSliderValue(PigmentToSlider[ArtistPaint.QuinacridoneRed], 1.0);
        SetSliderValue(PigmentToSlider[ArtistPaint.HansaYellowOpaque], 0.0);
        SetSliderValue(PigmentToSlider[ArtistPaint.CobaltBlue], 0.0);
    }
    
    private static double ParseValue(ChangeEventArgs args) => double.Parse((args.Value == null ? string.Empty : args.Value.ToString()) ?? string.Empty);
    private static void SetSliderValue(SolidColourSlider slider, ChangeEventArgs args) => SetSliderValue(slider, ParseValue(args));
    private static void SetSliderValue(SolidColourSlider slider, double value)
    {
        slider.Value = value;
        slider.ValueText = $"{value:F2}";
        SetColour();
    }

    private static void SetColour()
    {
        var pigments = PigmentToSlider.Keys.ToArray();
        var weights = PigmentToSlider.Values.Select(x => x.Value).ToArray();
        State.Update(pigments, weights);
    }

    private static void AddPigment(Pigment pigment)
    {
        var slider = new SolidColourSlider
        {
            Colour = Utils.PigmentToColour[pigment].MapToRgbGamut(GamutMap.RgbClipping), 
            LabelText = Utils.PigmentToName[pigment], 
            Range = new(0, 1), 
            Step = 0.01
        };

        PigmentToSlider.Add(pigment, slider);
    }
    
    private static void RemovePigment(Pigment pigment)
    {
        PigmentToSlider.Remove(pigment);
    }

    private static bool IsDisplayed(Pigment pigment) => PigmentToSlider.ContainsKey(pigment);
}