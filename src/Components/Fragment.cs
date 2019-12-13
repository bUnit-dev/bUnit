using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class Fragment : FragmentBase
    {
        [Parameter] public string Id { get; set; } = string.Empty;
    }
}
