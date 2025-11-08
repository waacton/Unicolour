using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Example.Web.Pages;

public partial class Print : ComponentBase
{
    private readonly List<SliderGradientColour> sliders = [];

    protected override void OnInitialized()
    {
        CreateSliders();
        UpdateSliderGradients();
        
        // TODO: include this if wanting to show light picker at the same time
        //       ... but think about handling everything as response to OnColourChange, not just this
        // State.OnColourChange += () =>
        // {
        //     var profile = State.Config.Icc.Profile;
        //     var axes = profile == null
        //         ? ["C", "M", "Y", "K"] // uncalibrated
        //         : Utils.IccAxes(profile.Header.DataColourSpace);
        //
        //     for (var i = 0; i < axes.Length; i++)
        //     {
        //         var value = State.Colour.Icc.Values[i];
        //         sliders[i].Value = value;
        //         sliders[i].ValueText = $"{value:F2}";
        //     }
        //
        //     UpdateSliderGradients();
        //     
        //     StateHasChanged();
        // };
    }
    
    private async Task SetProfile(InputFileChangeEventArgs args)
    {
        var file = args.File;
        await using var stream = file.OpenReadStream(maxAllowedSize: 32_768_000); // 32,000 KB - following that default 512000 == 500 KB
        var memory = new Memory<byte>(new byte[file.Size]);
        _ = await stream.ReadAsync(memory);

        var profile = new Profile(memory.ToArray(), file.Name);
        var config = new Configuration(iccConfig: new IccConfiguration(profile, profile.Name));
        
        State.Update(config);
        CreateSliders();
        UpdateSliderGradients();
    }
    
    private void CreateSliders()
    {
        sliders.Clear();

        var profile = State.Config.Icc.Profile;
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
        slider.ValueText = $"{value:F2}";
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

    private static Unicolour GetColour(double[] values) => new(State.Config, new Channels(values));
    
    private void UpdateColourState()
    {
        State.Update(new Channels(sliders.Select(x => x.Value).ToArray()));
    }
}