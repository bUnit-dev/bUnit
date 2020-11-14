#if NET5_0
using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Shouldly;
using Xunit;

namespace Bunit.JSInterop.ComponentSupport
{
	public class VirtualizeTest : TestContext
	{
		private readonly AutoFixture.Fixture TestData = new AutoFixture.Fixture();

		[Theory(DisplayName = "Can render component using <Virtualize>")]
		[InlineData(0)]
		[InlineData(7)]
		[InlineData(30)]
		[InlineData(500)]
		[InlineData(10000)]
		public void Test001(int itemsInDataSource)
		{
			var cut = RenderComponent<VirtualizedList>(ps => ps.Add(p => p.Items, TestData.CreateMany<string>(itemsInDataSource).ToArray()));

			cut.FindAll("li").Count.ShouldBe(itemsInDataSource);
		}

		class VirtualizedList : ComponentBase
		{
			[Parameter] public ICollection<string> Items { get; set; } = Array.Empty<string>();

			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{
				builder.OpenElement(0, "ul");
				builder.OpenComponent<Virtualize<string>>(10);
				builder.AddAttribute(10, "Items", Items);
				builder.AddAttribute(11, "ChildContent", (RenderFragment<string>)(item => b2 =>
				{
					b2.OpenElement(20, "li");
					b2.AddContent(21, item);
					b2.CloseElement();
				}));
				builder.CloseComponent();
				builder.CloseElement();
			}
		}
	}
}

#endif
