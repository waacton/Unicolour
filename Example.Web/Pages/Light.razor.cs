using Microsoft.AspNetCore.Components;

namespace Wacton.Unicolour.Example.Web.Pages;

public partial class Light : ComponentBase
{
    private bool dualMode;
    
    private void ToggleDualMode()
    {
        dualMode = !dualMode;
    }
}