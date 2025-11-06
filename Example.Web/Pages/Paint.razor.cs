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
        ArtistPaint.QuinacridoneMagenta,
        ArtistPaint.QuinacridoneRed,
        ArtistPaint.PyrroleRed,
        ArtistPaint.CadmiumRedLight,
        ArtistPaint.PyrroleOrange,
        ArtistPaint.CadmiumOrange,
        ArtistPaint.DiarylideYellow,
        ArtistPaint.HansaYellowOpaque,
        ArtistPaint.BismuthVanadateYellow,
        ArtistPaint.PhthaloGreenYellowShade,
        ArtistPaint.PhthaloGreenBlueShade,
        ArtistPaint.CeruleanBlueChromium,
        ArtistPaint.CobaltBlue,
        ArtistPaint.UltramarineBlue,
        ArtistPaint.PhthaloBlueGreenShade,
        ArtistPaint.PhthaloBlueRedShade,
        ArtistPaint.DioxazinePurple,
        ArtistPaint.BoneBlack,
        ArtistPaint.TitaniumWhite
    ];

    private static readonly List<Pigment> Pigments = [];
    private static readonly List<string> Names = [];
    // private static readonly List<string> axes = ["R", "Y", "B"]; // TODO: can slider axis text be merged with label, and positioned differently for long names?
    private static readonly List<Unicolour> Colours = [];
    private static readonly List<SliderSolidColour> Sliders = [];

    protected override void OnInitialized()
    {
        Pigments.Clear();
        Names.Clear();
        Colours.Clear();
        Sliders.Clear();

        AddPigment(ArtistPaint.QuinacridoneRed);
        AddPigment(ArtistPaint.HansaYellowOpaque);
        AddPigment(ArtistPaint.CobaltBlue);

        SetSliderValue(Sliders[0], 1.0);
    }
    
    private static double ParseValue(ChangeEventArgs args) => double.Parse((args.Value == null ? string.Empty : args.Value.ToString()) ?? string.Empty);
    private static void SetSliderValue(SliderSolidColour slider, ChangeEventArgs args) => SetSliderValue(slider, ParseValue(args));
    private static void SetSliderValue(SliderSolidColour slider, double value)
    {
        slider.Value = value;
        SetColour();
    }

    private static void SetColour()
    {
        State.Colour = new Unicolour(Pigments.ToArray(), Sliders.Select(x => x.Value).ToArray());
    }

    private static void AddPigment(Pigment pigment)
    {
        var colour = ColourLookup[pigment];
        var name = NameLookup[pigment];
        
        Pigments.Add(pigment);
        Colours.Add(colour);
        Names.Add(name);
        Sliders.Add(new SliderSolidColour(colour, name, string.Empty));
    }

    private static void RemovePigment(Pigment pigment)
    {
        var colour = ColourLookup[pigment];
        var name = NameLookup[pigment];
        var slider = Sliders.Single(x => x.LabelText == name);
        
        Pigments.Remove(pigment);
        Colours.Remove(colour);
        Names.Remove(name);
        Sliders.Remove(slider);
    }

    private static void TogglePigment(Pigment pigment)
    {
        if (Pigments.Contains(pigment))
        {
            RemovePigment(pigment);
            SetColour();
        }
        else
        {
            AddPigment(pigment);
        }
    }

    private static bool IsSelected(Pigment pigment) => Pigments.Contains(pigment);
}