using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Example.Web.Pages;

public partial class Print : ComponentBase
{
    private Profile? profile = null;
    private Configuration config = new(iccConfig: new IccConfiguration(null, "Uncalibrated CMYK"));
    private readonly List<SliderGradientColour> sliders = [];

    protected override void OnInitialized()
    {
        CreateSliders();
        UpdateSliderGradients();
    }
    
    private async Task SetProfile(InputFileChangeEventArgs args)
    {
        var file = args.File;
        await using var stream = file.OpenReadStream(maxAllowedSize: 32_768_000); // 32,000 KB - following that default 512000 == 500 KB
        var memory = new Memory<byte>(new byte[file.Size]);
        _ = await stream.ReadAsync(memory);

        profile = new Profile(memory.ToArray(), file.Name);
        config = new Configuration(iccConfig: new IccConfiguration(profile, profile.Name));
        
        ConvertColourState();
        CreateSliders();
        UpdateSliderGradients();
    }
    
    private void CreateSliders()
    {
        sliders.Clear();

        var axes = profile == null
            ? ["C", "M", "Y", "K"] // uncalibrated
            : Utils.IccAxes(profile.Header.DataColourSpace);
        
        for (var i = 0; i < axes.Length; i++)
        {
            var value = State.Colour.Icc.Values[i];
            var slider = new SliderGradientColour
            {
                Value = value,
                ValueText = $"{value:F2}",
                LabelText = axes[i],
                Range = new(0, 1),
                Step = 0.01
            };
            
            sliders.Add(slider);
        }
    }

    private static double ParseValue(ChangeEventArgs args) => double.Parse((args.Value == null ? string.Empty : args.Value.ToString()) ?? string.Empty);
    private void SetSliderValue(SliderGradientColour slider, ChangeEventArgs args) => SetSliderValue(slider, ParseValue(args));
    private void SetSliderValue(SliderGradientColour slider, double value)
    {
        slider.Value = value;
        UpdateSliderGradients();
        UpdateColourState();
    }
    
    private void UpdateSliderGradients()
    {
        var baseline = sliders.Select(x => x.Value).ToArray();

        for (var i = 0; i < sliders.Count; i++)
        {
            var slider = sliders[i];
            
            const double stopCount = 4;
            List<Unicolour> stops = [];
            for (var stop = 0; stop <= stopCount; stop++)
            {
                var values = baseline.ToArray();
                values[i] = stop / stopCount;
                stops.Add(GetColour(values));
            }

            slider.Stops = stops.ToArray();
        }
    }

    private Unicolour GetColour(double[] values) => new(config, new Channels(values));
    
    private void ConvertColourState()
    {
        State.Colour = State.Colour.ConvertToConfiguration(config);
    }
    
    private void UpdateColourState()
    {
        State.Colour = GetColour(sliders.Select(x => x.Value).ToArray());
    }
}