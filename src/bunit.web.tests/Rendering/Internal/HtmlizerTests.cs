namespace Bunit.Rendering.Internal
{
	using Microsoft.AspNetCore.Components;
	using Microsoft.AspNetCore.Components.Rendering;
	using Microsoft.AspNetCore.Components.Web;
	using Xunit;

	public class HtmlizerTests : ComponentTestFixture
	{
		public static TheoryData GetOnClickData()
		{
			return new TheoryData<bool, bool>
			{
				{ false, true },
				{ true, false },
				{ true, true }
			};
		}

		[Theory(DisplayName =
			"The component is rendered without internal Blazor attributes, " +
			"regardless of the PreventDefault and StopPropagation settings.")]
		[MemberData(nameof(GetOnClickData))]
		public void Test001(bool stopPropagation, bool preventDefault)
		{
			//Arrange
			var component = RenderComponent<Htmlizer01Component>(
				EventCallback<MouseEventArgs>(nameof(Htmlizer01Component.OnClick), OnClickCallback),
				Parameter(nameof(Htmlizer01Component.OnClickStopPropagation), stopPropagation),
				Parameter(nameof(Htmlizer01Component.OnClickPreventDefault), preventDefault));

			//Act

			//Assert
			component.MarkupMatches("<button type=\"button\">Click me!</button>");
			void OnClickCallback(MouseEventArgs e)
			{
				// NOTE: This line is only for the completeness of the EventCallback
				Assert.NotNull(e);
			}
		}

		[Theory(DisplayName = "The component contains correctly prefixed internal attributes.")]
		[MemberData(nameof(GetOnClickData))]
		public void Test002(bool stopPropagation, bool preventDefault)
		{
			//Arrange
			var component = RenderComponent<Htmlizer01Component>(
				EventCallback<MouseEventArgs>(nameof(Htmlizer01Component.OnClick), OnClickCallback),
				Parameter(nameof(Htmlizer01Component.OnClickStopPropagation), stopPropagation),
				Parameter(nameof(Htmlizer01Component.OnClickPreventDefault), preventDefault));

			//Act
			var button = component.Find("button");

			//Assert
			Assert.Equal(stopPropagation, button.HasAttribute("blazor:__internal_stopPropagation_onclick"));
			Assert.Equal(preventDefault, button.HasAttribute("blazor:__internal_preventDefault_onclick"));

			void OnClickCallback(MouseEventArgs e)
			{
				// NOTE: This line is only for the completeness of the EventCallback
				Assert.NotNull(e);
			}
		}

		private class Htmlizer01Component : ComponentBase
		{
			[Parameter]
			public EventCallback<MouseEventArgs> OnClick { get; set; }

			[Parameter]
			public bool OnClickPreventDefault { get; set; }

			[Parameter]
			public bool OnClickStopPropagation { get; set; }

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
