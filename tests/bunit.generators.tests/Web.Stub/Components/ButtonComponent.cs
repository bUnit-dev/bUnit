using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Bunit.Web.Stub.Components;

public class ButtonComponent : ComponentBase
{
	[Parameter] public string Text { get; set; }
	[Parameter] public EventCallback OnClick { get; set; }
	[Required] public string Unused { get; set; }
}
