using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Shouldly;
using Xunit;

namespace Bunit.Rendering.Internal
{
	public class HtmlizerTests : TestContext
	{
		[Theory(DisplayName = "The component contains correctly prefixed internal attributes.")]
		[InlineData(false, true)]
		[InlineData(true, false)]
		[InlineData(true, true)]
		public void Test002(bool stopPropagation, bool preventDefault)
		{
			//Arrange
			var component = RenderComponent<Htmlizer01Component>(parameters => parameters
				.Add(p => p.OnClick, (MouseEventArgs e) => { })
				.Add(p => p.OnClickStopPropagation, stopPropagation)
				.Add(p => p.OnClickPreventDefault, preventDefault)
			);

			//Act
			var button = component.Find("button");

			//Assert
			button.HasAttribute(Htmlizer.ToBlazorAttribute("__internal_stopPropagation_onclick")).ShouldBe(stopPropagation);
			button.HasAttribute(Htmlizer.ToBlazorAttribute("__internal_preventDefault_onclick")).ShouldBe(preventDefault);
		}

		private class Htmlizer01Component : ComponentBase
		{
			[Parameter]
			public EventCallback<MouseEventArgs> OnClick { get; set; }

			[Parameter]
			public bool OnClickPreventDefault { get; set; }

			[Parameter]
			public bool OnClickStopPropagation { get; set; }

			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{
				base.BuildRenderTree(builder);

				builder.OpenElement(0, "button");
				builder.AddAttribute(1, "type", "button");

				if (OnClick.HasDelegate)
				{
					builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(this, OnClick));
					builder.AddEventStopPropagationAttribute(3, "onclick", OnClickStopPropagation);
					builder.AddEventPreventDefaultAttribute(4, "onclick", OnClickPreventDefault);
				}

				builder.AddContent(5, "Click me!");

				builder.CloseElement();
			}
		}
	}
}
