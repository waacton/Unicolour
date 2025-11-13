using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Example.Web;

// arguably unpleasant but seems fine for a small app with a couple of items of shared state
// not worth the effort of state injection or cascading callbacks
public static class State
{
    public static event Action? OnColourChange;
    private static Unicolour colour = new("ff1493");
    public static Unicolour Colour
    {
        get => colour;
        private set
        {
            colour = value;
            OnColourChange?.Invoke();
        }
    }
    
    public static void Update(ColourSpace colourSpace, double first, double second, double third)
    {
        Colour = new Unicolour(Config, colourSpace, first, second, third); 
    }
    
    public static void Update(Pigment[] pigments, double[] weights)
    {
        Colour = new Unicolour(Config, pigments, weights); 
    }
    
    public static void Update(Channels channels)
    {
        Colour = new Unicolour(Config, channels); 
    }
    
    public static readonly Configuration NoConfig = new(iccConfig: new IccConfiguration(profile: null, "⚠️ Uncalibrated CMYK"));
    public static Configuration Config { get; private set; } = NoConfig;
    public static void Update(Configuration config)
    {
        Config = config;
        colour = colour.ConvertToConfiguration(config); // avoid triggering onColourChange; colour isn't actually changing
    }
        
    public static event Action? OnBusyChange;
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

    public static bool IsBusy => !string.IsNullOrEmpty(BusyMessage);

    public static void SetBusy(string message)
    {
        BusyMessage = message.ToUpper();
    }
    
    public static void ClearBusy()
    {
        BusyMessage = string.Empty;
    }
}