using Microsoft.AspNetCore.Components;

namespace Wacton.Unicolour.Example.Web;

public partial class LightPicker : ComponentBase
{
    private ColourSpace colourSpace = ColourSpace.Rgb255;
    private readonly SliderGradientColour[] sliders = [new(), new(), new()];

    private static ColourSpace[] ColourSpaceOptions = ColourLookup.RangeLookup.Keys.OrderBy(x => x.ToString()).ToArray();
    
    protected override void OnInitialized()
    {
        UpdateSliderProperties();
        UpdateSliderGradients();

        State.OnChange += () =>
        {
            UpdateSliderProperties();
            UpdateSliderGradients();
            StateHasChanged();
        };
    }
    
    private void SetColourSpace(ChangeEventArgs args)
    {
        var colourSpaceName = (args.Value == null ? string.Empty : args.Value.ToString()) ?? string.Empty;
        _ = Enum.TryParse(colourSpaceName, out colourSpace);
        
        UpdateSliderProperties();
        UpdateSliderGradients();
        UpdateColourState();
    }

    private static double ParseValue(ChangeEventArgs args) => double.Parse((args.Value == null ? string.Empty : args.Value.ToString()) ?? string.Empty);
    private void SetSliderValue(SliderGradientColour slider, ChangeEventArgs args) => SetSliderValue(slider, ParseValue(args));
    private void SetSliderValue(SliderGradientColour slider, double value)
    {
        slider.Value = value;
        UpdateSliderGradients();
        UpdateColourState();
    }
    
    private void UpdateSliderProperties()
    {
        var (first, second, third) = State.Colour.GetRepresentation(colourSpace);
        UpdateSlider(0, first);
        UpdateSlider(1, second);
        UpdateSlider(2, third);
    }

    private void UpdateSlider(int index, double value)
    {
        var slider = sliders[index];
        var range = ColourLookup.RangeLookup[colourSpace][index];

        slider.Value = value;
        slider.Range = range;
        slider.Step = GetStep();
        slider.ValueText = GetValueText();
        slider.AxisText = ColourLookup.AxisLookup[colourSpace][index];
        return;

        double GetStep()
        {
            // this approach could be used for all spaces and triplet indexes for much finer control
            if (colourSpace == ColourSpace.Munsell && index == 0)
            {
                return 0.36; // 0.1 munsell hue
            }
        
            return range.Distance switch
            {
                < 0.5 => 0.001,
                < 5 => 0.01,
                < 50 => 0.1,
                _ => 1
            };
        }
            
        string GetValueText()
        {
            // this approach could be used for all spaces and triplet indexes for much finer control
            if (colourSpace == ColourSpace.Munsell && index == 0)
            {
                var hue = Hue.ToMunsell(value);
                return $"{hue.number:F1}{hue.letter}";
            }
        
            return range.Distance switch
            {
                < 0.5 => $"{value:F3}",
                < 5 => $"{value:F2}",
                < 50 => $"{value:F1}",
                _ => $"{value:F0}"
            };
        }
    }
    
    private void UpdateSliderGradients()
    {
        for (var i = 0; i < sliders.Length; i++)
        {
            var slider = sliders[i];
            var startColour = new Unicolour(colourSpace, GetStartValue(0), GetStartValue(1), GetStartValue(2));
            var endColour = new Unicolour(colourSpace, GetEndValue(0), GetEndValue(1), GetEndValue(2));
            var stopCount = colourSpace switch
            {
                ColourSpace.Hct => 8,
                ColourSpace.Munsell => 16,
                _ => 48
            };
            
            slider.Stops = startColour.Palette(endColour, colourSpace, stopCount, HueSpan.Increasing).ToArray();
            continue;

            double GetStartValue(int index) => index == i ? sliders[index].Min : sliders[index].Value;
            double GetEndValue(int index) => index == i ? sliders[index].Max : sliders[index].Value;
        }
    }

    private void UpdateColourState()
    {
        State.Colour = new Unicolour(colourSpace, sliders[0].Value, sliders[1].Value, sliders[2].Value);
    }
}