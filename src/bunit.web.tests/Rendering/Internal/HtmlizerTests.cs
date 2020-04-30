namespace Bunit.Rendering.Internal
{
	using Microsoft.AspNetCore.Components;
	using Microsoft.AspNetCore.Components.Rendering;
	using Microsoft.AspNetCore.Components.Web;
	using Xunit;

	public class HtmlizerTests : ComponentTestFixture
	{
		[Theory(DisplayName =
			"The component is rendered without internal Blazor attributes, " +
			"regardless of the PreventDefault and StopPropagation settings.")]
		[InlineData(false, false)]
		[InlineData(false, true)]
		[InlineData(true, false)]
		[InlineData(true, true)]
		public void Test001(bool stopPropagation, bool preventDefault)
		{
			//Arrange
			var component = RenderComponent<Test001Component>(
				EventCallback<MouseEventArgs>(nameof(Test001Component.OnClick), OnClickCallback),
				Parameter(nameof(Test001Component.OnClickStopPropagation), stopPropagation),
				Parameter(nameof(Test001Component.OnClickPreventDefault), preventDefault));

			//Act

			//Assert
			component.MarkupMatches("<button type=\"button\">Click me!</button>");
			void OnClickCallback(MouseEventArgs e) => Assert.NotNull(e);
		}

		private class Test001Component : ComponentBase
		{
			[Parameter]
			public EventCallback<MouseEventArgs> OnClick { get; set; }

			[Parameter]
			public bool OnClickStopPropagation { get; set; }

			[Parameter]
			public bool OnClickPreventDefault { get; set; }

			/// <inheritdoc />
			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{
				base.BuildRenderTree(builder);

				builder.OpenElement(0, "button");
				builder.AddAttribute(1, "type", "button");

				if (OnClick.HasDelegate)
				{
					builder.AddAttribute(2, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, OnClick));
					builder.AddEventStopPropagationAttribute(3, "onclick", OnClickStopPropagation);
					builder.AddEventPreventDefaultAttribute(4, "onclick", OnClickPreventDefault);
				}

				builder.AddContent(5, "Click me!");

				builder.CloseElement();
			}
		}
	}
}
