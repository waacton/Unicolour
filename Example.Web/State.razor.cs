using Microsoft.AspNetCore.Components;

using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Example.Web;

// global static properties arguably unpleasant
// but seems fine for a small app with a small number of items of shared state
// not worth the effort of state injection or cascading callbacks
public partial class State : ComponentBase
{
    static State()
    {
        Colour = new Unicolour(Config, "ff1493");
    }
    
    public static readonly Configuration NoConfig = new(iccConfig: new IccConfiguration(profile: null, "⚠️ Uncalibrated CMYK"));
    public static Configuration Config { get; private set; } = NoConfig;
    
    private static Unicolour colour = null!;
    public static Unicolour Colour
    {
        get => colour;
        private set
        {
            colour = value;
            UpdateColourProperties();
            OnColourChange?.Invoke();
        }
    }
    
    private static string busyMessage = string.Empty;
    public static string BusyMessage
    {
        get => busyMessage;
        private set
        {
            busyMessage = value;
            OnBusyChange?.Invoke();
        }
    }
    
    private static string cssInsideGamut;
    private static string cssOutsideGamut;
    public static Unicolour ColourInGamut { get; private set; } = null!;
    public static ConversionResult ConversionResult { get; private set; }
    public static bool IsBusy => !string.IsNullOrEmpty(BusyMessage);

    public static event Action? OnColourChange;
    public static event Action? OnBusyChange;

    private static void UpdateColourProperties()
    {
        ColourInGamut = colour.MapToRgbGamut(GamutMap.RgbClipping);
        cssInsideGamut = Utils.ToCss(colour, 100);
        cssOutsideGamut = Utils.ToCss(colour, colour.IsInRgbGamut ? 100 : 50);
        
        if (Utils.HasConversionError(colour))
        {
            ConversionResult = ConversionResult.Error;
        }
        else if (!colour.IsInRgbGamut)
        {
            ConversionResult = ConversionResult.OutOfGamut;
        }
        else
        {
            ConversionResult = ConversionResult.InGamut;
        }
    }
    
    public static void UpdateColour(ColourSpace colourSpace, double first, double second, double third)
    {
        Colour = new Unicolour(Config, colourSpace, first, second, third); 
    }
    
    public static void UpdateColour(Pigment[] pigments, double[] weights)
    {
        Colour = new Unicolour(Config, pigments, weights); 
    }
    
    public static void UpdateColour(Channels channels)
    {
        Colour = new Unicolour(Config, channels); 
    }

    public static void UpdateConfig(Configuration config)
    {
        Config = config;
        colour = colour.ConvertToConfiguration(config); // avoid triggering onColourChange; colour isn't actually changing
    }

    public static void SetBusy(string message)
    {
        BusyMessage = message.ToUpper();
    }
    
    public static void ClearBusy()
    {
        BusyMessage = string.Empty;
    }
}

public enum ConversionResult
{
    InGamut,
    OutOfGamut,
    Error
}