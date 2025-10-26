namespace Wacton.Unicolour.Example.Web;

// arguably unpleasant but seems fine for a small app with 1 item of shared state
// not worth the effort of state injection or cascading callbacks
public static class State
{
    private static Unicolour colour = new("ff1493");
    public static Unicolour Colour
    {
        get => colour;
        set
        {
            colour = value;
            OnChange?.Invoke();
        }
    }

    public static event Action? OnChange;
}