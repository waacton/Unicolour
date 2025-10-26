using Microsoft.AspNetCore.Components;

namespace Wacton.Unicolour.Example.Web.Pages;

public partial class Light : ComponentBase
{
    private bool powerMode;
    
    private void TogglePower()
    {
        powerMode = !powerMode;
    }
}