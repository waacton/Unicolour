using Microsoft.AspNetCore.Components;

namespace Wacton.Unicolour.Example.Web;

public partial class LightSpaces : ComponentBase
{
    private ColourSpace colourSpace = ColourSpace.Rgb255;
    private readonly Slider[] sliders = [new(0), new(1), new(2)];

    private static ColourSpace[] ColourSpaceOptions = ColourLookup.RangeLookup.Keys.OrderBy(x => x.ToString()).ToArray();
    
    protected override void OnInitialized()
    {
        SetAllSliders();

        State.OnChange += () =>
        {
            SetAllSliders();
            StateHasChanged();
        };
    }

    private void SetAllSliders()
    {
        var (first, second, third) = State.Colour.GetRepresentation(colourSpace);
        sliders[0].Value = first;
        sliders[1].Value = second;
        sliders[2].Value = third;
        UpdateSliders();
    }

    private void SetColourSpace(ChangeEventArgs args)
    {
        var colourSpaceName = (args.Value == null ? string.Empty : args.Value.ToString()) ?? string.Empty;
        _ = Enum.TryParse(colourSpaceName, out colourSpace);
        
        SetAllSliders();
        SetColour();
    }

    private static double ParseValue(ChangeEventArgs args) => double.Parse((args.Value == null ? string.Empty : args.Value.ToString()) ?? string.Empty);
    private void SetSliderValue(Slider slider, ChangeEventArgs args) => SetSliderValue(slider, ParseValue(args));
    private void SetSliderValue(Slider slider, double value)
    {
        slider.Value = value;
        UpdateSliders();
        SetColour();
    }
    
    private void UpdateSliders()
    {
        for (var i = 0; i < sliders.Length; i++)
        {
            sliders[i].ColourSpace = colourSpace;
            var startColour = new Unicolour(colourSpace, GetStartValue(0), GetStartValue(1), GetStartValue(2));
            var endColour = new Unicolour(colourSpace, GetEndValue(0), GetEndValue(1), GetEndValue(2));

            sliders[i].Stops = GetColourStops(startColour, endColour);
            continue;

            double GetStartValue(int index) => index == i ? sliders[index].Min : sliders[index].Value;
            double GetEndValue(int index) => index == i ? sliders[index].Max : sliders[index].Value;
        }
    }
    
    private List<Unicolour> GetColourStops(Unicolour start, Unicolour end)
    {
        var sections = colourSpace is ColourSpace.Hct or ColourSpace.Munsell ? 8 : 16;
        var stops = new List<Unicolour> { start };
        for (var i = 1; i < sections; i++)
        {
            var distance = 1.0 / sections * i;
            stops.Add(start.Mix(end, colourSpace, distance, HueSpan.Increasing));
        }

        stops.Add(end);
        return stops;
    }

    private void SetColour()
    {
        State.Colour = new Unicolour(colourSpace, sliders[0].Value, sliders[1].Value, sliders[2].Value);
    }
}