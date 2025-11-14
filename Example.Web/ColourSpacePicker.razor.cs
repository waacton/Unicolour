using Microsoft.AspNetCore.Components;

namespace Wacton.Unicolour.Example.Web;

public partial class ColourSpacePicker : ComponentBase
{
    private ColourSpace colourSpace = ColourSpace.Rgb255;
    private readonly GradientColourSlider[] sliders = [new(), new(), new()];

    private static ColourSpace[] ColourSpaceOptions = Utils.SpaceToRange.Keys.OrderBy(space => space.ToString()).ToArray();
    
    protected override void OnInitialized()
    {
        UpdateSliderProperties();
        UpdateSliderGradients();

        State.OnColourChange += () =>
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
    private void SetSliderValue(GradientColourSlider slider, ChangeEventArgs args) => SetSliderValue(slider, ParseValue(args));
    private void SetSliderValue(GradientColourSlider slider, double value)
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
        var range = Utils.SpaceToRange[colourSpace][index];

        double step;
        string valueText;
        
        // this approach could be used for explicit, granular control over how sliders behave for each axis in each space
        // but default fallbacks are generally good enough for this example
        if (colourSpace == ColourSpace.Munsell && index == 0)
        {
            step = 0.36;

            var hue = Hue.ToMunsell(value);
            valueText = $"{hue.number:F1}{hue.letter}";
        }
        else
        {
            step = range.DefaultStep;
            valueText = range.DefaultValueString(value);
        }

        slider.Value = value;
        slider.ValueText = valueText;
        slider.LabelText = Utils.SpaceToAxes[colourSpace][index];
        slider.Range = range;
        slider.Step = step;
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

    private void UpdateColourState() => State.Update(colourSpace, sliders[0].Value, sliders[1].Value, sliders[2].Value);
}