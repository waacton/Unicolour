using Microsoft.AspNetCore.Components;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Example.Web.Pages;

public partial class Paint : ComponentBase
{
    // private readonly static Pigment redPigment = PigmentGenerator.From(new("#FF0000"));
    // private readonly static Pigment yellowPigment = PigmentGenerator.From(new("#FFFF00"));
    // private readonly static Pigment bluePigment = PigmentGenerator.From(new("#0000FF"));
    
        private static Dictionary<Pigment, string> NameLookup = new()
    {
        { ArtistPaint.BoneBlack, "Bone Black" },
        { ArtistPaint.TitaniumWhite, "Titanium White" },
        { ArtistPaint.BismuthVanadateYellow, "Bismuth Vanadate Yellow" },
        { ArtistPaint.HansaYellowOpaque, "Hansa Yellow" },
        { ArtistPaint.DiarylideYellow, "Diarylide Yellow" },
        { ArtistPaint.CadmiumOrange, "Cadmium Orange" },
        { ArtistPaint.PyrroleOrange, "Pyrrole Orange" },
        { ArtistPaint.CadmiumRedLight, "Cadmium Red Light" },
        { ArtistPaint.PyrroleRed, "Pyrrole Red" },
        { ArtistPaint.QuinacridoneRed, "Quinacridone Red" },
        { ArtistPaint.QuinacridoneMagenta, "Quinacridone Magenta" },
        { ArtistPaint.DioxazinePurple, "Dioxazine Purple" },
        { ArtistPaint.PhthaloBlueRedShade, "Phthalo Blue (Red Shade)" },
        { ArtistPaint.PhthaloBlueGreenShade, "Phthalo Blue (Green Shade)" },
        { ArtistPaint.UltramarineBlue, "Ultramarine Blue" },
        { ArtistPaint.CobaltBlue, "Cobalt Blue" },
        { ArtistPaint.CeruleanBlueChromium, "Cerulean Blue Chromium" },
        { ArtistPaint.PhthaloGreenBlueShade, "Phthalo Green (Blue Shade)" },
        { ArtistPaint.PhthaloGreenYellowShade, "Phthalo Green (Yellow Shade)" }
    };

    private static Dictionary<Pigment, Unicolour> ColourLookup = new()
    {
        { ArtistPaint.BoneBlack, GetSinglePigmentColour(ArtistPaint.BoneBlack) },
        { ArtistPaint.TitaniumWhite, GetSinglePigmentColour(ArtistPaint.TitaniumWhite) },
        { ArtistPaint.BismuthVanadateYellow, GetSinglePigmentColour(ArtistPaint.BismuthVanadateYellow) },
        { ArtistPaint.HansaYellowOpaque, GetSinglePigmentColour(ArtistPaint.HansaYellowOpaque) },
        { ArtistPaint.DiarylideYellow, GetSinglePigmentColour(ArtistPaint.DiarylideYellow) },
        { ArtistPaint.CadmiumOrange, GetSinglePigmentColour(ArtistPaint.CadmiumOrange) },
        { ArtistPaint.PyrroleOrange, GetSinglePigmentColour(ArtistPaint.PyrroleOrange) },
        { ArtistPaint.CadmiumRedLight, GetSinglePigmentColour(ArtistPaint.CadmiumRedLight) },
        { ArtistPaint.PyrroleRed, GetSinglePigmentColour(ArtistPaint.PyrroleRed) },
        { ArtistPaint.QuinacridoneRed, GetSinglePigmentColour(ArtistPaint.QuinacridoneRed) },
        { ArtistPaint.QuinacridoneMagenta, GetSinglePigmentColour(ArtistPaint.QuinacridoneMagenta) },
        { ArtistPaint.DioxazinePurple, GetSinglePigmentColour(ArtistPaint.DioxazinePurple) },
        { ArtistPaint.PhthaloBlueRedShade, GetSinglePigmentColour(ArtistPaint.PhthaloBlueRedShade) },
        { ArtistPaint.PhthaloBlueGreenShade, GetSinglePigmentColour(ArtistPaint.PhthaloBlueGreenShade) },
        { ArtistPaint.UltramarineBlue, GetSinglePigmentColour(ArtistPaint.UltramarineBlue) },
        { ArtistPaint.CobaltBlue, GetSinglePigmentColour(ArtistPaint.CobaltBlue) },
        { ArtistPaint.CeruleanBlueChromium, GetSinglePigmentColour(ArtistPaint.CeruleanBlueChromium) },
        { ArtistPaint.PhthaloGreenBlueShade, GetSinglePigmentColour(ArtistPaint.PhthaloGreenBlueShade) },
        { ArtistPaint.PhthaloGreenYellowShade, GetSinglePigmentColour(ArtistPaint.PhthaloGreenYellowShade) },
    };
    
    private static Unicolour GetSinglePigmentColour(Pigment pigment) => new Unicolour([pigment], [1]).MapToRgbGamut(GamutMap.RgbClipping);

    private static readonly Pigment[] allPigments =
    [
        ArtistPaint.BismuthVanadateYellow,
        ArtistPaint.HansaYellowOpaque,
        ArtistPaint.DiarylideYellow,
        ArtistPaint.CadmiumOrange,
        ArtistPaint.PyrroleOrange,
        ArtistPaint.CadmiumRedLight,
        ArtistPaint.PyrroleRed,
        ArtistPaint.QuinacridoneRed,
        ArtistPaint.QuinacridoneMagenta,
        ArtistPaint.DioxazinePurple,
        ArtistPaint.PhthaloBlueRedShade,
        ArtistPaint.PhthaloBlueGreenShade,
        ArtistPaint.UltramarineBlue,
        ArtistPaint.CobaltBlue,
        ArtistPaint.CeruleanBlueChromium,
        ArtistPaint.PhthaloGreenYellowShade,
        ArtistPaint.PhthaloGreenBlueShade,
        ArtistPaint.BoneBlack,
        ArtistPaint.TitaniumWhite,
    ];
    
    private static readonly Pigment[] pigments = [ArtistPaint.QuinacridoneRed, ArtistPaint.HansaYellowOpaque, ArtistPaint.CobaltBlue];
    private static readonly string[] names = ["Quinacridone Red", "Hansa Yellow", "Cobalt Blue"];
    private static readonly string[] axes = ["R", "Y", "B"]; // TODO: can slider axis text be merged with label, and positioned differently for long names?
    private static readonly Unicolour[] colours = pigments.Select(x => ColourLookup![x]).ToArray();
    private readonly SliderSolidColour[] sliders = Enumerable.Range(0, pigments.Length).Select(CreateSlider).ToArray();

    private static SliderSolidColour CreateSlider(int index) => new(colours[index], names[index], axes[index]);
    
    protected override void OnInitialized()
    {
        SetSliderValue(sliders[0], 1.0);
    }
    
    private static double ParseValue(ChangeEventArgs args) => double.Parse((args.Value == null ? string.Empty : args.Value.ToString()) ?? string.Empty);
    private void SetSliderValue(SliderSolidColour slider, ChangeEventArgs args) => SetSliderValue(slider, ParseValue(args));
    private void SetSliderValue(SliderSolidColour slider, double value)
    {
        slider.Value = value;
        SetColour();
    }

    private void SetColour()
    {
        State.Colour = new Unicolour(pigments, sliders.Select(x => x.Value).ToArray());
    }
}