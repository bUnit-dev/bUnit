using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bunit.TestAssets.SampleComponents
{
	public class AllTypesOfParams<TItem> : ComponentBase
	{
		[Inject]
		public IJSRuntime? JsRuntime { get; set; }

		[Parameter(CaptureUnmatchedValues = true)]
		public IReadOnlyDictionary<string, object> Attributes { get; set; } = default!;

		[Parameter]
		public string? RegularParam { get; set; }

		[CascadingParameter]
		public int? UnnamedCascadingValue { get; set; }

		[CascadingParameter(Name = nameof(NamedCascadingValue))]
		public int? NamedCascadingValue { get; set; }

		[Parameter]
		public EventCallback NonGenericCallback { get; set; }

		[Parameter]
		public EventCallback<EventArgs> GenericCallback { get; set; }

		[Parameter]
		public RenderFragment? ChildContent { get; set; }

		[Parameter]
		public RenderFragment? OtherContent { get; set; }

		[Parameter]
		public RenderFragment<TItem>? ItemTemplate { get; set; }

		public int? NoParameterProperty { get; set; }

		public int DummyMethod()
		{
			return 42;
		}
	}
}
